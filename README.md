# FU News Management System - PRN222 Assignment 01

## Thông tin Sinh viên
- **Họ và tên:** Nguyễn Thế Dũng
- **Mã số sinh viên:** QE190275
- **Môn học:** PRN222 - Phát triển ứng dụng đa nền tảng với .NET
- **Solution:** `NguyenTheDung_QE190275_A01.sln`

---

## Giới thiệu Dự án
**FU News Management System** là hệ thống quản lý và xuất bản tin tức dành cho trường đại học, được xây dựng trên nền tảng **ASP.NET Core MVC (.NET 9.0)** và **Entity Framework Core** kết hợp cơ sở dữ liệu **Microsoft SQL Server**.

Hệ thống hỗ trợ quản lý tập trung quy trình biên tập bản tin, phân loại chuyên mục, gắn nhãn chủ đề (Tag), quản lý tài khoản phân quyền theo vai trò và thống kê báo cáo xuất bản theo khoảng thời gian.

---

## Kiến trúc Hệ thống và Mẫu Thiết kế (Design Patterns)
Dự án tuân thủ nghiêm ngặt kiến trúc **3 Tầng (3-Layer Architecture)** kết hợp **Repository Pattern** và **Singleton Pattern**:

1. **FUNewsManagement_BusinessObjects (Tầng Thực thể & Dữ liệu):**
   - Chứa các lớp Entity (`SystemAccount`, `Category`, `NewsArticle`, `Tag`) kèm các thuộc tính kiểm tra hợp lệ dữ liệu (Data Annotations Validation).
   - Chứa `FuNewsManagementContext` quản lý kết nối và ánh xạ cơ sở dữ liệu thông qua Entity Framework Core (đọc chuỗi kết nối từ `appsettings.json`).

2. **FUNewsManagement_DataAccess (Tầng Truy cập Dữ liệu):**
   - **Data Access Objects (DAOs):** Triển khai theo mẫu thiết kế **Singleton Pattern** (`SystemAccountDAO`, `CategoryDAO`, `NewsArticleDAO`, `TagDAO`), xử lý các truy vấn LINQ và đảm bảo tính toàn vẹn dữ liệu.
   - **Repositories:** Cung cấp các Interface và lớp thực thi (`ISystemAccountRepository`, `ICategoryRepository`, `INewsArticleRepository`, `ITagRepository`) làm cầu nối trung gian tách biệt tầng nghiệp vụ khỏi tầng truy cập dữ liệu.

3. **FUNewsManagement_Services (Tầng Nghiệp vụ - Business Logic Layer):**
   - Chứa các dịch vụ nghiệp vụ (`SystemAccountService`, `CategoryService`, `NewsArticleService`, `TagService`) phục vụ cho tầng giao diện thông qua cơ chế Dependency Injection (DI).

4. **NguyenTheDungMVC (Tầng Giao diện - Presentation Layer):**
   - Ứng dụng Web theo mô hình **Model-View-Controller (MVC)**.
   - Toàn bộ thao tác Thêm mới (Create) và Cập nhật (Update) được thực hiện thông qua hộp thoại **Popup Modal**.
   - Toàn bộ thao tác Xóa (Delete) đều đi kèm hộp thoại xác nhận (**Confirmation Modal**) và kiểm tra ràng buộc khóa ngoại trước khi thực thi.

---

## Phân quyền và Chức năng Chính

### 1. Khách (Guest - Không cần đăng nhập) và Giảng viên (Lecturer - Role = 2)
- Xem danh sách các bản tin đang hoạt động (`NewsStatus = true`) thuộc các chuyên mục đang kích hoạt.
- Xem nội dung chi tiết của từng bài báo, bao gồm thông tin tác giả, người cập nhật cuối, ngày xuất bản, chuyên mục và các nhãn chủ đề đính kèm.
- Tìm kiếm và lọc bài viết theo từ khóa (tiêu đề, tóm tắt, nội dung, nhãn) hoặc theo chuyên mục.

