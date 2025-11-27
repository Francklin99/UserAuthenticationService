using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Features.Auth.Commands.ValidateToken
{
    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, (bool IsValid, string Error)>
    {
        private readonly TokenValidationParameters _tokenValidationParams;

        public ValidateTokenCommandHandler(IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            _tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)
                )
            };


        }

        public Task<(bool IsValid, string Error)> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(request.Token, _tokenValidationParams, out _);

                return Task.FromResult((true, ""));
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, ex.Message));
            }
        }
    }
}
