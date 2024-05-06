namespace Hexagonal.Application.Common.Traits
{
    /// <summary>
    /// Interface represents an entity that is not removable
    /// </summary>
    public interface INotRemovableEntity : IEntity
    {
        /// <summary>
        /// True: Item is system level and cannot be removed
        /// </summary>
        bool NotRemovable { get; set; }

        /// <summary>
        /// Checks if entity is removable or not
        /// </summary>
        /// <returns>true if not removable, false entity can be deleted</returns>
        public bool IsNotRemovable() => NotRemovable;
    }
}
