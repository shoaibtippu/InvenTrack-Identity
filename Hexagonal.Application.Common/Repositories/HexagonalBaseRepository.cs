using Hexagonal.Application.Common.Dto;
using Hexagonal.Application.Common.Functional.Either;
using Hexagonal.Application.Common.Traits;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Hexagonal.Application.Common.Exceptions.EntityExceptions;
using Hexagonal.Application.Common.Extensions;

namespace Hexagonal.Application.Common.Repositories
{
    /// <summary>
    ///     Defines general CRUD operations performed by repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class HexagonalBaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TKey : notnull
    {
        #region Ctor

        private readonly DbContext _dbContext;
        private readonly ILogger _logger;
        private readonly DbSet<TEntity> _dbSet;

        public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? Include { get; set; }

        public HexagonalBaseRepository(DbContext dbContext, ILogger logger,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            _dbContext = dbContext;
            _logger = logger;
            this._dbSet = dbContext.Set<TEntity>();
            this.Include = include;
        }
        #endregion

        #region GetAllPaginated

        /// <summary>
        /// </summary>
        /// <param name="pageParameters"></param>
        /// <param name="pageParametersExpression"></param>
        /// <param name="documentFilterExpression"></param>
        /// <param name="order">the order by expression</param>
        /// <returns></returns>

        protected virtual async Task<List<TEntity>> GetAll(PageParameters? pageParameters,
            Expression<Func<TEntity, bool>> pageParametersExpression,
            List<Expression<Func<TEntity, bool>>>? documentFilterExpression,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? order = null)
        {
            var query = this._dbSet.AsNoTracking();
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Cast<ISoftDelete>().FilterSoftDelete().Cast<TEntity>();
            }

            if (documentFilterExpression != null)
                query = documentFilterExpression.Aggregate(query, (current, expression) => current.Where(expression));

            if (Include != null) query = Include(query);

            if (order != null) query = order(query);

            if (pageParameters != null)
                query = query.PaginateList(pageParameters.Term, pageParameters.Skip, pageParameters.Top,
                    pageParametersExpression);

            return await query.ToListAsync();
        }
        #endregion

        #region GetAll

