using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Core;

namespace WoodenAutomative.Domain.Dtos.Response.Distributor
{
    public class DistributorListResponse : PagedDataResponse
    {
        public List<DistributorDetailsResponse> DistributorDetails { get; set; }
    }
}
