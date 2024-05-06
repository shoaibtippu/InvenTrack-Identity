using Hexagonal.Application.Common.Traits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Hexagonal.Application.Common.Repositories
{
    /// <inheritdoc />
    public class HexagonalCrudRepository<TEntity> : HexagonalBaseRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {
        /// <inheritdoc />
        public HexagonalCrudRepository(DbContext dbContext, ILogger logger,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
            : base(dbContext, logger, include)
        {
        }
    }
}