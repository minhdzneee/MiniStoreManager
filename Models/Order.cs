using System.ComponentModel.DataAnnotations;

namespace MiniStoreManager.Models;

public class Order
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên khách hàng tối đa 100 ký tự")]
    [Display(Name = "Tên khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [RegularExpression(@"^[0-9+\-\s]{9,15}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Display(Name = "Ngày đặt")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Display(Name = "Tổng tiền")]
    public decimal TotalAmount { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
