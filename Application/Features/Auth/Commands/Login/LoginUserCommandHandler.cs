using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoginReponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Response<LoginReponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<LoginReponseDto>();

            try
            {
                var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email, cancellationToken);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    throw new Exception("Credenciales incorrectas");
                }

                var accessToken = _jwtTokenGenerator.GenerateToken(user);

                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

                user.AddRefreshToken(refreshToken, DateTime.Now.AddDays(7));

                await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                var loginDto = new LoginReponseDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = user.Roles.Select(r => r.Name).ToList(),
                    JwToken = accessToken,
                    RefreshToken = refreshToken
                };

                response.Succeeded = true;
                response.Data = loginDto;
                response.Message = "Inicio de sesión exitoso";
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
                response.Errors = [ex.Message];
            }
            
            return response;
        }
    }
}
