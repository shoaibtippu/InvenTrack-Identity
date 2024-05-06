namespace Hexagonal.Application.Common.Traits
{
    /// <summary>
    /// Base interface for a document/entity
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// Base interface for a document/entity
    /// </summary>
    public interface IEntity<TKey> : IEntity where TKey : notnull
    {
        /// <summary>
        /// Gets Id of entity/Document
        /// </summary>
        public TKey Id { get; }
    }
}
