using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
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

        public AppUser()
        {
            Orders = new List<Order>();
            PaymentDetails = new List<PaymentDetail>();
            Notifications = new List<Notification>();
            Addresses = new List<Address>();
        }
    }
}
