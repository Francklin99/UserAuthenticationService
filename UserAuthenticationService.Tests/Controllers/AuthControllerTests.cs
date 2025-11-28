using API.Controllers;
using Application.Features.Auth.Commands.Login;
using Application.Wrappers;
using Application.DTOs;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace UserAuthenticationService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "password" };
            var responseDto = new LoginReponseDto { Email = "test@example.com", JwToken = "token" };
            var response = new Response<LoginReponseDto>(responseDto, "Success");

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Login(command);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(response);
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
