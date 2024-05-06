namespace Hexagonal.Application.Common.Functional.Option
{
    /// <summary>
    /// Implementation of Some option monad
    /// </summary>
    /// <typeparam name="T">The type of option</typeparam>
    public sealed class Some<T> : Option<T>, IEquatable<Some<T>>
    {
        /// <summary>
        /// Represents underlying some content
        /// </summary>
        private T Content { get; }

        /// <inheritdoc />
        public Some(T value)
        {
            Content = value;
        }

        /// <summary>
        /// Maps some object to value
        /// </summary>
        /// <param name="some">value of ome object</param>
        /// <returns>returns underlying content</returns>
        public static implicit operator T(Some<T> some) =>
            some.Content;

        /// <summary>
        /// Creates instance of some object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Some<T>(T value) =>
            new Some<T>(value);

        /// <inheritdoc />
        public override Option<TResult> Map<TResult>(Func<T, TResult> map) =>
            map(Content);

        /// <inheritdoc />
        public override Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) =>
            map(Content);

        /// <inheritdoc />
        public override T Reduce(T whenNone) =>
            Content;

        /// <inheritdoc />
        public override T Reduce(Func<T> whenNone) =>
            Content;

        /// <inheritdoc />
        public override string ToString() =>
            $"Some({(Content != null ? Content.ToString() : "<null>")})";

        /// <inheritdoc />
        public bool Equals(Some<T>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Content, other.Content);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Some<T> some && Equals(some);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Content != null ? EqualityComparer<T>.Default.GetHashCode(Content) : 0;

        /// <summary>
        /// Compares two sum objects
        /// </summary>
        /// <param name="a">first some object</param>
        /// <param name="b">second some object</param>
        /// <returns>result of equality</returns>
        public static bool operator ==(Some<T>? a, Some<T>? b) =>
            a is null && b is null ||
            !(a is null) && a.Equals(b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">first some object</param>
        /// <param name="b">second some object</param>
        /// <returns>result of not equal</returns>
        public static bool operator !=(Some<T> a, Some<T> b) => !(a == b);
    }
}
