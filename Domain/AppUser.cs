using Microsoft.AspNetCore.Identity;

namespace Domain
{
    // IdentityUser is a special POCO from Microsoft.AspnetCore.Identity dll that contains a bunch of user-related data
    // like Username, Email, PasswordHash, SecurityStamp, etc.
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public virtual Cart Cart { get; set; }

        public List<Order> Orders { get; set; }

        public List<PaymentDetail> PaymentDetails { get; set; }

        public List<Notification> Notifications { get; set; }

        public List<Address> Addresses { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public AppUser()
        {
            Orders = new List<Order>();
            PaymentDetails = new List<PaymentDetail>();
            Notifications = new List<Notification>();
            Addresses = new List<Address>();
        }
    }
}
