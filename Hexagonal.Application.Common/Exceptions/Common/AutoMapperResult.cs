using AutoMapper;
using Hexagonal.Application.Common.Exceptions.EntityExceptions;
using Hexagonal.Application.Common.Functional.Either;

namespace Hexagonal.Application.Common.Exceptions.Common
{
    public class AutoMapperResult
    {
        private readonly IMapper _mapper;

        public AutoMapperResult(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TResult GetResult<TEntity, TResult>(
            Either<RepositoryError, TEntity> response)
        {
            if (response is Right<RepositoryError, TEntity> UserRight)
            {
                TEntity obj = UserRight;
                return _mapper.Map<TResult>(obj);
            }
            ThrowErrorIfAny(response);
            return default!;
        }
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public List<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return _mapper.Map<List<TDestination>>(source);
        }

        public List<TDestination> MapResult<TSource, TDestination>(List<TSource> source)
        {
            return _mapper.Map<List<TDestination>>(source);
        }

        public void GetEmptyResult<TEntity>(Either<RepositoryError, TEntity> response)
        {
            ThrowErrorIfAny(response);
        }

        private void ThrowErrorIfAny<TEntity>(Either<RepositoryError, TEntity> response)
        {
            if (response is Left<RepositoryError, TEntity> left)
            {
                throw left;
            }
        }
    }
}
