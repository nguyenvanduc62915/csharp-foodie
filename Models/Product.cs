using System.ComponentModel.DataAnnotations;
namespace AppCore.Models;

public class Product {
    [Key]
    public int ProductId { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string Image { get; set; }
    
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool Active { get; set; }
    
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }
    
    // public virtual ICollection<Orders> Orders { get; set; }
}

