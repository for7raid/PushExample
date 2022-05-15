using PushExample.Models;

namespace PushExample.Abstraction
{
    public interface INotificationSender
    {
        Task<MessageState> Send(MessageBase message);
    }
}