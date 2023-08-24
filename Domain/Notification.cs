namespace Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime SentAt { get; set; }
        public string Message { get; set; }

        public DateTime Expiration { get; set; }

        public string CustomerId { get; set; }

    }
}
