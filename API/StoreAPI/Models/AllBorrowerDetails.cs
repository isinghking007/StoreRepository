using Amazon.S3.Model;

public class AllBorrowerDetails
{
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public int CustomerId { get; set; }
    public int DueId { get; set; }
    public string Mobile { get; set; }
    public string TotalBillAmount { get; set; }
    public string PaidAmount { get; set; }
    public string NewAmount { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string FileLocation { get; set; }
    public string FileName { get; set; }
}
