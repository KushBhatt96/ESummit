namespace Domain
{
    public class PaymentDetail
    {
        public int PaymentDetailId { get; set; }
        public string CardholderName { get; set; }
        public CardType CardType { get; set; }
        public int Number { get; set; }
        public DateTime Expiration { get; set; }
        public string CVV { get; set; }
        public string ZipCode { get; set; }
        public int CustomerId { get; set; }
    }

    public enum CardType
    {
        Visa,
        Mastercard,
        Amex
    }
}
