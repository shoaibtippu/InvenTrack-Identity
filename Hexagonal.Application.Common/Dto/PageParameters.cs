using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Hexagonal.Application.Common.Dto
{
    /// <summary>
    /// Represents page parameters
    /// </summary>
    [DebuggerDisplay("Skip: {" + nameof(Skip) + "}, Top: {" + nameof(Top) + "}, Term: {" + nameof(Term) + "}")]
    public class PageParameters : IBaseDto
    {
        /// <summary>
        /// Will skip the first 'N' results, where N is the value of 'skip'
        /// </summary>
        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        /// <summary>
        /// Returns a result set of at most 'N' elements, where 'N' is the value assigned to the parameter 'take'.
        /// </summary>
        [JsonPropertyName("top")]
        public int? Top { get; set; }

        /// <summary>
        /// Filters the result to only containing users to whom the 'userName' property contains what is defined in the 'Term' parameter.
        /// </summary>
        [JsonPropertyName("term")]
        public string? Term { get; set; }
    }
}
