using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketHistory> TicketHistories => Set<TicketHistory>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Cấu hình bảng Ticket
            builder.Entity<Ticket>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Title).IsRequired().HasMaxLength(200);
                b.Property(t => t.Description).IsRequired().HasMaxLength(2000);
                b.Property(t => t.Amount).HasPrecision(18, 2); // Định dạng số tiền với 2 chữ số thập phân

                // 1 Ticket có nhiều History logs, khi xoá Ticket thì xoá kèm history
                b.HasMany(t => t.Histories)
                 .WithOne(h => h.Ticket)
                 .HasForeignKey(h => h.TicketId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình bảng TicketHistory
            builder.Entity<TicketHistory>(b =>
            {
                b.HasKey(h => h.Id);
                b.Property(h => h.Comment).HasMaxLength(1000);
            });
        }
    }
}
