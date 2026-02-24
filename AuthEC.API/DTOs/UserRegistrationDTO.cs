using System.ComponentModel.DataAnnotations;

namespace AuthEC.API.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
