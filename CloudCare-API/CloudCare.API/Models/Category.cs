namespace CloudCare.API.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? UserId { get; set; } // nullable for global defaults

    public ICollection<Expense> Expenses { get; set; }
}