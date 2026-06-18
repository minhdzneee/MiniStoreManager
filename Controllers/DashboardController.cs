using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreManager.Data;
using MiniStoreManager.ViewModels;

namespace MiniStoreManager.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalProducts = await _context.Products.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync(),
            TotalOrders = await _context.Orders.CountAsync(),
            TotalRevenue = await _context.Orders.SumAsync(order => (decimal?)order.TotalAmount) ?? 0,
            LowStockProducts = await _context.Products
                .Include(product => product.Category)
                .OrderBy(product => product.Quantity)
                .ThenBy(product => product.Name)
                .Take(5)
                .ToListAsync()
        };

        return View(viewModel);
    }
}
