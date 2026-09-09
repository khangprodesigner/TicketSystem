using System;
using System.Collections.Generic;
using System.Text;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Interfaces
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetMyTicketsAsync(string userId);
        Task<List<Ticket>> GetAllPendingTicketsAsync(); // Dành cho Manager duyệt
        Task<Ticket?> GetTicketByIdAsync(Guid id);
        Task<bool> CreateTicketAsync(Ticket ticket);
        Task<bool> ProcessTicketAsync(Guid ticketId, string approverId, string approverName, TicketStatus status, string comment);
    }
}
