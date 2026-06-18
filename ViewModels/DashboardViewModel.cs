using MiniStoreManager.Models;

namespace MiniStoreManager.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }

    public int TotalCategories { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalRevenue { get; set; }

    public List<Product> LowStockProducts { get; set; } = new();
}
