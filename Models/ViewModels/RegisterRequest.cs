
namespace AppCore.Models.ViewModels;
public class RegisterRequest {
    public string Email { get; set; }

    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; } 
    
    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }
}