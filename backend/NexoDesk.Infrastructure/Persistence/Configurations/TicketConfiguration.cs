using NexoDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NexoDesk.Infrastructure.Persistence.Configurations;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(ticket => ticket.Id);
        builder.Property(ticket => ticket.Id).ValueGeneratedNever();
        builder.Property(ticket => ticket.Title).HasMaxLength(150).IsRequired();
        builder.Property(ticket => ticket.Description).HasMaxLength(3000).IsRequired();
        builder.Property(ticket => ticket.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(ticket => ticket.Priority).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(ticket => ticket.CreatedAt).HasColumnType("datetime2").IsRequired();
        builder.Property(ticket => ticket.UpdatedAt).HasColumnType("datetime2").IsRequired();
        builder.Property(ticket => ticket.ClosedAt).HasColumnType("datetime2");

        builder.HasIndex(ticket => ticket.Status);
        builder.HasIndex(ticket => ticket.Priority);
        builder.HasIndex(ticket => ticket.CreatedByUserId);
        builder.HasIndex(ticket => ticket.AssignedAgentId);

        builder.HasMany(ticket => ticket.Comments)
            .WithOne(comment => comment.Ticket)
            .HasForeignKey(comment => comment.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
