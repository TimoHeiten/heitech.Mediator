using heitech.LinqXt.Enumerables;
using heitech.MediatorMessenger.Exceptions;
using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Implementation.Mediator
{
    internal class Mediator<TKey> : IInternalMediatorMessenger<TKey>
    {
        private IRegisterer<TKey> registerer;

        public void Command(IMessageObject<TKey> message)
        {
            CheckNull(message);
            ForAllReceivers(message.Receivers).ForAll(x => x.ReceiveCommand(message));
        }

        public async Task CommandAsync(IMessageObject<TKey> message)
        {
            CheckNull(message);
            var tasks = new List<Task>();
            ForAllReceivers(message.Receivers).ForAll(x => tasks.Add(x.ReceiveCommandAsync(message)));

            await Task.WhenAll(tasks);
        }

        private IMessenger<TKey> GetMessenger(TKey key) => registerer.Get(key);

        private IEnumerable<IMessenger<TKey>> ForAllReceivers(IEnumerable<TKey> receivers)
        {
            foreach (TKey identifier in receivers)
                yield return GetMessenger(identifier);
        }


        public TResponse Query<TResponse>(IRequestObject<TKey> request)
        {
            CheckNull(request);
            IMessenger<TKey> messenger = GetMessenger(request.Receiver);
            object response = messenger.ReceiveQuery(request);
            CheckCorrectType<TResponse>(response);

            return (TResponse)response;
        }

        public async Task<TResponse> QueryAsync<TResponse>(IRequestObject<TKey> request)
        {
            CheckNull(request);
            IMessenger<TKey> messenger = GetMessenger(request.Receiver);
            object response = await messenger.ReceiveQueryAsync(request);
            CheckCorrectType<TResponse>(response);

            return (TResponse)response;
        }

        private void CheckCorrectType<TResponse>(object messengerResponse)
        {
            if (messengerResponse == null)
                throw new ArgumentException("response must not be null. Use heitech.Implementation.Messages.NullableResponse<T> for this purpose");

            Type result = messengerResponse.GetType();
            Type expected = typeof(TResponse);

            if (!AreTypesRelated(result, expected))
            {
                throw new UnexpectedResponseTypeException($"Response of type {expected.Name} was expected." +
                    $"Yet a response of type {result.Name} was given");
            }
        }

        private bool AreTypesRelated(Type expected, Type other)
            => expected == other
            || other.IsAssignableFrom(expected)
            || other.IsSubclassOf(expected);

        public void SetRegisterer(IRegisterer<TKey> registerer) => this.registerer = registerer;

        private object CheckNull(object obj)
            => obj ?? throw new ArgumentException($"Message/Requestobject must not be null");
    }
}
