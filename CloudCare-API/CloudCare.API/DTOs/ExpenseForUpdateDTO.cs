namespace CloudCare.API.DTOs;
using System.ComponentModel.DataAnnotations;

public class ExpenseForUpdateDto
{
    [Required]
    public int Id { get; set; }  // Required because we're updating a specific record

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public bool IsRecurring { get; set; }

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Vendor { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string PaymentMethod { get; set; } = null!;

    [StringLength(500)]
    public string? Notes { get; set; }

    [Url]
    public string? ReceiptUrl { get; set; }
}