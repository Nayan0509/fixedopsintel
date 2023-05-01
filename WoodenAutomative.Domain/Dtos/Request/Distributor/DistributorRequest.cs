using System.ComponentModel.DataAnnotations;
using WoodenAutomative.Domain.Dtos.Response.Distributor;

namespace WoodenAutomative.Domain.Dtos.Request.Distributor
{
    public class DistributorRequest
    {
        [Display(Name = "Distributor Name")]
        [Required(ErrorMessage = "Distributor Name is Required")]
        public string DistributorName { get; set; }

        [Display(Name = "Distributor Address")]
        [Required(ErrorMessage = "Distributor Address is Required")]
        public string DistributorAddress { get; set; }
        
        [Display(Name = "Distributor City")]
        [Required(ErrorMessage = "Distributor City is Required")]
        public string DistributorCity { get; set; }
        
        [Display(Name = "Distributor State")]
        [Required(ErrorMessage = "Distributor State is Required")]
        public string DistributorState { get; set; }
        
        [Display(Name = "Distributor Country")]
        [Required(ErrorMessage = "Distributor Country is Required")]
        public string DistributorCountry { get; set; }
        
        [Display(Name = "Distributor ZipCode")]
        [Required(ErrorMessage = "Distributor ZipCode is Required")]
        public string DistributorZipCode { get; set; }
        
        [Display(Name = "Product Distributor")]
        [Required(ErrorMessage = "Product Distributor is Required")]
        public string ProductDistributor { get; set; }
    }
}