        /// <summary>
        /// Asynchronously retrieves all entities from the database. If TEntity implements ISoftDelete, it excludes soft-deleted entities.
        /// </summary>
        /// <returns>A collection of entities.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var query = this._dbSet.AsNoTracking();

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Cast<ISoftDelete>().FilterSoftDelete().Cast<TEntity>();

            }
            return await query.ToListAsync();
        }
        #endregion

        #region GetById

        /// <summary>
        ///     Defines get by Id Document
        /// </summary>
        /// <param name="id">the id of entity</param>
        /// <returns>Either monad as repository error or entity</returns>
        public virtual async Task<Either<RepositoryError, TEntity>> Get(TKey id)
        {
            var query = this._dbSet.AsTracking(QueryTrackingBehavior.TrackAll);

            if (this.Include != null) query = this.Include(query);

            var entity = await query.FirstOrDefaultAsync(t => t.Id.Equals(id));

            return entity switch
            {
                null => new EntityNotFound(),
                ISoftDelete softDeleteEntity when softDeleteEntity.IsSoftDeleted() => new EntitySoftDeleted(),
                _ => entity
            };
        }


        #endregion

        #region Create

        /// <summary>
        ///     Inserts entity into database
        /// </summary>
        /// <param name="entity">the entity that needs to be inserted into database</param>
        /// <returns></returns>

        public virtual async Task<Either<RepositoryError, TEntity>> Create(TEntity entity)
        {
            if (entity is ITrackable trackableEntity)
            {
                MarkAsCreated(trackableEntity);
            }
            this._dbSet.Add(entity);
            try
            {
                int result = await this._dbContext.SaveChangesAsync();
                if (result < 1) this._logger.LogWarning("no entity is added to database {@result}", result);
                return entity;
            }
            catch (DbUpdateException e) when (e.InnerException is SqlException sqlException)
            {
                await this._dbContext.Entry(entity).ReloadAsync();

                return entity switch
                {
                    ISoftDelete softDeleteEntity when softDeleteEntity.IsSoftDeleted() => new EntitySoftDeleted(),
                    _ => new EntityConflict()
                };
            }
        }

        #endregion

        #region Update

        /// <summary>
        ///     Updates a entity in repository
        /// </summary>
        /// <param name="id">the id of entity</param>
        /// <param name="entity">the entity that needs to be updated</param>
        /// <returns>Either monad as repository error or updated entity</returns>

        public virtual async Task<Either<RepositoryError, TEntity>> Update(TKey id, TEntity entity)
        {
            {
                var existingEntity = await this._dbSet.FindAsync(id);

                switch (existingEntity)
                {
                    case null:
                        return new EntityNotFound();
                    case ISoftDelete softDeleteEntity when softDeleteEntity.IsSoftDeleted():
                        return new EntitySoftDeleted();
                }
            }
            if (entity is ITrackable trackableEntity)
            {
                MarkAsUpdated(trackableEntity);
            }
            this._dbContext.Attach(entity);
            this._dbContext.Entry(entity).State = EntityState.Modified;
            this._dbContext.Entry(entity).Property(nameof(ITrackable.CreatedAt)).IsModified = false;
            this._dbContext.Entry(entity).Property(nameof(ITrackable.CreatedBy)).IsModified = false;
            try
            {
                int result = await this._dbContext.SaveChangesAsync();
                if (result < 1) this._logger.LogWarning("no entity is updated to database {@result}", result);
                return entity;
            }

            catch (DbUpdateException e) when (e.InnerException is SqlException sqlException)
            {

                var conflictEntity = await this._dbSet.AsQueryable().FirstOrDefaultAsync(t => t.Id.Equals(entity.Id));
                return conflictEntity switch
                {
                    ISoftDelete softDeleteEntity when softDeleteEntity.IsSoftDeleted() => new EntitySoftDeleted(),
                    _ => new EntityConflict()
                };
            }
        }
        #endregion

        #region Delete

        /// <summary>
        ///     Deletes an instance of entity
        /// </summary>
        /// <param name="id">The id of entity to be deleted</param>
        /// <returns>Either monad as repository error or deleted entity</returns>

        public virtual async Task<Either<RepositoryError, TEntity>> Delete(TKey id)
        {
            var entityToDelete = await this._dbSet.FindAsync(id);

            switch (entityToDelete)
            {
                case null:
                    return new EntityNotFound();
                case INotRemovableEntity notRemovableEntity when notRemovableEntity.IsNotRemovable():
                    return new EntityNotRemovable();
                case ISoftDelete softDeleteEntity when softDeleteEntity.IsSoftDeleted():
                    return new EntitySoftDeleted();
                case ISoftDelete softDeleteEntity:
                    MarkAsSoftDelete(softDeleteEntity, entityToDelete);
                    this._dbContext.Entry(entityToDelete).State = EntityState.Modified;
                    break;
                default:
                    this._dbContext.Entry(entityToDelete).State = EntityState.Deleted;
                    break;
            }

            int result = await this._dbContext.SaveChangesAsync();
            if (result < 1) this._logger.LogWarning("no entity is updated to database {@result}", result);
            return entityToDelete;
        }

        #endregion

        #region MarkSoftDelete

        private void MarkAsSoftDelete(ISoftDelete softDeleteEntity, TEntity entity)
        {
            if (entity is ITrackable trackableEntity)
            {
                this._dbContext.Entry(trackableEntity)
                    .CurrentValues[nameof(ITrackable.UpdatedAt)] = DateTime.Now;
                this._dbContext.Entry(trackableEntity)
                    .CurrentValues[nameof(ITrackable.UpdatedBy)] = Guid.NewGuid();

            }
            if (!softDeleteEntity.IsSoftDeleted())
            {
                this._dbContext.Entry(softDeleteEntity)
                    .CurrentValues[nameof(ISoftDelete.Deleted)] = DateTime.Now; ;
                this._dbContext.Entry(softDeleteEntity)
                    .CurrentValues[nameof(ISoftDelete.DeletedBy)] = Guid.NewGuid();
                return;
            }

            if (softDeleteEntity is IEntity<Guid> mvEntity)
                this._logger.LogInformation("Entity is already marked as soft delete {id}, Entity {entity}", mvEntity.Id,
                    mvEntity.GetType().Name);
        }

        #endregion

        #region  MarkCreated

        private void MarkAsCreated(ITrackable trackableEntity)
        {
            this._dbContext.Entry(trackableEntity)
                .CurrentValues[nameof(ITrackable.CreatedAt)] = DateTime.Now;
            this._dbContext.Entry(trackableEntity)
                .CurrentValues[nameof(ITrackable.CreatedBy)] = Guid.NewGuid();
        }

        #endregion

        #region MarkUpdated

        private void MarkAsUpdated(ITrackable trackableEntity)
        {
            this._dbContext.Entry(trackableEntity)
                .CurrentValues[nameof(ITrackable.UpdatedAt)] = DateTime.Now;
            this._dbContext.Entry(trackableEntity)
                .CurrentValues[nameof(ITrackable.UpdatedBy)] = Guid.NewGuid();
        }

        #endregion
    }
}
