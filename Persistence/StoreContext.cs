using Domain;
using Domain.ExplicitJoinEntities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {

        }

        // Each DbSet<T> represents a DB Table for Entity T
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }
}