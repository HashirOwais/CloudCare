namespace CloudCare.API.DTOs;

public class ReadExpenseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }

    // Display names
    public string Category { get; set; }           // category name
    public string Vendor { get; set; }             // vendor name
    public string PaymentMethod { get; set; }      // payment method name

    // Foreign key IDs (for editing form)
    public int CategoryId { get; set; }
    public int VendorId { get; set; }
    public int PaymentMethodId { get; set; }

    public string? Notes { get; set; }
    public string? ReceiptUrl { get; set; }
}