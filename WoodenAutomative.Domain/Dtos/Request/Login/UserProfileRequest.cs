using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Request.Login
{
    public class UserProfileRequest
    {
        [StringLength(500)]
        public string Id { get; set; }
        [Display(Name = "First Name")]
        [StringLength(500)]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(500)]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [StringLength(256)]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Mobile Number")]
        [StringLength(256)]
        [Required]
        public string PhoneNumber { get; set; }

        public string ModifiedBy { get; set; }

    }
}
