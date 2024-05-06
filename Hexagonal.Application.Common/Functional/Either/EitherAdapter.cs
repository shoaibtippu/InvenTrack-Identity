namespace Hexagonal.Application.Common.Functional.Either
{
    /// <summary>
    /// Either extension methods
    /// </summary>
    public static class EitherAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TNewRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Either<TLeft, TNewRight> Map<TLeft, TRight, TNewRight>(
            this Either<TLeft, TRight> either, Func<TRight, TNewRight> map) =>
            either is Right<TLeft, TRight> right
                ? (Either<TLeft, TNewRight>)map(right)
                : (TLeft)(Left<TLeft, TRight>)either;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TNewRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Either<TLeft, TNewRight> Map<TLeft, TRight, TNewRight>(
            this Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TNewRight>> map) =>
            either is Right<TLeft, TRight> right
                ? map(right)
                : (TLeft)(Left<TLeft, TRight>)either;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static TRight Reduce<TLeft, TRight>(
            this Either<TLeft, TRight> either, Func<TLeft, TRight> map) =>
            either is Left<TLeft, TRight> left
                ? map(left)
                : (Right<TLeft, TRight>)either;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="map"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static Either<TLeft, TRight> Reduce<TLeft, TRight>(
            this Either<TLeft, TRight> either, Func<TLeft, TRight> map,
            Func<TLeft, bool> when) =>
            either is Left<TLeft, TRight> bound && when(bound)
                ? map(bound)
                : either;
    }
}
