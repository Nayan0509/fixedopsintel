using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Request.ChangePassword
{
    public class ChangePasswordRequest
    {
        [Display(Name ="Current Password")]
        public string CurrentPassword { get; set; }
        [Display(Name = "New Password")]

        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]

        public string ConfirmNewPassword { get; set; }
    }
}
