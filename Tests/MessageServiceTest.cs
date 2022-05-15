using Microsoft.Extensions.DependencyInjection;
using PushExample.Abstraction;
using PushExample.Implementation;
using PushExample.Models;

namespace Tests
{
    [TestClass]
    public class MessageServiceTest
    {
        ServiceProvider serviceProvider;

        [TestInitialize]
        public void Prepare()
        {
            serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<FakeHttpService>()
                .AddScoped<INotificationSenderProvider, MockNotificationSenderProvider>()
                .AddScoped<IMessageService, MessageService>()
                .AddScoped<IMessageStore, InMemoryMessageStore>()
                .AddScoped<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .BuildServiceProvider();
        }

        [TestMethod]
        public void Save_and_Check_store()
        {
           
            var store = serviceProvider.GetRequiredService<IMessageStore>();
            var service = serviceProvider.GetRequiredService<IMessageService>();


            var message = new GoogleMessage { 
                Condition = nameof(GoogleMessage.Condition),
                DeviceToken = nameof(GoogleMessage.DeviceToken),
                Message = nameof(GoogleMessage.Message),
                Title = nameof(GoogleMessage.Title)
            };

            service.QueueMessageAsync(message);

            var saved = store.GetById(message.Id).Result as GoogleMessage;
            
            Assert.IsNotNull(saved);
            Assert.AreEqual(message.Id, saved.Id);
            Assert.AreEqual(message.Title, saved.Title);
            Assert.AreEqual(message.Condition, saved.Condition);
            Assert.AreEqual(message.Message, saved.Message);
            Assert.AreEqual(MessageState.Pending, saved.State);
            Assert.AreEqual(message.DeviceToken, saved.DeviceToken);
        }

        [TestMethod]
        public void Save_and_Check_Queue() {
            var service = serviceProvider.GetRequiredService<IMessageService>();
            var quque = serviceProvider.GetRequiredService<IBackgroundTaskQueue>();

            var message = new GoogleMessage
            {
                Condition = nameof(GoogleMessage.Condition),
                DeviceToken = nameof(GoogleMessage.DeviceToken),
                Message = nameof(GoogleMessage.Message),
                Title = nameof(GoogleMessage.Title)
            };

            service.QueueMessageAsync(message);

            var savedId = quque.DequeueAsync(new CancellationToken()).Result;

            Assert.AreEqual(message.Id, savedId);
           
        }

        [TestMethod]
        public void Send_and_check_Stage()
        {
            var service = serviceProvider.GetRequiredService<IMessageService>();
            var store = serviceProvider.GetRequiredService<IMessageStore>();

            var message = new GoogleMessage
            {
                Condition = nameof(GoogleMessage.Condition),
                DeviceToken = nameof(GoogleMessage.DeviceToken),
                Message = nameof(GoogleMessage.Message),
                Title = nameof(GoogleMessage.Title)
            };

            service.QueueMessageAsync(message).Wait();
            service.SendAsync(message.Id).Wait();

            var sended = store.GetById(message.Id).Result;

            Assert.AreEqual(MessageState.Sent, sended.State);

        }
    }

    public class MockNotificationSenderProvider : INotificationSenderProvider
    {
        public INotificationSender ProvideFor(Type messageType)
        {
            return new MockNotificationSender();
        }
    }

    public class MockNotificationSender : INotificationSender
    {
        public Task<MessageState> Send(MessageBase message)
        {
            return Task.FromResult(MessageState.Sent);
        }
    }

}