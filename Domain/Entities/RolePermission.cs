namespace Domain.Entities
{
    public class RolePermission
    {
        public int RoleId { get; private set; }

        public Role Role { get; private set; }

        public int PermissionId { get; private set; }

        public Permission Permission { get; private set; }
    }
}

