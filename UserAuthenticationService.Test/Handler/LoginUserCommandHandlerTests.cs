using Application.Features.Auth.Commands.Login;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Test.Handler
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _handler = new LoginUserCommandHandler(_unitOfWorkMock.Object, _jwtTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "password" };
            var user = new User
            {
                FullName = "Test User",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
            };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _jwtTokenGeneratorMock.Setup(j => j.GenerateToken(user)).Returns("access_token");
            _jwtTokenGeneratorMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh_token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.JwToken.Should().Be("access_token");
            result.Data.RefreshToken.Should().Be("refresh_token");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "password" };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Credenciales incorrectas");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPasswordIsInvalid()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "wrongpassword" };
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
            };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Credenciales incorrectas");
        }
    }
}
