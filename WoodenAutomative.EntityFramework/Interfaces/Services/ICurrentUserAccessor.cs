using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface ICurrentUserAccessor
    {
        string Role { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}
