using Domain.ExplicitJoinEntities;

namespace Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public double Subtotal { get; set; }
        public double Tax { get; set; }
        public double ShippingCost { get; set; }
        public double Total { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public ShippingStatus Status { get; set; }

        // For the order entity class, we are choosing to include an Address navigation property
        // rather than AddressId foreign key. The foreign key will implicitly be created in the database anyway.
        // We are excluding the foreign key in this entity class, since there won't be much use for it.
        public Address Address { get; set; }
        public string AppUserId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }

    public enum ShippingStatus
    {
        Preparation,
        InTransit,
        OutForDelivery,
        Delivered
    }
}
