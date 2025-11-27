using Application.Features.Auth.Commands.ForgotPassword;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.ResetPassword;
using Application.Features.Auth.Commands.ValidateToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registrar un nuevo usuario
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Iniciar sesión
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Recuperar contraseña
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "If the email exists, a reset token has been sent." });
        }

        /// <summary>
        /// Restablecer contraseña
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command) 
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Password reset successfully." });
        }

        /// <summary>
        /// Validar token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenCommand command)
        {
            var (isValid, error) = await _mediator.Send(command);
            if (!isValid)
            {
                return Unauthorized(new { Message = $"Invalid token: {error}" });
            }
            return Ok(new { IsValid = true });
        }
    }
}
