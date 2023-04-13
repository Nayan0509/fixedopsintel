using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Response.Login
{
    public class LoginResponse
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmailID { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyCode { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? EntryStatusID { get; set; }
        public string LoginUser { get; set; }
        public string LoginPassword { get; set; }
        public int? Flag { get; set; }
        public string LicenseKey { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyLogoMini { get; set; }
        public string UserName { get; set; }
        public int? SecurityID { get; set; }
        public string SecurityAnswer { get; set; }
    }
}
