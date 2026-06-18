# MiniStoreManager

MiniStoreManager là website quản lý bán hàng mini được xây dựng bằng **ASP.NET Core MVC**, **Entity Framework Core**, **SQLite** và **Bootstrap 5**. Project phù hợp cho sinh viên mới học C#/.NET muốn tham khảo một ứng dụng MVC đầy đủ CRUD, có database thật và giao diện dễ demo.

## Công nghệ sử dụng

- Ngôn ngữ: C#
- Framework: ASP.NET Core MVC
- ORM: Entity Framework Core
- Database: SQLite
- Giao diện: Bootstrap 5
- Công cụ chạy: .NET CLI, VS Code
- Hệ điều hành phù hợp: macOS, Windows, Linux

## Chức năng chính

- Trang chủ dạng dashboard giới thiệu hệ thống MiniStore và hiển thị số liệu tổng quan.
- Quản lý danh mục sản phẩm: xem, thêm, sửa, xóa.
- Quản lý sản phẩm: xem, thêm, sửa, xóa, chi tiết, tìm kiếm theo tên, lọc theo danh mục.
- Quản lý đơn hàng: tạo đơn, chọn sản phẩm, nhập số lượng, tự tính tổng tiền, xem chi tiết, xóa đơn.
- Quản lý chi tiết đơn hàng: mỗi đơn hàng có nhiều sản phẩm.
- Thống kê: tổng sản phẩm, tổng danh mục, tổng đơn hàng, tổng doanh thu và 5 sản phẩm tồn kho thấp nhất.
- Seed data: có sẵn dữ liệu mẫu sau khi tạo database.

## Yêu cầu môi trường

Cài đặt trước khi chạy project:

