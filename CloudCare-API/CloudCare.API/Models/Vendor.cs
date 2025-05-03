namespace CloudCare.API.Models;

public class Vendor
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? UserId { get; set; }

    public ICollection<Expense> Expenses { get; set; }
}