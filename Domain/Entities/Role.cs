using Domain.Common;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        private readonly List<Permission> _permissions = new();

        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

        private Role() { }

        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void AddPermission(Permission permission)
        {
            if (!_permissions.Contains(permission))
                _permissions.Add(permission);
        }
    }
}

