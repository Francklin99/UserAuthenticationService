using Domain.Common;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.common
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private readonly AppDbContext _dbContext;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IRoleRepository> _roleRepository;

        public IUserRepository UserRepository => _userRepository.Value;
        public IRoleRepository RoleRepository => _roleRepository.Value;

        public UnitOfWork(
        AppDbContext dbContext,
        Lazy<IUserRepository> userRepository,
        Lazy<IRoleRepository> roleRepository
        )
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> Commit(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _dbContext.SaveChangesAsync(cancellationToken);
                return response >= 1;
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposed = true;
            }
        }

    }
}
