using MediatR;

namespace Application.Features.Auth.Commands.ValidateToken
{
    public class ValidateTokenCommand : IRequest<(bool IsValid, string Error)>
    {
        public required string Token { get; set; }
    }
}
