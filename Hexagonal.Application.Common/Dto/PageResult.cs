using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace Hexagonal.Application.Common.Dto
{
    /// <summary>
    /// Page result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T> : IBaseDto
    {
        private PageResult(IEnumerable<T> items)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Meta = new MetaObject(items.Count());
            // ReSharper disable once PossibleMultipleEnumeration
            Items = items;
        }

        /// <summary>
        /// Gets or sets the meta object
        /// </summary>
        [JsonPropertyName("_meta")]
        public MetaObject Meta { [UsedImplicitly] get; }

        /// <summary>
        /// Gets or sets the value object
        /// </summary>
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { [UsedImplicitly] get; }

        /// <summary>
        /// Factory method creates an instance of page result
        /// </summary>
        /// <param name="items">List to convert to paged result</param>

        /// <returns>Paged result</returns>
        public static PageResult<T> Create(IEnumerable<T> items) => new PageResult<T>(items);
    }
}
