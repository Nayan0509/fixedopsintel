using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Core;

namespace WoodenAutomative.Domain.Dtos.Request.Distributor
{
    public class DistributorListRequest : PagedDataRequest
    {
        public string Name { get; set; }
    }
}
