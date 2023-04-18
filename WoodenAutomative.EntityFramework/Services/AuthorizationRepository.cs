using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{
    public class AuthorizationRepository: IAuthorizationRepository
    {
        public AuthorizationRepository()
        {
            
        }

        public Task<bool> SetPassword(UserProfileRequest userProfileRequest)
        {
            throw new NotImplementedException();
        }
    }
}
