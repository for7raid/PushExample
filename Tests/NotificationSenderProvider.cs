using Microsoft.Extensions.DependencyInjection;
using PushExample.Abstraction;
using PushExample.Implementation;
using PushExample.Models;

namespace Tests
{
    [TestClass]
    public class NotificationSenderProviderTests
    {
        [DataTestMethod]
        [DataRow(typeof(AppleMessage), typeof(AppleNotificationSender))]
        [DataRow(typeof(GoogleMessage), typeof(GoogleNotificationSender))]
        public void Resolve_sender(Type messageType, Type senderType)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<FakeHttpService>()
                .AddScoped<INotificationSenderProvider, NotificationSenderProvider>()
                .AddScoped<GoogleNotificationSender>()
                .AddScoped<AppleNotificationSender>()
                .BuildServiceProvider();

            var provider = serviceProvider.GetService<INotificationSenderProvider>();
            var sender = provider.ProvideFor(messageType);

            Assert.AreEqual(senderType, sender.GetType());
        }
    }
}