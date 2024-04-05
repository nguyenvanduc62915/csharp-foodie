using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppCore.Models;
public class User : Account
{

    [MaxLength(500)]
    public string? Address { get; set; }

    public List<Order> Orders { get; set; }
}