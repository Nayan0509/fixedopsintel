using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace WoodenAutomative.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        
        public string PasswordHash { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsDeleted { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastPasswordModifiedDate { get; set; }

        public DateTime? LastLoginTime { get; set; }
        
        public DateTime ModifiedDate { get; set; }


    }
}
