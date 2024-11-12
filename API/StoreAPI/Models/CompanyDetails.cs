using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class CompanyDetails
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        
    }
}
