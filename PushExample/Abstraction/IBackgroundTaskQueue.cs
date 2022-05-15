namespace PushExample.Abstraction
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(Guid id);

        Task<Guid> DequeueAsync(
            CancellationToken cancellationToken);
    }
}
