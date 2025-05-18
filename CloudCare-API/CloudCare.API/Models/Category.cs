namespace CloudCare.API.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    // 🚫 Removed UserId
    // 🚫 Removed navigation property to Expense
}