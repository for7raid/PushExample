using PushExample.Implementation;
using PushExample.Models;

namespace Tests
{
    [TestClass]
    public class StoreTest
    {
        [TestMethod]
        public void Save_and_GetById()
        {
            var store = new InMemoryMessageStore();
            var message = new GoogleMessage { 
                Condition = nameof(GoogleMessage.Condition),
                DeviceToken = nameof(GoogleMessage.DeviceToken),
                Message = nameof(GoogleMessage.Message),
                Title = nameof(GoogleMessage.Title)
            };

            store.Save(message).Wait();

            var saved = store.GetById(message.Id).Result as GoogleMessage;
            
            Assert.IsNotNull(saved);
            Assert.AreEqual(message.Id, saved.Id);
            Assert.AreEqual(message.Title, saved.Title);
            Assert.AreEqual(message.Condition, saved.Condition);
            Assert.AreEqual(message.Message, saved.Message);
            Assert.AreEqual(MessageState.Pending, saved.State);
            Assert.AreEqual(message.DeviceToken, saved.DeviceToken);
        }
    }
}