namespace Hexagonal.Application.Common.Functional.Option
{
    /// <summary>
    /// Represents option monad
    /// </summary>
    /// <typeparam name="T">the type of option object</typeparam>
    public abstract class Option<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Option<T>(T value) =>
            new Some<T>(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="none"></param>
        public static implicit operator Option<T>(None none) =>
            new None<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public abstract Option<TResult> Map<TResult>(Func<T, TResult> map);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>

        public abstract Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whenNone"></param>
        /// <returns></returns>
        public abstract T Reduce(T whenNone);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whenNone"></param>
        /// <returns></returns>
        public abstract T Reduce(Func<T> whenNone);
    }
}
