using PushExample.Models;

namespace PushExample.Abstraction
{
    public interface IMessageStore
    {
        public Task Save(MessageBase messageBase);
        public Task<IEnumerable<MessageBase>> GetAll();
        public Task<IEnumerable<MessageBase>> GetPending();
        public Task<MessageBase> GetById(Guid id);
    }
}
