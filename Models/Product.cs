using System.ComponentModel.DataAnnotations;

namespace MiniStoreManager.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
    [StringLength(150, ErrorMessage = "Tên sản phẩm tối đa 150 ký tự")]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 999999999, ErrorMessage = "Giá bán phải lớn hơn 0")]
    [Display(Name = "Giá bán")]
    public decimal Price { get; set; }

    [Range(0, 1000000, ErrorMessage = "Số lượng không được âm")]
    [Display(Name = "Số lượng tồn")]
    public int Quantity { get; set; }

    [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [StringLength(300, ErrorMessage = "Đường dẫn hình ảnh tối đa 300 ký tự")]
    [Display(Name = "Hình ảnh")]
    public string? ImageUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục")]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
