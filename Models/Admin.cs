using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppCore.Models;
public class Admin : Account {
    
    [MaxLength(500)]
    public String? Info { get; set; }
   
}