using Application.Wrappers;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();

            try
            {
                if (!await _unitOfWork.UserRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
                {
                    throw new Exception("El correo ya existe.");
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new User(
                    request.FullName,
                    request.Email,
                    passwordHash,
                    request.PhoneNumber
                );

                // Buscar el rol por nombre
                var role = await _unitOfWork.RoleRepository.GetByNameAsync(request.RoleName, cancellationToken);

                if (role == null)
                {
                    throw new Exception($"El rol '{request.RoleName}' no existe.");
                }

                user.AssignRole(role);

                await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);

                if(await _unitOfWork.Commit(cancellationToken))
                {
                    response.Succeeded = true;
                    response.Message = "El usuario se ha registrado correctamente.";
                    response.Data = true;
                }
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
