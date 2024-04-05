using System.ComponentModel.DataAnnotations;

namespace AppCore.Models.ViewModels;

public class EditAccountViewModel
{
    public Account Account { get; set; }

    [Display(Name = "Roles")]
    public IEnumerable<string> Roles { get; set; }

    [Display(Name = "Selected Roles")]
    public List<string> SelectedRoles { get; set; }
}
