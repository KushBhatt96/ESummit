namespace Domain.ExplicitJoinEntities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        // This is a navigation property to the parent Product entity
        public Product Product { get; set; }

        public int Quantity { get; set; }

        // TODO: Get this to work later
        //public double TotalPrice => Product.Price * Quantity;
    }
}
