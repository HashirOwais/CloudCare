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

    [Required]
    public string Auth0Id { get; set; } = null!; // From JWT "sub" claim

    [Required]
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string DaycareName { get; set; } = null!;
    public string DaycareAddress { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Notes { get; set; }

    public string? Role { get; set; } = "provider"; // Can be "admin", "provider", etc.

    public bool IsRegistered { get; set; } = false;

    public DateTime UserCreated { get; set; } = DateTime.UtcNow;

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}