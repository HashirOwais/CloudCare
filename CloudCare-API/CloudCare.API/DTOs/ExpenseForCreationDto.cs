namespace CloudCare.API.DTOs;

public class ExpenseForCreationDto
{
    
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }

    // IDs for lookup
    public int CategoryId { get; set; }
    public int VendorId { get; set; }
    public int PaymentMethodId { get; set; }

    public string? Notes { get; set; }
    public string? ReceiptUrl { get; set; }
}