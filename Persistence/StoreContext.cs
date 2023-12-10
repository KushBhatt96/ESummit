using Domain;
using Domain.ExplicitJoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Common.Contants;

namespace Persistence
{
    public class StoreContext : IdentityDbContext<AppUser>
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // firstly call the OnModelCreating logic in the base class
            base.OnModelCreating(builder);

            // one of our DB tables will be AspNetRoles which contains all the different roles in our application
            // the following code will add Customer and Admin roles when we apply our migration
            // this is actually a way to seed data in entity framework
            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Name = RoleNames.Customer, NormalizedName = RoleNames.Customer.ToUpper() },
                    new IdentityRole { Name = RoleNames.Admin, NormalizedName = RoleNames.Admin.ToUpper() }
                );
        }

    }
}