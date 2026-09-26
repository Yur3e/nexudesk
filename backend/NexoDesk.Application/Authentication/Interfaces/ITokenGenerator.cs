using NexoDesk.Domain.Entities;

namespace NexoDesk.Application.Authentication.Interfaces;

public interface ITokenGenerator
{
    string Generate(User user);
}
