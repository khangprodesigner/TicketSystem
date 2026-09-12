using System;
using System.Collections.Generic;
using System.Text;
using  TicketSystem.Application.DTOs;
namespace TicketSystem.Application.DTOs
{
    public class DashboardDto
    {
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public double LeaveBalance { get; set; }
        // phân phối loại phiếu
        public double[] TicketTypeDistribution { get; set; } = Array.Empty<double>();
        public string[] TicketTypeLabels { get; set; } = Array.Empty<string>();
    }
}
