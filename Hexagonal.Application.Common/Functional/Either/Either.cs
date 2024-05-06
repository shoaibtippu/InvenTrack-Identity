namespace Hexagonal.Application.Common.Functional.Either
{
    /// <summary>
    /// Represents Either Monad
    /// </summary>
    /// <typeparam name="TLeft">The left object (generally represents error/wrong result)</typeparam>
    /// <typeparam name="TRight">The right object (generally represents correct result)</typeparam>
    public abstract class Either<TLeft, TRight>
    {
        /// <summary>
        /// Constructs an either left monad
        /// </summary>
        /// <param name="obj">Left monad</param>
        public static implicit operator Either<TLeft, TRight>(TLeft obj) =>
            new Left<TLeft, TRight>(obj);

        /// <summary>
        /// Constructs an either right monad
        /// </summary>
        /// <param name="obj">Right monad</param>
        public static implicit operator Either<TLeft, TRight>(TRight obj) =>
            new Right<TLeft, TRight>(obj);
    }
}
