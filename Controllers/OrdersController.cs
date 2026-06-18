using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreManager.Data;
using MiniStoreManager.Models;
using MiniStoreManager.ViewModels;

namespace MiniStoreManager.Controllers;

public class OrdersController : Controller
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(order => order.OrderDetails)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(item => item.OrderDetails)
            .ThenInclude(detail => detail.Product)
            .ThenInclude(product => product!.Category)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (order is null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new OrderCreateViewModel();
        await PopulateOrderItems(viewModel);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderCreateViewModel viewModel)
    {
        var selectedItems = viewModel.Items
            .Where(item => item.Quantity > 0)
            .ToList();

        if (!selectedItems.Any())
        {
            ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một sản phẩm.");
        }

        var productIds = selectedItems.Select(item => item.ProductId).ToList();
        var products = await _context.Products
            .Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id);

        foreach (var item in selectedItems)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                ModelState.AddModelError(string.Empty, "Có sản phẩm không tồn tại trong hệ thống.");
                continue;
            }

            if (item.Quantity > product.Quantity)
            {
                ModelState.AddModelError(string.Empty, $"{product.Name} chỉ còn {product.Quantity} sản phẩm trong kho.");
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateOrderItems(viewModel);
            return View(viewModel);
        }

        var order = new Order
        {
            CustomerName = viewModel.CustomerName,
            CustomerPhone = viewModel.CustomerPhone,
            OrderDate = DateTime.Now
        };

        foreach (var item in selectedItems)
        {
            var product = products[item.ProductId];
            var subTotal = product.Price * item.Quantity;

            product.Quantity -= item.Quantity;
            order.TotalAmount += subTotal;
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                SubTotal = subTotal
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã tạo đơn hàng mới.";

        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(item => item.OrderDetails)
            .ThenInclude(detail => detail.Product)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (order is null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders
            .Include(item => item.OrderDetails)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (order is null)
        {
            return NotFound();
        }

        // Khi xóa đơn demo, hoàn lại số lượng đã bán để số tồn kho luôn dễ kiểm tra.
        foreach (var detail in order.OrderDetails)
        {
            var product = await _context.Products.FindAsync(detail.ProductId);
            if (product is not null)
            {
                product.Quantity += detail.Quantity;
            }
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã xóa đơn hàng và hoàn lại tồn kho.";

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateOrderItems(OrderCreateViewModel viewModel)
    {
        var submittedQuantities = viewModel.Items.ToDictionary(item => item.ProductId, item => item.Quantity);
        var products = await _context.Products
            .Include(product => product.Category)
            .OrderBy(product => product.Name)
            .ToListAsync();

        viewModel.Items = products.Select(product => new OrderProductInput
        {
            ProductId = product.Id,
            ProductName = product.Name,
            CategoryName = product.Category?.Name,
            ImageUrl = product.ImageUrl,
            UnitPrice = product.Price,
            AvailableQuantity = product.Quantity,
            Quantity = submittedQuantities.TryGetValue(product.Id, out var quantity) ? quantity : 0
        }).ToList();
    }
}
