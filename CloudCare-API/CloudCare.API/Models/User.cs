using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudCare.API.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Email { get; set; } = null!; // Auth0 email

    public string Name { get; set; } = null!; // Provider's name

    public string DaycareName { get; set; } = null!;

    public string DaycareAddress { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? Notes { get; set; }

    public DateTime UserCreated { get; set; }

    // Relationship: One user can have many expenses
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}