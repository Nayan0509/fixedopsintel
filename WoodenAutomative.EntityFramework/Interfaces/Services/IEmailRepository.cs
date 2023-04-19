using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Models;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IEmailRepository
    {
       public bool SendEmail(EmailData emailData);
    }
}
