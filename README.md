# 🎫 Ticket System

> Hệ thống quản lý và phê duyệt yêu cầu nội bộ (HRM / Approval Portal) được xây dựng bằng **Blazor Server (.NET 10)** và **MudBlazor**.

## ✨ Giới thiệu

Ticket System giúp doanh nghiệp quản lý các yêu cầu nội bộ với quy trình phê duyệt linh hoạt, hỗ trợ nhiều cấp duyệt và lưu vết toàn bộ lịch sử xử lý.

---

## 📌 Tính năng

- 🏖️ Đơn xin nghỉ phép
- 💰 Đề nghị hoàn ứng / thanh toán
- ⏰ Giải trình chấm công
- ✅ Quy trình phê duyệt nhiều cấp (Multi-level Approval)
- 📜 Audit Log & lịch sử xử lý
- 🔔 Theo dõi trạng thái yêu cầu

---

## 🏗️ Kiến trúc

```
Presentation (Blazor Server)
        │
Application
        │
Domain
        │
Infrastructure
        │
SQL Server
```

---

## ⚙️ Chạy dự án Local

### 1. Clone repository

```bash
git clone https://github.com/khangprodesigner/TicketSystem.git
```

### 2. Cập nhật Connection String

Mở file `appsettings.json` và chỉnh sửa:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TicketSystem;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Cập nhật Database

```bash
dotnet ef database update
```

### 4. Chạy ứng dụng

```bash
dotnet run
```

Hoặc chạy trực tiếp bằng Visual Studio / Rider.

---

## 📂 Công nghệ sử dụng

| Layer | Technology |
|--------|------------|
| UI | Blazor Server + MudBlazor |
| Backend | ASP.NET Core (.NET 10) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Validation | FluentValidation |
| Architecture | Clean Architecture |

---

## 📄 License

This project is for learning and internal business management purposes.
