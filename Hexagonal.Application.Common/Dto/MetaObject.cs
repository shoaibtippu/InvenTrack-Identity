using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Hexagonal.Application.Common.Dto
{
    /// <summary>
    /// Represents default list result
    /// </summary>
    [DebuggerDisplay("TotalCount: {" + nameof(TotalCount) + "}")]
    public class MetaObject : IBaseDto
    {
        /// <summary>
        /// Returns total number of items in result
        /// </summary>
        [JsonPropertyName("_totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Returns total number of items in result
        /// </summary>
        [JsonPropertyName("_totalAllRecords")]
        public int TotalAllRecords { get; set; }

        /// <summary>
        /// Initializes meta object
        /// </summary>
        /// <param name="totalCount"></param>
        public MetaObject(int totalCount)
        {
            TotalCount = totalCount;
        }

        /// <summary>
        /// Initializes meta object
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="count"></param>
        public MetaObject(int totalCount, int count)
        {
            TotalCount = totalCount;
            TotalAllRecords = count;
        }
    }
}
