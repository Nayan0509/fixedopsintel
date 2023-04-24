using System.ComponentModel.DataAnnotations;

namespace WoodenAutomative.Domain.Models
{
    public class otp
    {
        [Key]
        public int Id { get; set; }
        public string OTPNumber { get; set; }
        public DateTime ValidTill { get; set; }
        public DateTime SendingTime { get; set; }
        public string AuthorizeFor { get; set; }
        public string AuthorizationType { get; set; }
        public bool IsVerify { get; set; }
    }
}
