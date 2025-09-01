using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DocCheck.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MinLength(3)]
        [MaxLength(36)]
        public string? FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(36)]
        public string? MiddleName { get; set; }

        [MinLength(3)]
        [MaxLength(36)]
        public string? LastName { get; set; }


        [MaxLength(36)]
        public string? BitrixId { get; set; }

        public string Name => $"{FirstName} {LastName}";
    }

}
