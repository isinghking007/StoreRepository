using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class FormDataDTO
    {
        [Key]
        public int FormId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public IFormFile FileDetails { get; set; }
        public string Description { get; set; }
    }
}
