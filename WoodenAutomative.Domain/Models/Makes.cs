using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.Domain.Models
{
    public class Makes
    {
        [Key]
        public int Id { get; set; }

        public string FranchiseName { get; set; }
    }
}
