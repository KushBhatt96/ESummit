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
        public int CustomerId { get; set; }
    }
}