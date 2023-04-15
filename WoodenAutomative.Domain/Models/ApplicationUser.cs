using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace WoodenAutomative.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastPasswordModifiedDate { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
