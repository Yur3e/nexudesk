using NexoDesk.Application.Authentication.Interfaces;
using NexoDesk.Domain.Entities;
using NexoDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace NexoDesk.Infrastructure.Persistence;

public sealed class DevelopmentDbInitializer(
    HelpDeskDbContext context,
    IPasswordHasher passwordHasher)
{
    private const string AgentEmail = "admin@helpdesk.local";
    private const string AgentPassword = "Admin123!";
    private const string AgentName = "Administrador NexoDesk";

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await context.Database.MigrateAsync(cancellationToken);

        var existingAgent = await context.Users.SingleOrDefaultAsync(
            user => user.Email == AgentEmail,
            cancellationToken);

        if (existingAgent is not null)
        {
            if (!string.Equals(existingAgent.Name, AgentName, StringComparison.Ordinal))
            {
                existingAgent.UpdateName(AgentName);
                await context.SaveChangesAsync(cancellationToken);
            }

            return;
        }

        var agent = new User(
            AgentName,
            AgentEmail,
            passwordHasher.Hash(AgentPassword),
            UserRole.Agente,
            DateTime.UtcNow);

        await context.Users.AddAsync(agent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
