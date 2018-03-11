using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Implementation.Messenger
{
    /// <summary>
    /// Supplies all handling of messages/requests that are added in the Initialize method
    /// </summary>
    public abstract class MessengerBase<TKey> : IMessenger<TKey>
    {
        public abstract TKey MessengerIdentifier { get; }
        /// <summary>
        /// Initialize/Register all expected Messages/Requests.
        /// called from ctor
        /// </summary>
        protected abstract void InitializeMessages();

        protected MessengerBase(bool initializeMessages = true)
        {
            if (initializeMessages)
                InitializeMessages();
        }

        private readonly Dictionary<Type, IMessage> messages = new Dictionary<Type, IMessage>();
        private readonly Dictionary<RequestResultContainer, IRequest> requests = new Dictionary<RequestResultContainer, IRequest>();

        public virtual void ReceiveCommand(IMessageObject<TKey> message)
            =>  messages[message.GetType()].Invoke(message);

        public virtual TResult ReceiveQuery<TResult>(IRequestObject<TKey> request)
        {
            RequestResultContainer _iRequest = requests.Keys.FirstOrDefault(x => x.HasRequestedTypeAndExpectedResult(request.GetType(), typeof(TResult)));
            if (_iRequest == null)
                throw new KeyNotFoundException();

            return (TResult)requests[_iRequest].InvokeRequest(request);
        }

        public MessengerBase<TKey> AddMessage<TMessageObject>(Action<TMessageObject> action)
            where TMessageObject : class, IMessageObject<TKey>
        {
            var message = new MessageWrapper<TMessageObject>();
            messages.Add(typeof(TMessageObject), message.Init(action));
            return this;
        }

        public MessengerBase<TKey> AddRequest<TRequestObject, TResult>(Func<TRequestObject, TResult> func)
            where TRequestObject : class, IRequestObject<TKey>
        {
            var container = new RequestResultContainer(typeof(TRequestObject), typeof(TResult));
            var request = new RequestWrapper<TKey, TRequestObject, TResult>();
            requests.Add(container, request.Init(func));

            return this;
        }

        // ------------------------------------------------------------------------------------------------------
        // async part
        // ------------------------------------------------------------------------------------------------------
        private readonly Dictionary<Type, IMessage> asyncMessages = new Dictionary<Type, IMessage>();
        private readonly Dictionary<RequestResultContainer, IRequest> asyncRequests = new Dictionary<RequestResultContainer, IRequest>();

        public MessengerBase<TKey> AddMessage<TMessageObject>(Func<TMessageObject, Task> action)
           where TMessageObject : class, IMessageObject<TKey>
        {
            var message = new MessageWrapper<TMessageObject>();
            messages.Add(typeof(TMessageObject), message.InitAsync(action));
            return this;
        }

        public MessengerBase<TKey> AddRequest<TRequestObject, TResult>(Func<TRequestObject, Task<TResult>> func)
           where TRequestObject : class, IRequestObject<TKey>
        {
            var container = new RequestResultContainer(typeof(TRequestObject), typeof(TResult));
            var request = new RequestWrapper<TKey, TRequestObject, TResult>();
            asyncRequests.Add(container, request.InitAsync(func));

            return this;
        }

        public Task ReceiveCommandAsync(IMessageObject<TKey> message)
             => asyncMessages[message.GetType()].InvokeAsync(message);

        public async Task<TResult> ReceiveQueryAsync<TResult>(IRequestObject<TKey> request)
        {
            var _iRequest = requests.Keys.FirstOrDefault(x => x.HasRequestedTypeAndExpectedResult(request.GetType(), typeof(TResult)));
            if (_iRequest == null)
                throw new KeyNotFoundException();

            object o = await asyncRequests[_iRequest].InvokeRequestAsync(request);
            return (TResult)o;
        }
    }
}
