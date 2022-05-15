using PushExample.Abstraction;
using PushExample.Models;

namespace PushExample.Implementation
{
    public class AppleNotificationSender : NotificationSenderBase<AppleMessage>
    {
        private readonly FakeHttpService _fakeHttpService;

        public AppleNotificationSender(ILogger<AppleNotificationSender> logger, FakeHttpService fakeHttpService) :
            base(logger)
        {
            _fakeHttpService = fakeHttpService;
        }

        public override async Task<MessageState> Send(MessageBase message)
        {
            await base.Send(message);
            try
            {
                var isSent = await _fakeHttpService.Send();

                return isSent ? MessageState.Sent : MessageState.Error;
            }
            catch (Exception e)
            {
                Logger.LogError("Error while sending message to Apple", e);
                return MessageState.Error;
            }


        }
    }
}
