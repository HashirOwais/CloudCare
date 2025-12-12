namespace CloudCare.Business.DTOs;
using System.ComponentModel.DataAnnotations;
using CloudCare.Data.Models;


public class ExpenseForCreationDto
{

    [Required]
    [StringLength(200)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    public bool IsRecurring { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int VendorId { get; set; }

    [Required]
    public int PaymentMethodId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Url]
    public string? ReceiptUrl { get; set; }

    public BillingCycle BillingCycle { get; set; } = BillingCycle.None;
}