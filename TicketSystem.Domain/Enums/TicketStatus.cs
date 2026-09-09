using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.Domain.Enums
{
    public enum TicketStatus
    {
        Draft = 0, // Bản nháp (chưa gửi)
        Pending = 1, // Đang chờ xử lý (đã gửi nhưng chưa được duyệt)
        Approved = 2, // Đã được duyệt (được chấp nhận)
        Rejected = 3, // Bị từ chối (không được chấp nhận)
        Cancelled = 4 // Đã hủy (người tạo tự hủy yêu cầu)
    }
}
