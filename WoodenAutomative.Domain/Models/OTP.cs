using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Models
{
    public class OTP
    {
        [Key]
        public int Id { get; set; }

        public int OTPNumber { get; set; }

        public DateTime ValidTill { get; set; }

        public DateTime SendingTime { get; set; }

        public string AuthorizeFor { get; set; }

        public string AuthorizationType { get; set; }
    }
}
