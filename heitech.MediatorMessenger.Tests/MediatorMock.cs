using heitech.MediatorMessenger.Interface;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Tests
{
    internal class MediatorMock : IInternalMediatorMessenger<string>
    {
        internal IRequestObject<string> SentQuery { get; private set; }
        internal IMessageObject<string> SentMessage { get; private set; }

        public void Command(IMessageObject<string> message)
        {
            SentMessage = message;
        }

        public Task CommandAsync(IMessageObject<string> message)
        {
            SentMessage = message;
            return Task.CompletedTask;
        }


        internal object Result;

        public TResponse Query<TResponse>(IRequestObject<string> request)
        {
            SentQuery = request;
            return (TResponse)Result;
        }

        public Task<TResponse> QueryAsync<TResponse>(IRequestObject<string> request)
        {
            SentQuery = request;
            return Task.FromResult((TResponse)Result);
        }

        internal bool WasRegistererSet { get; private set; }
        public void SetRegisterer(IRegisterer<string> registerer)
        {
            WasRegistererSet = true;
        }
    }
}
