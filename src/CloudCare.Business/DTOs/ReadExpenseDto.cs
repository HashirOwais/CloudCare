using CloudCare.Data.Models;

namespace CloudCare.Business.DTOs;

public class ReadExpenseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public bool IsRecurring { get; set; }
    public BillingCycle BillingCycle { get; set; }

    // Display names
    public string? Category { get; set; }
    public string? Vendor { get; set; }
    public string? PaymentMethod { get; set; }


    // Foreign key IDs (for editing form)
    public int CategoryId { get; set; }
    public int VendorId { get; set; }
    public int PaymentMethodId { get; set; }

    public string? Notes { get; set; }
    public string? ReceiptUrl { get; set; }
    public int? RecurrenceSourceId { get; set; }
}