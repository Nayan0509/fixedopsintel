﻿using WoodenAutomative.Domain.Dtos.Request.Email;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IEmailRepository
    {
       public bool SendEmail(EmailData emailData);
       public Task<bool> SendEmailOTP(string userId);
       public Task<bool> SendMobileOTP(string userId);
       public Task<bool> VerifyOTP(string email,string otp);
       public Task<bool> InsertOTP(string email, string AuthorizationType, string OtpValue);
    }
}
