using Domain.ExplicitJoinEntities;

namespace Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType Type { get; set; }
        public string Brand { get; set; }
        public int QuantityInStock { get; set; }

        public List<CartItem> CartItems { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Product()
        {
            CartItems = new List<CartItem>();
            OrderItems = new List<OrderItem>();
        }
    }

    public enum ProductType
    {
        Shirts,
        Pants,
        Footwear,
        Jackets,
        Accessories,
    }
}