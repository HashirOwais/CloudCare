using CloudCare.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CloudCare.Shared.DTOs.User;

public class UserForCreationDto
{
    [Required]
    public string Auth0Id { get; set; } = null!; // From JWT "sub" claim

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string DaycareName { get; set; } = null!;

    [Required]
    public string DaycareAddress { get; set; } = null!;

    [Phone]
    public string? PhoneNumber { get; set; }

    [Url]
    public string? WebsiteUrl { get; set; }

    public string? Notes { get; set; }
}