using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
namespace TicketSystem.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public double LeaveBalance { get; set; } = 12.0; // Quỹ phép năm, mặc định 12 ngày phép/năm
    }
}
