namespace AppCore.Models.ViewModels;


public class ProductRequest {
    public string? Name { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; }
    public int CategoryId { get; set; }

}