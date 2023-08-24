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
        public Address ShippingAddress { get; set; }
        public int CustomerId { get; set; }
        //public List<Product> Products { get; set; }

        //public Order()
        //{
        //    Products = new List<Product>();
        //}
    }

    public enum ShippingStatus
    {
        Preparation,
        InTransit,
        OutForDelivery,
        Delivered
    }
}
