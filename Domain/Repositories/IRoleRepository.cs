using Domain.Common;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}

