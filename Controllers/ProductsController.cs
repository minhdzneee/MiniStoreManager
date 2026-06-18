using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniStoreManager.Data;
using MiniStoreManager.Models;
using MiniStoreManager.ViewModels;

namespace MiniStoreManager.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? searchTerm, int? categoryId)
    {
        var query = _context.Products
            .Include(product => product.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(product => EF.Functions.Like(product.Name, $"%{searchTerm.Trim()}%"));
        }

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(product => product.CategoryId == categoryId.Value);
        }

        var viewModel = new ProductFilterViewModel
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            Categories = await GetCategorySelectList(categoryId),
            Products = await query
                .OrderBy(product => product.Name)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["CategoryId"] = await GetCategorySelectList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewData["CategoryId"] = await GetCategorySelectList(product.CategoryId);
            return View(product);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã thêm sản phẩm mới.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewData["CategoryId"] = await GetCategorySelectList(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["CategoryId"] = await GetCategorySelectList(product.CategoryId);
            return View(product);
        }

        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã cập nhật sản phẩm.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(product.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        var hasOrderDetails = await _context.OrderDetails.AnyAsync(detail => detail.ProductId == id);
        if (hasOrderDetails)
        {
            TempData["ErrorMessage"] = "Không thể xóa sản phẩm đã có trong đơn hàng.";
            return RedirectToAction(nameof(Index));
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã xóa sản phẩm.";

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetCategorySelectList(int? selectedId = null)
    {
        var categories = await _context.Categories
            .OrderBy(category => category.Name)
            .Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name,
                Selected = selectedId.HasValue && category.Id == selectedId.Value
            })
            .ToListAsync();

        return categories;
    }

    private Task<bool> ProductExists(int id)
    {
        return _context.Products.AnyAsync(product => product.Id == id);
    }
}
