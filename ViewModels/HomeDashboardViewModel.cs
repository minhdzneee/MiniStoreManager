using MiniStoreManager.Models;

namespace MiniStoreManager.ViewModels;

public class HomeDashboardViewModel
{
    public int TotalProducts { get; set; }

    public int TotalCategories { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalRevenue { get; set; }

    public List<Product> FeaturedProducts { get; set; } = new();

    public List<Product> LowStockProducts { get; set; } = new();

    public List<Order> RecentOrders { get; set; } = new();
}
