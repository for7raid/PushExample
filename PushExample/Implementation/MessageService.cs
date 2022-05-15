using PushExample.Abstraction;
using PushExample.Models;

namespace PushExample.Implementation
{
    public class MessageService : IMessageService
    {
        public MessageService(ILogger<MessageService> logger, IMessageStore messageStore, IBackgroundTaskQueue taskQueue, INotificationSenderProvider notificationSenderProvider)
        {
            Logger = logger;
            MessageStore = messageStore;
            TaskQueue = taskQueue;
            NotificationSenderProvider = notificationSenderProvider;
        }

        ILogger<MessageService> Logger { get; }
        IMessageStore MessageStore { get; }
        IBackgroundTaskQueue TaskQueue { get; }
        public INotificationSenderProvider NotificationSenderProvider { get; }

        public async Task<MessageState> GetStateAsync(Guid id)
        {
            var message = await MessageStore.GetById(id);
            return message.State;
        }

        public async Task<Guid> QueueMessageAsync(MessageBase message)
        {
            await MessageStore.Save(message);
            await TaskQueue.QueueBackgroundWorkItemAsync(message.Id);
            return message.Id;
        }

        public async Task SendAsync(Guid messageId)
        {
            var message = await MessageStore.GetById(messageId);
            try
            {
                if (message == null) return;

                var sender = NotificationSenderProvider.ProvideFor(message.GetType());

                var state = await sender.Send(message);

                message.State = state;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Cant send message");
                message.State = MessageState.Error;
            }

            await MessageStore.Save(message);
        }

    }
}
