namespace PushExample.Models
{
    public class MessageBase
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public MessageState State { get; set; } = MessageState.Pending;
    }
}
