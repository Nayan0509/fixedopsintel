using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Dtos.Response.Distributor
{
    public class DistributorResponse
    {
        public int Id { get; set; }
        public string DistributorName { get; set; }

        public string DistributorAddress { get; set; }

        public string DistributorCity { get; set; }

        public string DistributorState { get; set; }

        public string DistributorCountry { get; set; }

        public string DistributorZipCode { get; set; }

        public string ProductDistributor { get; set; }
    }
}
