namespace CloudCare.API.Models;

public class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } // e.g., "Cash", "Credit", "E-Transfer"

    public ICollection<Expense> Expenses { get; set; }
}