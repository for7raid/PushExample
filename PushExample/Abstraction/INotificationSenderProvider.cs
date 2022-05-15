namespace PushExample.Abstraction
{
    public interface INotificationSenderProvider
    {
        INotificationSender ProvideFor(Type messageType);
    }
}
