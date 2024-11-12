using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class Role
    {
        [Key] 
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RoleCode { get; set; }
    }
}
