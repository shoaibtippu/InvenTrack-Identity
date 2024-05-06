namespace Hexagonal.Application.Common.Functional.Either
{
    public class Left<TLeft, TRight> : Either<TLeft, TRight>
    {
        private TLeft Content { get; }

        /// <summary>
        /// Construct left monad
        /// </summary>
        /// <param name="content">left object</param>
        public Left(TLeft content) => Content = content;

        /// <summary>
        /// Maps Left monad to left object
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator TLeft(Left<TLeft, TRight> obj) => obj.Content;
    }
}
