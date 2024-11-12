using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public required string MobileNumber { get; set; } // Make this composite key
        [Required]
        public required int CompanyId { get; set; }

    }
}
