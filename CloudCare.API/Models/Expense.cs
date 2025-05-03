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
    public DateTime Date { get; set; } = DateTime.Now;

    // Foreign keys
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int VendorId { get; set; }
    public Vendor Vendor { get; set; }

    public bool IsRecurring { get; set; }
}