using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.Domain.Enums
{
    public enum TicketType
    {
        LeaveRequest = 1, // xin nghỉ phép
        ExpenseClaim = 2, // thanh toán / hoàn ứng
        AssetRequest = 3, // xin cấp thiết bị
        AttendanceAdjustment = 4 // giải trình chấm công 
    }
}
