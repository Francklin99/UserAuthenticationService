using Domain.Common;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken = default);

        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
    }
}
