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
        public string? Gender { get; set; }
        public string? Role { get; set; }
        public DateOnly DOB { get; set; }
        public int? LibraryId { get; set; }
    }
}
