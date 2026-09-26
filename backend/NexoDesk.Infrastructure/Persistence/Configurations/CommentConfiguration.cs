using NexoDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NexoDesk.Infrastructure.Persistence.Configurations;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(comment => comment.Id);
        builder.Property(comment => comment.Id).ValueGeneratedNever();
        builder.Property(comment => comment.Content).IsRequired();
        builder.Property(comment => comment.CreatedAt).HasColumnType("datetime2").IsRequired();

        builder.HasIndex(comment => comment.TicketId);
        builder.HasIndex(comment => comment.UserId);
    }
}
