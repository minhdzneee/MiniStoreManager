using System.ComponentModel.DataAnnotations;

namespace MiniStoreManager.Models;

public class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order? Order { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }

    [Range(1, 1000000, ErrorMessage = "Số lượng mua phải lớn hơn 0")]
    [Display(Name = "Số lượng mua")]
    public int Quantity { get; set; }

    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Thành tiền")]
    public decimal SubTotal { get; set; }
}
