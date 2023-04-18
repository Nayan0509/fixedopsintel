using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Request.Password
{
    public class SetPasswordRequest
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public string ConfrimPassword { get; set; }
    }
}
