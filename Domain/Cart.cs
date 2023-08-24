using Domain.ExplicitJoinEntities;

namespace Domain
{
    public class Cart
    {
        public int CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public double CartTotal { get; set; }
        public int CustomerId { get; set; }

        public List<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
