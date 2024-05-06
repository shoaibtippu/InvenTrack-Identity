namespace Hexagonal.Application.Common.Functional.Either
{
    /// <summary>
    /// Represents Right monad
    /// </summary>
    /// <typeparam name="TLeft">The left object (generally represents error/wrong result)</typeparam>
    /// <typeparam name="TRight">The right object (generally represents correct result)</typeparam>
    public class Right<TLeft, TRight> : Either<TLeft, TRight>
    {
        private TRight Content { get; }

        /// <summary>
        /// Construct right monad
        /// </summary>
        /// <param name="content">right object</param>
        public Right(TRight content) => Content = content;

        /// <summary>
        /// Maps Right monad to right object
        /// </summary>
        ///  <param name="obj"></param> 
        public static implicit operator TRight(Right<TLeft, TRight> obj) => obj.Content;
    }
}
