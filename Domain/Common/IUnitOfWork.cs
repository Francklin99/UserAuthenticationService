using Domain.Repositories;

namespace Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task<bool> Commit(CancellationToken cancellationToken);
    }
}
