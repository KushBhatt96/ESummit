namespace Domain.ExplicitJoinEntities
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public int CartId { get; set; }

        // This is a navigation property to the product parent entity
        public Product Product { get; set; }

        // We are using this explicit CartItem join entity here rather than skip navigations
        // as we need to track this additional Quantity property of a product in a cart
        public int Quantity { get; set; }

        // TODO: We make this a get-only property which is auto-calculated
        // since its value is strictly dependent on product price and cartItem quantity

        //public long TotalPrice => Product.Price * Quantity;
    }
}
