using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.common;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }
    }
}

