using System.ComponentModel.DataAnnotations;

public class CustomerDetailsDTO
{
    [Required]
    public string CustomerName { get; set; }

    [Required]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string Mobile { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public DateTime PurchaseDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
    public int TotalAmount { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Amount paid must be a positive value.")]
    public int AmountPaid { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Remaining amount must be a positive value.")]
    public int RemainingAmount { get; set; }

    [Required]
    public IFormFile File { get; set; }
}
