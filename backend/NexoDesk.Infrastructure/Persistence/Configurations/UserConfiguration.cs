using NexoDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NexoDesk.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).ValueGeneratedNever();
        builder.Property(user => user.Name).HasMaxLength(100).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(256).IsRequired();
        builder.Property(user => user.PasswordHash).IsRequired();
        builder.Property(user => user.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(user => user.CreatedAt).HasColumnType("datetime2").IsRequired();

        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasMany(user => user.CreatedTickets)
            .WithOne(ticket => ticket.CreatedByUser)
            .HasForeignKey(ticket => ticket.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(user => user.AssignedTickets)
            .WithOne(ticket => ticket.AssignedAgent)
            .HasForeignKey(ticket => ticket.AssignedAgentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(user => user.Comments)
            .WithOne(comment => comment.User)
            .HasForeignKey(comment => comment.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
