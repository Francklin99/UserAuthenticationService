using Application.Wrappers;
using MediatR;

namespace Application.Features.Auth.Commands.Register
{
    public record RegisterUserCommand(
        string FullName,
        string Email,
        string Password,
        string PhoneNumber,
        string RoleName
    ) : IRequest<Response<bool>>;
}
