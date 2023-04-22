using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WoodenAutomative.Domain.Dtos.Request.Email;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using static System.Net.WebRequestMethods;

namespace WoodenAutomative.EntityFramework.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WoodenAutomativeContext _context;
        private readonly IConfiguration _configuration;


        public EmailRepository(UserManager<ApplicationUser> userManager,
                               IConfiguration configuration, 
                               WoodenAutomativeContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public bool SendEmail(EmailData emailData)
        {
            using (var emailClient = new SmtpClient())
            {
                try
                {
                    var emailMessage = new MimeMessage();
                    MailboxAddress emailFrom = new MailboxAddress("woodenAutomative", "trackgaddireports1@gmail.com");
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = MailboxAddress.Parse(emailData.EmailToId);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = emailData.EmailSubject;

                    emailMessage.Body = new TextPart(TextFormat.Html) { Text = emailData.EmailBody };

                    emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    emailClient.Authenticate("trackgaddireports1@gmail.com", "txqrdkvxwrduspwy");
                    emailClient.Send(emailMessage);

                    return true;
                }
                catch (Exception ex)
                {
                    //Log Exception Details
                    return false;
                }
                finally
                {
                    emailClient.Disconnect(true);
                    emailClient.Dispose();
                }
            }
        }

        public async Task<bool> SendEmailOTP(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;
            var emailData=new EmailData();
            emailData.EmailToId = user.Email;
            emailData.EmailToName = user.FirstName;
            emailData.EmailSubject = "test";

            string otpValue = new Random().Next(100000, 999999).ToString();
            emailData.EmailBody = "Your OTP Number is " + otpValue + " ( Sent By : WoodenAutomative )";
            InsertOTP(user.Email, "Email", otpValue);
            var status=SendEmail(emailData);
            return status;
        }

        public async Task<bool> InsertOTP(string email,string AuthorizationType,string OtpValue)
        {
            var status = false;
            using (WoodenAutomativeContext db = new WoodenAutomativeContext(new DbContextOptionsBuilder<WoodenAutomativeContext>()
                                .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                                .Options))
            {
                otp createOtp = new otp()
                {
                    OTPNumber = OtpValue,
                    ValidTill = DateTime.Now.AddMinutes(2),
                    SendingTime = DateTime.Now,
                    AuthorizeFor = email,
                    AuthorizationType = AuthorizationType,
                    IsVerify = false
                };
                await db.OTP.AddAsync(createOtp);
                var result = await db.SaveChangesAsync();
                status = result > 0 ? true : false;
            }
            return status;
        }

        public async Task<bool> VerifyOTP(string email, string otp)
        {
                var status = false;
                using (WoodenAutomativeContext db = new WoodenAutomativeContext(new DbContextOptionsBuilder<WoodenAutomativeContext>()
                                    .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                                    .Options))
                {

                    var vaildOTP = await db.OTP.Where(x => x.AuthorizeFor == email && x.OTPNumber == otp && x.ValidTill > DateTime.Now && x.IsVerify == false).FirstOrDefaultAsync();
                    if (vaildOTP != null)
                    {
                        var user=await _userManager.FindByEmailAsync(email);
                        user.EmailConfirmed = true;
                        vaildOTP.IsVerify= true;
                        var result = await db.SaveChangesAsync();
                        status = result > 0 ? true : false;
                    }
                }
                return status;
        }

        public Task<bool> SendMobileOTP(string userId)
        {
            // Your Twilio account SID and auth token from twilio.com/console
            string accountSid = "YOUR_ACCOUNT_SID";
            string authToken = "YOUR_AUTH_TOKEN";
            string otpValue = new Random().Next(100000, 999999).ToString();
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Your OTP is {otpValue}",
                from: new PhoneNumber("YOUR_TWILIO_PHONE_NUMBER"),
                to: new PhoneNumber("")
            );
            throw new NotImplementedException();
        }
    }
}
