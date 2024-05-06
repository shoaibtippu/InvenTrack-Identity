using Hexagonal.Application.Common.Traits;

namespace InvenTrack.Application.Common.Models
{
    public abstract class Trackable : Base, ITrackable, IEntity
    {
        #region private backing Fields requird by EF core
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// backing field for created At
        /// </summary>
        protected DateTime? createdAt;

        /// <summary>
        /// backing field for created by
        /// </summary>
        protected Guid? createdBy;

        /// <summary>
        ///  backing fields or updated at
        /// </summary>
        protected DateTime? updatedAt;

        /// <summary>
        /// backing field for updated by
        /// </summary>
        protected Guid? updatedBy;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id"></param>
        protected Trackable(Guid id) : base(id) { }
        protected Trackable() { }
        /// <inheritdoc />
        public DateTime? CreatedAt => this.createdAt;


        /// <inheritdoc />
        public Guid? CreatedBy => this.createdBy;


        /// <inheritdoc />
        public DateTime? UpdatedAt => this.updatedAt;

        /// <inheritdoc />
        public Guid? UpdatedBy => this.updatedBy;
    }
}