### 2. Quản trị viên (Admin - Cấu hình trong `appsettings.json`)
- **Quản lý Tài khoản (`SystemAccount`):** Xem danh sách, tìm kiếm theo tên hoặc email, thêm mới, chỉnh sửa thông tin qua Popup Modal và xóa tài khoản (chặn xóa nếu tài khoản đã từng tạo hoặc cập nhật bài viết).
- **Báo cáo và Thống kê (`Report Statistics`):**
  - Lọc danh sách bài viết theo khoảng thời gian từ `StartDate` đến `EndDate` dựa trên ngày tạo (`CreatedDate`), sắp xếp giảm dần theo thời gian.
  - Thống kê tỷ trọng bài viết theo từng chuyên mục và theo từng tác giả.
  - Hỗ trợ xuất dữ liệu báo cáo ra file CSV (Excel).

### 3. Nhân viên (Staff - Role = 1)
- **Quản lý Danh mục (`Category`):** Xem, tìm kiếm, thêm mới, cập nhật qua Popup Modal và xóa danh mục. Hệ thống chỉ cho phép xóa danh mục khi danh mục đó chưa chứa bất kỳ bài viết nào và không phải là danh mục cha của danh mục khác.
- **Quản lý Bài viết và Nhãn (`NewsArticle` & `Tag`):**
  - Xem, tìm kiếm kết hợp bộ lọc đa tiêu chí (Từ khóa, Chuyên mục, Trạng thái).
  - Thêm mới, cập nhật bài viết qua Popup Modal và xóa bài viết có xác nhận.
  - Gắn nhiều nhãn (`Tag`) cho một bài viết thông qua bảng trung gian `NewsTag` (Quan hệ N-N).
  - Quản lý danh mục nhãn (`Tag`): Thêm, sửa, xóa và tìm kiếm nhãn (chặn xóa nhãn đang được gắn cho bài viết).
- **Lịch sử Bài đăng (`My History`):** Tra cứu và theo dõi danh sách các bài báo do chính tài khoản nhân viên đang đăng nhập khởi tạo.
- **Quản lý Hồ sơ Cá nhân (`My Profile`):** Xem và cập nhật thông tin cá nhân, mật khẩu qua Popup Modal.

---

## Hướng dẫn Cài đặt và Chạy Dự án

### 1. Yêu cầu Môi trường
- .NET SDK 8.0 / 9.0 trở lên
- Microsoft SQL Server 2012 trở lên
- Visual Studio 2022 hoặc Visual Studio Code

### 2. Khởi tạo Cơ sở Dữ liệu
1. Chạy script khởi tạo cơ sở dữ liệu gốc `FUNewsManagement.sql` trên SQL Server.
2. Chạy tiếp script `UpdateVietnameseData.sql` đi kèm trong thư mục gốc của dự án để cập nhật bộ dữ liệu tiếng Việt và danh sách tài khoản mẫu.

### 3. Cấu hình Kết nối
Kiểm tra chuỗi kết nối SQL Server trong file `NguyenTheDungMVC/appsettings.json`:

```json
"ConnectionStrings": {
  "FUNewsManagementDB": "Server=(local);Database=FUNewsManagement;Uid=sa;Pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

### 4. Khởi chạy Ứng dụng
Chạy các lệnh sau từ thư mục gốc của Solution:

```powershell
dotnet build NguyenTheDung_QE190275_A01.sln
dotnet run --project NguyenTheDungMVC/NguyenTheDungMVC.csproj
```

---

## Tài khoản Đăng nhập Mặc định

| Vai trò | Email | Mật khẩu |
| :--- | :--- | :--- |
| **Quản trị viên (Admin)** | `admin@gmail.com` | `@@abc123@@` |
| **Nhân viên (Staff - Role 1)** | `levanbinh@gmail.com`<br>`phamthihuong@gmail.com`<br>`hoangquocviet@gmail.com` | `@1` |
| **Giảng viên (Lecturer - Role 2)** | `nguyenvanan@gmail.com`<br>`tranminhanh@gmail.com` | `@1` |
