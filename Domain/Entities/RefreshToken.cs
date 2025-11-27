using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; private set; }

        public string Token { get; private set; }

        public DateTime ExpiryDate { get; private set; }

        public bool IsRevoked { get; private set; }

        private RefreshToken() { }

        public RefreshToken(int userId, string token, DateTime expiryDate)
        {
            UserId = userId;
            Token = token;
            ExpiryDate = expiryDate;
            IsRevoked = false;
        }

        public void Revoke()
        {
            IsRevoked = true;
            UpdateAudit();
        }
    }
}
