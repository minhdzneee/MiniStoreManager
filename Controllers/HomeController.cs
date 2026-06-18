using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MiniStoreManager.Data;
using MiniStoreManager.Models;
using MiniStoreManager.ViewModels;

namespace MiniStoreManager.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeDashboardViewModel
        {
            TotalProducts = await _context.Products.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync(),
            TotalOrders = await _context.Orders.CountAsync(),
            TotalRevenue = await _context.Orders.SumAsync(order => (decimal?)order.TotalAmount) ?? 0,
            FeaturedProducts = await _context.Products
                .Include(product => product.Category)
                .OrderByDescending(product => product.Quantity)
                .Take(4)
                .ToListAsync(),
            LowStockProducts = await _context.Products
                .Include(product => product.Category)
                .OrderBy(product => product.Quantity)
                .Take(5)
                .ToListAsync(),
            RecentOrders = await _context.Orders
                .OrderByDescending(order => order.OrderDate)
                .Take(4)
                .ToListAsync()
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
