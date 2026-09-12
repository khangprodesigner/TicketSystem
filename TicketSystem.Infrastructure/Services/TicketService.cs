using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Interfaces;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;
using TicketSystem.Infrastructure.Data;
using TicketSystem.Application.DTOs;
namespace TicketSystem.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Ticket>> GetMyTicketsAsync(string userId)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.RequesterId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetAllPendingTicketsAsync()
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.Status == TicketStatus.Pending)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(Guid id)
        {
            return await _context.Tickets
                .Include(t => t.Histories)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Status = TicketStatus.Pending;

            _context.Tickets.Add(ticket);
            return await _context.SaveChangesAsync() > 0;
        }

        // Lấy quỹ phép của người dùng
        public async Task<double> GetUserLeaveBalanceAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.LeaveBalance ?? 0;
        }

        public async Task<bool> ProcessTicketAsync (Guid ticketId, string approverId, string approverName, TicketStatus newStatus, string comment)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            // Nếu là đơn Nghỉ phép, kiểm tra và trừ quỹ phép của người dùng
            if (newStatus == TicketStatus.Approved && ticket.Type == TicketType.LeaveRequest)
            {
                var requester = await _context.Users.FindAsync(ticket.RequesterId);
                if (requester != null)
                {
                    double daysRequested = ticket.TotalDaysOrHours ??
                        (ticket.StartDate.HasValue && ticket.EndDate.HasValue
                        ? (ticket.EndDate.Value.Date - ticket.StartDate.Value.Date).TotalDays + 1
                        : 1);

                    if (requester.LeaveBalance < daysRequested)
                    {
                        // Không đủ quỹ phép để trừ
                        return false;
                    }

                    requester.LeaveBalance -= daysRequested;
                }
            }

            // Cập nhật trạng thái và thông tin người duyệt
            var history = new TicketHistory
            {
                TicketId = ticketId,
                ActorId = approverId,
                ActorName = approverName,
                PreviousStatus = ticket.Status,
                NewStatus = newStatus,
                Comment = comment,
                ActionDate = DateTime.UtcNow
            };

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.TicketHistories.Add(history);

            // Cả việc trừ quỹ phép, đổi status Ticket và lưu History đều được thực hiện trong cùng một transaction
            // nên chỉ cần gọi SaveChangesAsync một lần.
            return await _context.SaveChangesAsync() > 0;
        }

        // For Dashboard: Get counts of tickets by status and leave balance for the user
        public async Task<DashboardDto> GetUserDashboardDataAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var tickets = await _context.Tickets
                .AsNoTracking()
                .Where(t => t.RequesterId == userId)
                .ToListAsync();

            var leaveCount = tickets.Count(t => t.Type == TicketType.LeaveRequest);
            var expenseCount = tickets.Count(t => t.Type == TicketType.ExpenseClaim);
            var assetCount = tickets.Count(t => t.Type == TicketType.AssetRequest);
            var adjustCount = tickets.Count(t => t.Type == TicketType.AttendanceAdjustment);

            return new DashboardDto
            {
                PendingCount = tickets.Count(t => t.Status == TicketStatus.Pending),
                ApprovedCount = tickets.Count(t => t.Status == TicketStatus.Approved),
                RejectedCount = tickets.Count(t => t.Status == TicketStatus.Rejected),
                LeaveBalance = user?.LeaveBalance ?? 0,
                TicketTypeDistribution = new double[] { leaveCount, expenseCount, assetCount, adjustCount },
                TicketTypeLabels = new string[] { "Nghỉ phép", "Hoàn ứng", "Yêu cầu tài sản", "Giải trình công" }
            };
        }

        // For Manger Dashboard: Get counts of all pending tickets and distribution by type
        public async Task<DashboardDto> GetManagerDashboardDataAsync()
        {
            var tickets = await _context.Tickets
                .AsNoTracking()
                .ToListAsync();

            var leaveCount = tickets.Count(t => t.Type == TicketType.LeaveRequest);
            var expenseCount = tickets.Count(t => t.Type == TicketType.ExpenseClaim);
            var assetCount = tickets.Count(t => t.Type == TicketType.AssetRequest);
            var adjustCount = tickets.Count(t => t.Type == TicketType.AttendanceAdjustment);

            return new DashboardDto
            {
                PendingCount = tickets.Count(t => t.Status == TicketStatus.Pending),
                ApprovedCount = tickets.Count(t => t.Status == TicketStatus.Approved),
                RejectedCount = tickets.Count(t => t.Status == TicketStatus.Rejected),
                TicketTypeDistribution = new double[] { leaveCount, expenseCount, assetCount, adjustCount },
                TicketTypeLabels = new string[] { "Nghỉ phép", "Hoàn ứng", "Yêu cầu tài sản", "Giải trình công" }
            };
        }
        // For Profile:
        public async Task<bool> UpdateUserProfileAsync(string userId, string fullName, string department)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.FullName = fullName;
            user.Department = department;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
