using System;
using System.Collections.Generic;
using System.Text;
using TicketSystem.Domain.Enums;
namespace TicketSystem.Application.DTOs
{
    public class CreateTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketType Type { get; set; } = TicketType.LeaveRequest;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        // Nghỉ phép / Giải trình chấm công
        public DateTime? StartDate { get; set; } = DateTime.Today;
        public DateTime? EndDate { get; set; } = DateTime.Today;
        public double? TotalDaysorHours { get; set; }

        // Thanh toán / Hoàn ứng
        public decimal? Amount { get; set; }
    }
}
