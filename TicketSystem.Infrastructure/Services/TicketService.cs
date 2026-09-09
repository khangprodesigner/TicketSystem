using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Interfaces;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;
using TicketSystem.Infrastructure.Data;

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

        public async Task<bool> ProcessTicketAsync (Guid ticketId, string approverId, string approverName, TicketStatus newStatus, string comment)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

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
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
