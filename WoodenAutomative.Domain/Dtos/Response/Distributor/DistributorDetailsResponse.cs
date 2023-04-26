using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Response.Distributor
{
    public class DistributorDetailsResponse
    {
        public string DistributorName { get; set; }
        public string ProductDistributor { get; set; }
        public string Username { get; set; }
        public string TerritoryName { get; set; }
        public int Id { get; set; }
    }
}
