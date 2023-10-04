using Domain.ExplicitJoinEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Cart
    {
        public int CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public double CartTotal { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
