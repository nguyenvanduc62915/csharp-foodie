namespace AppCore.Models.ViewModels;

public class CategoryRequest {

    public int CategoryId;
    public string Name { get; set; } = "";
    public string Image { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; }

}