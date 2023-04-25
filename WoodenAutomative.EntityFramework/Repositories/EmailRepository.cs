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
            string otpValue = new Random().Next(100000, 999999).ToString();
            string message = "Your OTP Number is " + otpValue + " ( Sent By : TrackGaddi )";

            SendSmsViaNewAPI(message, "7878548818");
            //try
            //{
            //    var user = await _userManager.FindByIdAsync(userId);
            //    // Your Twilio account SID and auth token from twilio.com/console
            //    string accountSid = "AC6c46d01370a2958f1340596417d7f3dc";
            //    string authToken = "7641a191ac254b8508cf572fb1e9d4c8";
            //    string otpValue = new Random().Next(100000, 999999).ToString();
            //    TwilioClient.Init(accountSid, authToken);

            //    var service = ServiceResource.Create(friendlyName: "My Verify Service");

            //    var verification = VerificationResource.Create(
            //                        to: "+16076383751",
            //                        channel: "sms",
            //                        pathServiceSid: service.AccountSid
            //                    );

            //    var verificationCheck = VerificationCheckResource.Create(
            //        to: user.PhoneNumber,
            //        code: "123456",
            //        pathServiceSid: service.Sid
            //    );
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            throw new NotImplementedException();
        }

       
        public string SendSmsViaNewAPI(string message, string mobileNumber)
        {
            string returnString = string.Empty;

            try
            {
                string baseSendSMSUri = "http://sms.onlinebusinessbazaar.in/api/mt/SendSMS?";
                string senderId = "VTRACK";
                string userName = "wellwin";
                string password = "sms123";
                string channel = "trans";
                string smsDCS = "0";
                string flashsms = "0";
                string route = "1";

                string messageText = HttpUtility.UrlEncode(message);

                // Prepare send SMS API Uri
                StringBuilder sendSMSUri = new StringBuilder();
                sendSMSUri.Append(baseSendSMSUri);
                sendSMSUri.AppendFormat("user={0}", userName);
                sendSMSUri.AppendFormat("&password={0}", password);
                sendSMSUri.AppendFormat("&senderid={0}", senderId);
                sendSMSUri.AppendFormat("&channel={0}", channel);
                sendSMSUri.AppendFormat("&DCS={0}", smsDCS);
                sendSMSUri.AppendFormat("&flashsms={0}", flashsms);
                sendSMSUri.AppendFormat("&number={0}", mobileNumber);
                sendSMSUri.AppendFormat("&text={0}", message);
                sendSMSUri.AppendFormat("&route={0}", route);

                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri.ToString());
                UTF8Encoding encoding = new UTF8Encoding();

                httpWReq.Method = "GET";
                httpWReq.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                reader.Close();
                response.Close();

                returnString = responseString;

                dynamic stuff = Newtonsoft.Json.JsonConvert.DeserializeObject(returnString);
                int errorCode = Convert.ToInt32(stuff.ErrorCode.ToString());

                if (errorCode == 0)
                    return "Success.";
                else
                    return "Failure - " + returnString;

            }
            catch (SystemException ex)
            {
                returnString = ex.ToString();
            }
            catch (Exception ex)
            {
                returnString = ex.ToString();
            }
            return returnString;
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
