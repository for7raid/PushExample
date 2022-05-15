using PushExample.Abstraction;
using PushExample.Models;
using System.Collections.Concurrent;

namespace PushExample.Implementation
{
    public class InMemoryMessageStore : IMessageStore
    {
        private static readonly ConcurrentDictionary<Guid, MessageBase> _store = new ConcurrentDictionary<Guid, MessageBase>();
        public Task<IEnumerable<MessageBase>> GetAll()
        {
            return Task.FromResult(_store.Values.AsEnumerable());
        }

        public Task<MessageBase> GetById(Guid id)
        {
            return Task.FromResult(_store[id]);
        }

        public Task<IEnumerable<MessageBase>> GetPending()
        {
            throw new NotImplementedException();
        }

        public Task Save(MessageBase messageBase)
        {
            _store.TryAdd(messageBase.Id, messageBase);
            return Task.CompletedTask;
        }
    }
}
