using Domain.Common;

namespace Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Module { get; private set; }

        public string Action { get; private set; }

        public string Description { get; private set; }

        private Permission() { }

        public Permission(string module, string action, string description)
        {
            Module = module;
            Action = action;
            Description = description;
        }
    }
}

