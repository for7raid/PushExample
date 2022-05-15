using PushExample.Models;

namespace PushExample.Abstraction
{
    public interface IMessageService
    {
        Task<Guid> QueueMessageAsync(MessageBase message);
        Task<MessageState> GetStateAsync(Guid id);
        Task SendAsync(Guid id);
    }
}
