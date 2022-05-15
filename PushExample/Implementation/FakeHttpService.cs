namespace PushExample.Implementation
{
    public class FakeHttpService
    {
        private int counter = 0;
        public async Task<bool> Send()
        {
            await Task.Delay(new Random().Next(500, 2000));
            return counter++ % 5 != 0;
        }
    }
}
