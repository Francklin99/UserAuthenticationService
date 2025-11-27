using Domain.Common;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public int UserId { get; private set; }

        public string Street { get; private set; }

        public string City { get; private set; }

        public string ZipCode { get; private set; }

        public string Country { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public bool IsMain { get; private set; }

        private Address() { }

        public Address(int userId, string street, string city, string zipCode, string country, bool isMain, double lat, double lng)
        {
            UserId = userId;
            Street = street;
            City = city;
            ZipCode = zipCode;
            Country = country;
            IsMain = isMain;
            Latitude = lat;
            Longitude = lng;
        }

        public void SetMain(bool isMain)
        {
            IsMain = isMain;
        }
    }
}
