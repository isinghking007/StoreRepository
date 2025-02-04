using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class FormData
    {
        [Key]
        public int FormId { get; set; }

        public string Name {  get; set; }
        public string Email { get; set; }
        public string FileLocation { get; set; }
        public string Description { get; set; }
    }
}
