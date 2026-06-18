using System.ComponentModel.DataAnnotations;

namespace MiniStoreManager.ViewModels;

public class OrderCreateViewModel
{
    [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên khách hàng tối đa 100 ký tự")]
    [Display(Name = "Tên khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [RegularExpression(@"^[0-9+\-\s]{9,15}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string CustomerPhone { get; set; } = string.Empty;

    public List<OrderProductInput> Items { get; set; } = new();
}

public class OrderProductInput
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string? CategoryName { get; set; }

    public string? ImageUrl { get; set; }

    public decimal UnitPrice { get; set; }

    public int AvailableQuantity { get; set; }

    [Range(0, 1000000, ErrorMessage = "Số lượng không được âm")]
    public int Quantity { get; set; }
}
