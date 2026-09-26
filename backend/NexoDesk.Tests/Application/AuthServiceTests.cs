using NexoDesk.Application.Authentication.Contracts;
using NexoDesk.Application.Authentication.Interfaces;
using NexoDesk.Application.Authentication.Services;
using NexoDesk.Application.Authentication.Validators;
using NexoDesk.Application.Common.Exceptions;
using Moq;

namespace NexoDesk.Tests.Application;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflict()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(repository => repository.ExistsByEmailAsync(
                "user@helpdesk.local",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var passwordHasher = new Mock<IPasswordHasher>();
        var tokenGenerator = new Mock<ITokenGenerator>();
        var service = new AuthService(
            userRepository.Object,
            passwordHasher.Object,
            tokenGenerator.Object,
            new RegisterRequestValidator(),
            new LoginRequestValidator());

        await Assert.ThrowsAsync<ConflictException>(() => service.RegisterAsync(
            new RegisterRequest("Usuário", "user@helpdesk.local", "Senha123"),
            CancellationToken.None));

        passwordHasher.Verify(hasher => hasher.Hash(It.IsAny<string>()), Times.Never);
    }
}
