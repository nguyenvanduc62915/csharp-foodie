using System.ComponentModel.DataAnnotations;

namespace AppCore.Models;
public class Category {

    [Key]
    public int CategoryId { get; set; }
    
    [MaxLength(200)]
    public string Name { get; set; }
    
    [MaxLength(500)]
    public string Image { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }
    // public virtual ICollection<Product> Products { get; set; }
}