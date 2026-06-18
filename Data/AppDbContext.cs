using Microsoft.EntityFrameworkCore;
using MiniStoreManager.Models;

namespace MiniStoreManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasMany(category => category.Products)
            .WithOne(product => product.Category)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasMany(order => order.OrderDetails)
            .WithOne(detail => detail.Order)
            .HasForeignKey(detail => detail.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.OrderDetails)
            .WithOne(detail => detail.Product)
            .HasForeignKey(detail => detail.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .Property(product => product.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(order => order.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderDetail>()
            .Property(detail => detail.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderDetail>()
            .Property(detail => detail.SubTotal)
            .HasColumnType("decimal(18,2)");

        // Dữ liệu mẫu giúp sinh viên chạy demo ngay sau khi tạo database.
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Đồ uống", Description = "Nước suối, trà, cà phê và nước giải khát" },
            new Category { Id = 2, Name = "Thực phẩm", Description = "Các mặt hàng ăn nhanh và thực phẩm khô" },
            new Category { Id = 3, Name = "Gia dụng", Description = "Vật dụng thiết yếu cho cửa hàng mini" },
            new Category { Id = 4, Name = "Chăm sóc cá nhân", Description = "Sản phẩm vệ sinh và chăm sóc hằng ngày" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Nước suối 500ml",
                Price = 6000,
                Quantity = 120,
                Description = "Nước suối đóng chai tiện lợi cho khách mua lẻ.",
                ImageUrl = "/images/water.svg",
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Cà phê lon",
                Price = 15000,
                Quantity = 45,
                Description = "Cà phê lon dùng lạnh, bán chạy vào buổi sáng.",
                ImageUrl = "/images/coffee.svg",
                CategoryId = 1
            },
            new Product
            {
                Id = 3,
                Name = "Mì ly hải sản",
                Price = 12000,
                Quantity = 32,
                Description = "Mì ly ăn nhanh, phù hợp khu văn phòng và ký túc xá.",
                ImageUrl = "/images/noodle.svg",
                CategoryId = 2
            },
            new Product
            {
                Id = 4,
                Name = "Bánh quy bơ",
                Price = 28000,
                Quantity = 18,
                Description = "Bánh quy hộp nhỏ, dễ trưng bày tại quầy.",
                ImageUrl = "/images/cookie.svg",
                CategoryId = 2
            },
            new Product
            {
                Id = 5,
                Name = "Khăn giấy hộp",
                Price = 22000,
                Quantity = 9,
                Description = "Khăn giấy mềm dùng cho gia đình và văn phòng.",
                ImageUrl = "/images/tissue.svg",
                CategoryId = 3
            },
            new Product
            {
                Id = 6,
                Name = "Sữa tắm mini",
                Price = 35000,
                Quantity = 6,
                Description = "Sữa tắm dung tích nhỏ, tiện mang đi du lịch.",
                ImageUrl = "/images/bodywash.svg",
                CategoryId = 4
            }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CustomerName = "Nguyễn Minh An",
                CustomerPhone = "0901234567",
                OrderDate = new DateTime(2026, 6, 1, 9, 30, 0),
                TotalAmount = 42000
            },
            new Order
            {
                Id = 2,
                CustomerName = "Trần Hoài Nam",
                CustomerPhone = "0912345678",
                OrderDate = new DateTime(2026, 6, 2, 15, 10, 0),
                TotalAmount = 50000
            }
        );

        modelBuilder.Entity<OrderDetail>().HasData(
            new OrderDetail { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 6000, SubTotal = 12000 },
            new OrderDetail { Id = 2, OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 15000, SubTotal = 30000 },
            new OrderDetail { Id = 3, OrderId = 2, ProductId = 3, Quantity = 2, UnitPrice = 12000, SubTotal = 24000 },
            new OrderDetail { Id = 4, OrderId = 2, ProductId = 5, Quantity = 1, UnitPrice = 22000, SubTotal = 22000 },
            new OrderDetail { Id = 5, OrderId = 2, ProductId = 1, Quantity = 1, UnitPrice = 6000, SubTotal = 6000 }
        );
    }
}
