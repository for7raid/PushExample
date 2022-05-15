using PushExample.Abstraction;
using System.Threading.Channels;

namespace PushExample.Implementation
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Guid> _queue;

        public BackgroundTaskQueue()
        {
            // Capacity should be set based on the expected application load and
            // number of concurrent threads accessing the queue.            
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Guid>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(
            Guid id)
        {
            
            await _queue.Writer.WriteAsync(id);
        }

        public async Task<Guid> DequeueAsync(
            CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
