using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Text;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WoodenAutomative.Domain.Dtos.Request.Email;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

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
                    MailboxAddress emailFrom = new MailboxAddress("Fixed Ops Intel", "admin@fixedopsintel.com");
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = MailboxAddress.Parse(emailData.EmailToId);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = emailData.EmailSubject;

                    emailMessage.Body = new TextPart(TextFormat.Html) { Text = emailData.EmailBody };

                    emailClient.Connect("register-imap-oxcs.hostingplatform.com", 587, SecureSocketOptions.StartTls);
                    emailClient.Authenticate("admin@fixedopsintel.com", "@GGiany202020");
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
            emailData.EmailBody = "Your OTP Number is " + otpValue ;
            await InsertOTP(user.Email, "Email", otpValue);
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

        public async Task<bool> VerifyOTP(string userId, string otp)
        {
            
            var status = false;
                using (WoodenAutomativeContext co = new WoodenAutomativeContext(new DbContextOptionsBuilder<WoodenAutomativeContext>()
                                    .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                                    .Options))
                {
                    var user = await co.Users.FindAsync(userId);
                if (user == null) return false;
                    var vaildOTP = await co.OTP.Where(x => x.AuthorizeFor == user.Email && x.OTPNumber == otp && x.ValidTill > DateTime.Now && x.IsVerify == false).FirstOrDefaultAsync();
                    if (vaildOTP != null)
                    {
                        user.EmailConfirmed = true;
                        vaildOTP.IsVerify= true;
                        var result = await co.SaveChangesAsync();
                        status = result > 0 ? true : false;
                    }
                }
                return status;
        }

        public async Task<bool> SendMobileOTP(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                string accountSid = "AC6c46d01370a2958f1340596417d7f3dc";
                string authToken = "7641a191ac254b8508cf572fb1e9d4c8";
                string otpValue = new Random().Next(100000, 999999).ToString();
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "Hello from C#",
                    from: new Twilio.Types.PhoneNumber("18776294572"),
                    to: new Twilio.Types.PhoneNumber("917878548818")
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SendEmailOTPForForgotpassword(string email)
        {
            try
            {
                using (WoodenAutomativeContext db = new WoodenAutomativeContext(new DbContextOptionsBuilder<WoodenAutomativeContext>()
                    .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                    .Options))
                {
                    var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
                    if (user == null)
                        return false;
                    var emailData = new EmailData();
                    emailData.EmailToId = user.Email;
                    emailData.EmailToName = user.FirstName;
                    emailData.EmailSubject = "test";

                    string otpValue = new Random().Next(100000, 999999).ToString();
                    emailData.EmailBody = "Your OTP Number is " + otpValue + " ( Sent By : WoodenAutomative )";
                    await InsertOTP(user.Email, "Email", otpValue);
                    var status = SendEmail(emailData);
                    return status;
                }
            }
            catch(Exception ex) {
                throw ex;
            }
        }

        public async Task<bool> VerifyOTPForforgotpassword(string email, string otp)
        {
            var status = false;
            using (WoodenAutomativeContext db = new WoodenAutomativeContext(new DbContextOptionsBuilder<WoodenAutomativeContext>()
                                .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                                .Options))
            {
                var user = await db.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
                if(user == null) return false;
                var vaildOTP = await db.OTP.Where(x => x.AuthorizeFor == user.Email && x.OTPNumber == otp && x.ValidTill > DateTime.Now && x.IsVerify == false).FirstOrDefaultAsync();
                if (vaildOTP != null)
                {
                    vaildOTP.IsVerify = true;
                    var result = await db.SaveChangesAsync();
                    status = result > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
