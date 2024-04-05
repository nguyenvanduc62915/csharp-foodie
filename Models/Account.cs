using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppCore.Models;

public class Account : IdentityUser {
    
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(200)]
    public string Email { get; set; }
    [MaxLength(500)]
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Address { get; set; }
}
