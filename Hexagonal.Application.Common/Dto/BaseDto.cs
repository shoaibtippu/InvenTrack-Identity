using System.Text.Json.Serialization;

namespace Hexagonal.Application.Common.Dto
{
    /// <summary>
    /// Represents general base DTO class
    /// </summary>
    public abstract class BaseDto : BaseDto<string>
    {

    }

    /// <summary>
    ///  Represents general base DTO class
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseDto<TKey> : IBaseDto where TKey : notnull
    {
        /// <summary>
        /// Represents unique identifier
        /// </summary>
        [JsonPropertyName("id")]
        public TKey Id { get; set; } = default!;
    }
}
