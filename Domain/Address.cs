namespace Domain
{
    public class Address
    {
        public int AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string AppUserId { get; set; }

        // This represents a list of all the orders that are being delivered to this address, as per the ERD
        public List<Order> Orders { get; set; }

        public Address()
        {
            Orders = new List<Order>();
        }
    }
}