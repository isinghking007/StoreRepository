using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class CustomerDetails
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }


    }
}
