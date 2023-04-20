using WoodenAutomative.Domain.Dtos.Request.Email;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IEmailRepository
    {
       public bool SendEmail(EmailData emailData);
       public Task<bool> SendOTP(string userId);
        public Task<bool> InsertOTP(string email, string AuthorizationType, string OtpValue);
    }
}
