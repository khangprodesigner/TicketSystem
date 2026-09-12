using System;
using System.Collections.Generic;
using System.Text;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Interfaces
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetMyTicketsAsync(string userId);
        Task<List<Ticket>> GetAllPendingTicketsAsync(); // Dành cho Manager duyệt
        Task<Ticket?> GetTicketByIdAsync(Guid id);
        Task<bool> CreateTicketAsync(Ticket ticket);
        Task<bool> ProcessTicketAsync(Guid ticketId, string approverId, string approverName, TicketStatus status, string comment);
        // Lấy quỹ phép
        Task<double> GetUserLeaveBalanceAsync(string userId);

        // Lấy dữ liệu cho Dashboard
        Task<DashboardDto> GetUserDashboardDataAsync(string userId);
        Task<DashboardDto> GetManagerDashboardDataAsync();
        Task<bool> UpdateUserProfileAsync(string userId, string fullName, string department);
    }
}
