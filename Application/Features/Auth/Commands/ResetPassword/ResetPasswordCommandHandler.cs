using Domain.Repositories;
using MediatR;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new Exception("Solicitud no válida.");

            if (user.PasswordResetToken != request.Token || user.PasswordResetTokenExpiry < DateTime.Now)
            {
                throw new Exception("Token no válido o caducado.");
            }

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.UpdatePassword(newPasswordHash);

            user.ClearPasswordResetToken();

            await _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
