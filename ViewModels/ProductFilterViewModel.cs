using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStoreManager.Models;

namespace MiniStoreManager.ViewModels;

public class ProductFilterViewModel
{
    public string? SearchTerm { get; set; }

    public int? CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();

    public List<Product> Products { get; set; } = new();
}
