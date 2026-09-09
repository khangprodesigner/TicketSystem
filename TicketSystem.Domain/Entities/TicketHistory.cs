using System;
using System.Collections.Generic;
using System.Text;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Domain.Entities
{
    public class TicketHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; } // Navigation property to the associated Ticket

        public string ActorId { get; set; } = string.Empty; // ID người thực hiện (Manager / HR / Requester)
        public string ActorName { get; set; } = string.Empty; // Tên người thực hiện
        public TicketStatus PreviousStatus { get; set; } // 
        public TicketStatus NewStatus { get; set; } // 
        public string Comment { get; set; } = string.Empty; // Ghi chú lí do duyệt hoặc từ chối
        public DateTime ActionDate { get; set; } = DateTime.UtcNow; // Thời điểm thực hiện hành động

    }
}
