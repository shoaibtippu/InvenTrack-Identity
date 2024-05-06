namespace Hexagonal.Application.Common.Traits
{
    public interface ITrackable : IEntity
    {
        /// <summary>
        /// Gets the created Instant
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        /// Gets the User who created this entity
        /// </summary>
        public Guid? CreatedBy { get; }

        /// <summary>
        /// Gets the updated Instant
        /// </summary>
        public DateTime? UpdatedAt { get; }

        /// <summary>
        /// Gets the user who updated this entity
        /// </summary>
        public Guid? UpdatedBy { get; }
    }
}
