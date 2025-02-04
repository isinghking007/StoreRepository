using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class BorrowerDetails
    {
        [Key]
        public int BorrowerId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string BorrowerName { get; set; }
        public string Address { get; set; }
        public string BorrowerPhone { get;set ; }

        public DateTime BorrowDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
