using Microsoft.AspNetCore.Identity;

namespace InvenTrack.Application.Models
{
    public class User : IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        #region TrackableProperties
        public DateTime? CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        #endregion
    }
}
