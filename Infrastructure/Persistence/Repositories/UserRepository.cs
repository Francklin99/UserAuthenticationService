using Domain.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Persistence.common;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(u => u.RefreshTokens)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public override async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(u => u.RefreshTokens)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(u => u.RefreshTokens)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == token), cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}
