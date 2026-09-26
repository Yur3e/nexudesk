using NexoDesk.Application.Authentication.Interfaces;
using NexoDesk.Domain.Entities;
using NexoDesk.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NexoDesk.Infrastructure.Repositories;

public sealed class UserRepository(HelpDeskDbContext context) : IUserRepository
{
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken) =>
        context.Users.AnyAsync(user => user.Email == email, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
