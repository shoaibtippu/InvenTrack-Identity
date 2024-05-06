namespace Hexagonal.Application.Common.Traits
{
    /// <summary>
    /// Default interface that represents Soft Delete
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or Sets Soft Deleted flag
        /// </summary>
        public DateTime? Deleted { get; }

        /// <summary>
        /// Gets the User who deleted this entity
        /// </summary>
        public Guid? DeletedBy { get; }

        /// <summary>
        /// Checks if entity is deleted
        /// </summary>
        /// <returns>true if deleted, false if not deleted</returns>
        public bool IsSoftDeleted() => Deleted.HasValue;
    }
}
