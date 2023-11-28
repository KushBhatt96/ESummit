using Domain.ExplicitJoinEntities;

namespace Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string PictureUrl { get; set; }
        public string? TransitionPictureUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Sex { get; set; }

        public List<CartItem> CartItems { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Product()
        {
            CartItems = new List<CartItem>();
            OrderItems = new List<OrderItem>();
        }
    }

}