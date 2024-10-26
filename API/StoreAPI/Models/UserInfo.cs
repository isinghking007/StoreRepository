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
        public string MobileNumber { get; set; }

    }
}
