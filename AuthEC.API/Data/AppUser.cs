using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthEC.API.Data
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName ="nvarchar(150)")]
        public string? FullName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(10)")]
        public string? Gender { get; set; }

        [PersonalData]
        public DateOnly DOB { get; set; }

        [PersonalData]
        public int? LibraryId { get; set; }
    }
}
