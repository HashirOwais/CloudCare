using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudCare.API.Models;

public class Expense
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public bool IsRecurring { get; set; }

    public BillingCycle BillingCycle { get; set; } = BillingCycle.None;

    public string? ReceiptUrl { get; set; }

    public string? Notes { get; set; }

    // Foreign Keys
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
     public Category Category { get; set; }

    public int VendorId { get; set; }
    public Vendor Vendor { get; set; }

    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public int UserId { get; set; }
    
    //for recurring logic 
    public int? RecurrenceSourceId { get; set; } // null = template, else points to template Expense.Id

}