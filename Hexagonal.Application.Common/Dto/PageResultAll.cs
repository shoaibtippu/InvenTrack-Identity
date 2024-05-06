using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace Hexagonal.Application.Common.Dto
{
    /// <summary>
    /// Represents a paged result.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the paged result.</typeparam>
    /// <typeparam name="TCount">The type of the count without skip and top filter.</typeparam>
    public class PageResultAll<TItem, TCount> : IBaseDto
    {
        private PageResultAll(IEnumerable<TItem> withFilter, TCount withOutFilter)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Meta = new MetaObject(withFilter.Count(), Convert.ToInt32(withOutFilter));
            // ReSharper disable once PossibleMultipleEnumeration
            Items = withFilter;
        }

        /// <summary>
        /// Gets the metadata for the paged result.
        /// </summary>
        [JsonPropertyName("_meta")]
        public MetaObject Meta { [UsedImplicitly] get; }

        /// <summary>
        /// Gets the items in the paged result.
        /// </summary>
        [JsonPropertyName("items")]
        public IEnumerable<TItem> Items { [UsedImplicitly] get; }

        /// <summary>
        /// Creates a new instance of the <see cref="PageResultAll{TItem, TCount}"/> class.
        /// </summary>
        /// <param name="items">The items to include in the paged result.</param>
        /// <param name="withOutFilter">The count without skip and top filter.</param>
        /// <returns>A new <see cref="PageResultAll{TItem, TCount}"/> instance.</returns>
        public static PageResultAll<TItem, TCount> Create(IEnumerable<TItem> items, TCount withOutFilter)
            => new PageResultAll<TItem, TCount>(items, withOutFilter);
    }
}
