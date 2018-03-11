using heitech.MediatorMessenger.Interface;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Tests
{
    internal class MessengerMock : IMessenger<string>
    {
        internal bool WasInitialized { get; private set; }
        internal bool ReceivedCommand { get; private set; }
        internal bool ReceivedAsyncCommand { get; private set; }

        internal bool ReceivedFunc { get; private set; }
        internal bool ReceivedFuncAsync { get; private set; }

        private string ident;
        public string MessengerIdentifier => ident;

        internal MessengerMock() : this("Mock") { }
        internal MessengerMock(string ident) => this.ident = ident;

        public IMessenger<string> Initialize()
        {
            WasInitialized = true;
            return this;
        }

        internal IMessageObject<string> IncommingMessage { get; private set; }

        public void ReceiveCommand(IMessageObject<string> message)
        {
            ReceivedCommand = true;

        }

        public Task ReceiveCommandAsync(IMessageObject<string> message)
        {
            ReceivedAsyncCommand = true;
            return Task.CompletedTask;
        }

        internal object Result;

        public TResult ReceiveQuery<TResult>(IRequestObject<string> request)
        {
            ReceivedFunc = true;
            return (TResult)Result;
        }

        public Task<T> ReceiveQueryAsync<T>(IRequestObject<string> request)
        {
            ReceivedFuncAsync = true;
            return Task.FromResult((T)Result);
        }
    }
}
