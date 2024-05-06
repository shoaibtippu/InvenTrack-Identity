using Hexagonal.Application.Common.Traits;

namespace InvenTrack.Application.Common.Models
{
    public abstract class Base : ISoftDelete, IEntity<Guid>
    {
        #region private backing Fields requird by EF core
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// backing field for deleted
        /// </summary>
        protected DateTime? deleted;

        /// <summary>
        /// backing field for deleted by
        /// </summary>
        protected Guid? deletedBy;

        // ReSharper restore InconsistentNaming
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id"></param>
        protected Base(Guid id) => Id = id;
        protected Base() { }
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        /// <inheritdoc />
        public DateTime? Deleted => this.deleted;


        /// <inheritdoc />
        public Guid? DeletedBy => this.deletedBy;
    }
}
