using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Request.OTP
{
    public class ForgotPasswordOTPRequest
    {
        public int Digit1 { get; set; }
        public int Digit2 { get; set; }
        public int Digit3 { get; set; }
        public int Digit4 { get; set; }
        public int Digit5 { get; set; }
        public int Digit6 { get; set; }
        public string Email { get; set; }
    }
}
