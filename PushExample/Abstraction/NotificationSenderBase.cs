using PushExample.Models;

namespace PushExample.Abstraction
{
    public abstract class NotificationSenderBase<T> : INotificationSender where T : MessageBase
    {
        protected ILogger Logger { get; set; }

        public NotificationSenderBase(ILogger logger)
        {
            Logger = logger;
        }

        public virtual Task<MessageState> Send(MessageBase message)
        {
            Logger.LogInformation(this.GetType().Name+ ": " + message.ToString());
            return Task.FromResult(MessageState.Pending);
        }
    }
}
