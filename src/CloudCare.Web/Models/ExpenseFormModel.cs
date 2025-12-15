namespace CloudCare.Web.Models;

public class ExpenseFormModel
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int? CategoryId { get; set; }
    public int? VendorId { get; set; }    public int? PaymentMethodId { get; set; }
}
