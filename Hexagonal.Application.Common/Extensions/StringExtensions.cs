namespace Hexagonal.Application.Common.Extensions
{
    /// <summary>
    /// Defines general string extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Maps a string to Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str) => Guid.Parse(str);

        /// <summary>
        /// Maps a nullable string to Nullable Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid? ToNullableGuid(this string str) => string.IsNullOrWhiteSpace(str) ? null : Guid.Parse(str);
    }
}
