namespace Hexagonal.Application.Common.Functional.Option
{
    /// <summary>
    /// Implementation of None option monad
    /// </summary>
    /// <typeparam name="T">The type of option</typeparam>
    public sealed class None<T> : Option<T>, IEquatable<None<T>>, IEquatable<None>
    {
        /// <inheritdoc />
	    public override Option<TResult> Map<TResult>(Func<T, TResult> map) =>
            None.Value;

        /// <inheritdoc />
        public override Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) =>
            None.Value;

        /// <inheritdoc />
        public override T Reduce(T whenNone) =>
            whenNone;

        /// <inheritdoc />
        public override T Reduce(Func<T> whenNone) =>
            whenNone();

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            !(obj is null) && ((obj is None<T>) || (obj is None));

        /// <inheritdoc />
        public override int GetHashCode() => 0;

        /// <inheritdoc />
        public bool Equals(None<T>? other) => true;

        /// <inheritdoc />
        public bool Equals(None? other) => true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(None<T>? a, None<T>? b) =>
            (a is null && b is null) ||
            (!(a is null) && a.Equals(b));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(None<T> a, None<T> b) => !(a == b);

        /// <inheritdoc />
        public override string ToString() => "None";
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class None : IEquatable<None>
    {
        /// <summary>
        /// 
        /// </summary>
        public static None Value { get; } = new None();

        private None() { }

        /// <inheritdoc />
        public override string ToString() => "None";

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            !(obj is null) && ((obj is None) || IsGenericNone(obj.GetType()));

        private bool IsGenericNone(Type type) =>
            type.GenericTypeArguments.Length == 1 &&
            typeof(None<>).MakeGenericType(type.GenericTypeArguments[0]) == type;

        /// <inheritdoc />
        public bool Equals(None? other) => true;

        /// <inheritdoc />
        public override int GetHashCode() => 0;

    }
}
