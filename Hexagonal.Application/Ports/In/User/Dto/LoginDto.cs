using System.ComponentModel.DataAnnotations;

namespace InvenTrack.Application.Ports.In.User.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = String.Empty;
    }
}