- [.NET SDK 10.0 hoặc mới hơn](https://dotnet.microsoft.com/download)
- VS Code hoặc IDE hỗ trợ .NET
- Git

Kiểm tra .NET đã cài chưa:

```bash
dotnet --version
```

## Cách chạy project sau khi clone từ GitHub

Clone repository:

```bash
git clone https://github.com/minhdzneee/MiniStoreManager.git
cd MiniStoreManager
```

Restore package NuGet:

```bash
dotnet restore
```

Restore local tool `dotnet-ef`:

```bash
dotnet tool restore
```

Tạo hoặc cập nhật database SQLite:

```bash
dotnet tool run dotnet-ef database update
```

Chạy website:

```bash
dotnet run
```

Mở trình duyệt:

```bash
open http://localhost:5088
```

Trên Windows, có thể mở trực tiếp đường dẫn này trong trình duyệt:

```text
http://localhost:5088
```

## Hướng dẫn sử dụng project

Phần này dành cho người muốn mở source code, chạy project, sửa code hoặc tạo lại database.

### Mở project bằng VS Code

```bash
code .
```

Sau khi mở VS Code, có thể xem các phần chính:

- `Models/`: định nghĩa dữ liệu và validation.
- `Controllers/`: xử lý các request từ người dùng.
- `Views/`: giao diện Razor MVC.
- `Data/AppDbContext.cs`: cấu hình database, quan hệ bảng và dữ liệu mẫu.
- `wwwroot/css/site.css`: CSS chính của giao diện.

### Chạy project khi đã có database

```bash
dotnet run
```

Sau đó mở:

```text
http://localhost:5088
```

### Tạo lại database từ đầu

Nếu muốn reset dữ liệu demo, xóa file `ministore.db`, sau đó chạy:

```bash
dotnet tool run dotnet-ef database update
```

Lệnh này tạo lại database SQLite và nạp dữ liệu mẫu từ `Data/AppDbContext.cs`.

### Kiểm tra project có build được không

```bash
dotnet build
```

Nếu build thành công, terminal sẽ hiển thị `Build succeeded`.

### Khi sửa Model và muốn tạo migration mới

```bash
dotnet tool run dotnet-ef migrations add TenMigrationMoi
dotnet tool run dotnet-ef database update
```

Ví dụ:

```bash
dotnet tool run dotnet-ef migrations add AddProductBarcode
dotnet tool run dotnet-ef database update
```

## Cấu hình database

Chuỗi kết nối SQLite nằm trong file `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=ministore.db"
}
```

Khi chạy lệnh sau, Entity Framework Core sẽ tạo file `ministore.db` ở thư mục gốc project:

```bash
dotnet tool run dotnet-ef database update
```

Nếu muốn tạo lại database từ đầu, hãy xóa file `ministore.db`, sau đó chạy lại:

```bash
dotnet tool run dotnet-ef database update
```

## Cấu trúc thư mục

```text
MiniStoreManager/
├── Controllers/        # Xử lý request MVC
├── Data/               # AppDbContext, cấu hình EF Core, seed data
├── Migrations/         # Migration tạo database SQLite
├── Models/             # Entity model: Category, Product, Order, OrderDetail
├── ViewModels/         # Model riêng cho form và dashboard
├── Views/              # Razor views cho giao diện MVC
├── wwwroot/            # CSS, JavaScript, Bootstrap, hình ảnh
├── appsettings.json    # Cấu hình ứng dụng và connection string
├── Program.cs          # Cấu hình service, middleware, routing
└── MiniStoreManager.csproj
```

## Các bảng trong database

Project sử dụng 4 bảng chính:

- `Categories`: danh mục sản phẩm.
- `Products`: sản phẩm.
- `Orders`: đơn hàng.
- `OrderDetails`: chi tiết đơn hàng.

Quan hệ dữ liệu:

- Một `Category` có nhiều `Product`.
- Một `Order` có nhiều `OrderDetail`.
- Một `Product` có nhiều `OrderDetail`.

## Hướng dẫn sử dụng website

Phần này dành cho người dùng muốn thao tác trực tiếp trên website sau khi chạy project.

Website không yêu cầu đăng nhập. Sau khi mở `http://localhost:5088`, người dùng có thể thao tác ngay các chức năng quản lý.

### 1. Trang chủ

Trang chủ hiển thị dashboard tổng quan:

- Tổng số sản phẩm.
- Tổng số danh mục.
- Tổng số đơn hàng.
- Tổng doanh thu.
- Danh sách sản phẩm nổi bật.
- Danh sách sản phẩm tồn kho thấp.
- Danh sách đơn hàng gần đây.

Các nút thao tác nhanh:

- `Tạo đơn hàng`: chuyển đến màn hình tạo đơn hàng mới.
- `Xem sản phẩm`: chuyển đến danh sách sản phẩm.
- `Mở thống kê`: chuyển đến trang thống kê chi tiết.

### 2. Quản lý danh mục

Vào menu `Danh mục`.

Tại đây có thể:

- Xem danh sách danh mục.
- Bấm `Thêm danh mục` để tạo danh mục mới.
- Bấm `Sửa` để cập nhật tên hoặc mô tả danh mục.
- Bấm `Xóa` để xóa danh mục.

Lưu ý:

- Không thể xóa danh mục nếu danh mục đó đang có sản phẩm.
- Muốn xóa danh mục đang có sản phẩm, cần chuyển hoặc xóa các sản phẩm thuộc danh mục đó trước.

### 3. Quản lý sản phẩm

Vào menu `Sản phẩm`.

Tại đây có thể:

- Xem danh sách sản phẩm.
- Tìm sản phẩm theo tên bằng ô `Tìm theo tên`.
- Lọc sản phẩm theo danh mục bằng ô `Danh mục`.
- Bấm `Thêm sản phẩm` để tạo sản phẩm mới.
- Bấm `Chi tiết` để xem đầy đủ thông tin sản phẩm.
- Bấm `Sửa` để cập nhật giá, số lượng tồn kho, mô tả, hình ảnh hoặc danh mục.
- Bấm `Xóa` để xóa sản phẩm.

Lưu ý:

- Sản phẩm cần có tên, giá bán, số lượng tồn và danh mục.
- Không thể xóa sản phẩm đã từng xuất hiện trong đơn hàng.
- Trường `ImageUrl` có thể nhập đường dẫn ảnh như `/images/product-placeholder.svg` hoặc URL ảnh online.

### 4. Tạo đơn hàng

Vào menu `Đơn hàng`, sau đó bấm `Tạo đơn hàng`.

Các bước tạo đơn:

1. Nhập `Tên khách hàng`.
2. Nhập `Số điện thoại`.
3. Ở danh sách sản phẩm, nhập số lượng muốn mua vào ô `Số lượng mua`.
4. Website tự tính `Thành tiền` từng dòng.
5. Website tự tính `Tổng tiền` của đơn hàng.
6. Bấm `Lưu đơn hàng`.

Sau khi lưu:

- Đơn hàng được lưu vào database.
- Số lượng tồn kho của sản phẩm sẽ tự động giảm.
- Website chuyển sang trang chi tiết đơn hàng.

Lưu ý:

- Phải chọn ít nhất một sản phẩm có số lượng mua lớn hơn 0.
- Số lượng mua không được lớn hơn số lượng tồn kho.

### 5. Xem và xóa đơn hàng

Vào menu `Đơn hàng`.

Tại đây có thể:

- Xem danh sách đơn hàng.
- Bấm `Chi tiết` để xem thông tin khách hàng, ngày đặt, tổng tiền và các sản phẩm trong đơn.
- Bấm `Xóa` để xóa đơn hàng.

Khi xóa đơn hàng:

- Đơn hàng và chi tiết đơn hàng sẽ bị xóa.
- Số lượng sản phẩm trong đơn sẽ được hoàn lại vào tồn kho.

### 6. Xem thống kê

Vào menu `Thống kê`.

Trang thống kê hiển thị:

- Tổng số sản phẩm.
- Tổng số danh mục.
- Tổng số đơn hàng.
- Tổng doanh thu.
- 5 sản phẩm có số lượng tồn kho thấp nhất.

Trang này dùng để kiểm tra nhanh tình hình bán hàng và sản phẩm cần nhập thêm.

## Luồng demo nhanh

Nếu muốn demo project trong vài phút, có thể làm theo thứ tự:

1. Mở `Trang chủ` để giới thiệu dashboard.
2. Vào `Danh mục`, thêm một danh mục mới.
3. Vào `Sản phẩm`, thêm một sản phẩm thuộc danh mục vừa tạo.
4. Dùng tìm kiếm và lọc sản phẩm để chứng minh chức năng tra cứu.
5. Vào `Đơn hàng > Tạo đơn hàng`, chọn sản phẩm và lưu đơn.
6. Mở `Chi tiết đơn hàng` để xem tổng tiền và danh sách sản phẩm.
7. Quay lại `Sản phẩm` để kiểm tra tồn kho đã giảm.
8. Vào `Thống kê` để xem doanh thu và tồn kho thấp.

## Lệnh Entity Framework Core thường dùng

Tạo migration mới sau khi sửa model:

```bash
dotnet tool run dotnet-ef migrations add TenMigration
```

Cập nhật database:

```bash
dotnet tool run dotnet-ef database update
```

Xem danh sách migration:

```bash
dotnet tool run dotnet-ef migrations list
```

## Lưu ý khi upload lên GitHub

Nên commit:

- Source code trong `Controllers/`, `Models/`, `Views/`, `Data/`, `ViewModels/`, `wwwroot/`.
- File `.csproj`.
- File `appsettings.json`.
- Thư mục `Migrations/`.
- File `dotnet-tools.json` để người khác restore được `dotnet-ef`.
- File `README.md`.

Không nên commit:

- `bin/`
- `obj/`
- `ministore.db`
- `*.db-shm`
- `*.db-wal`
- File cấu hình cá nhân của IDE nếu không cần thiết.

Gợi ý `.gitignore`:

```gitignore
bin/
obj/
*.db
*.db-shm
*.db-wal
.DS_Store
.vs/
.vscode/
```

## Gợi ý trình bày project

- Giới thiệu mục tiêu: quản lý bán hàng mini với CRUD và database SQLite.
- Trình bày mô hình MVC: Controller xử lý, Model lưu dữ liệu, View hiển thị.
- Giải thích quan hệ database trong `AppDbContext`.
- Demo CRUD danh mục và sản phẩm.
- Demo tạo đơn hàng, tự tính tổng tiền và trừ tồn kho.
- Demo dashboard thống kê doanh thu và sản phẩm tồn kho thấp.

## Tác giả

Project được xây dựng cho mục đích học tập và demo bài tập ASP.NET Core MVC.
