using Domain.Repositories;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                // Do not reveal that the user does not exist
                return;
            }

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.Now.AddHours(1);

            user.SetPasswordResetToken(token, expiry);
            await _userRepository.UpdateAsync(user, cancellationToken);

            await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Your reset token is: {token}");
        }
    }
}
