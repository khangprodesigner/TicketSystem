using System;
using System.Collections.Generic;
using System.Text;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketType Type { get; set; }
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public TicketStatus Status { get; set; } = TicketStatus.Pending;

        // Các trường đặc thù cho Đơn nghỉ phép
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalDaysOrHours { get; set;  }

        // Các trường đặc thù cho Đơn thanh toán / hoàn ứng
        public decimal? Amount { get; set; }

        // Định danh nhân sự
        public string RequesterId { get; set; } = string.Empty;
        public string RequesterName { get; set; } = string.Empty; // Lưu snapshot tên để hiển thị nhanh chóng mà không cần join bảng nhân sự

        // Audit Metadata 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties: Lịch sử phê duyệt (Audit log)
        public ICollection<TicketHistory> Histories { get; set; } = new List<TicketHistory>();
    }
}
