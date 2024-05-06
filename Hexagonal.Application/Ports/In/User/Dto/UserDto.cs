using System.ComponentModel.DataAnnotations;

namespace InvenTrack.Application.Ports.In.User.Dto
{
    public class UserDto
    {
        public Guid? Id { get; set; } = Guid.Empty;

        [Required]
        public string? Name { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; } = String.Empty;
    }
}
