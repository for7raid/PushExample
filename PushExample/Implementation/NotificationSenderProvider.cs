using PushExample.Abstraction;
using PushExample.Models;

namespace PushExample.Implementation
{
    public class NotificationSenderProvider : INotificationSenderProvider
    {
        public NotificationSenderProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public INotificationSender ProvideFor(Type messageType)
        {
            var senderType =
                 messageType == typeof(GoogleMessage) ? typeof(GoogleNotificationSender) :
                 messageType == typeof(AppleMessage) ? typeof(AppleNotificationSender) :
                 throw new InvalidDataException("No sender defined for message type " + messageType);

            var sender = ServiceProvider.GetRequiredService(senderType) as INotificationSender;

            return sender;
        }
    }
}
