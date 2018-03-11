using heitech.LinqXt.Enumerables;
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
            return messenger.ReceiveQuery<TResponse>(request);
        }

        public Task<TResponse> QueryAsync<TResponse>(IRequestObject<TKey> request)
        {
            CheckNull(request);
            IMessenger<TKey> messenger = GetMessenger(request.Receiver);
            return messenger.ReceiveQueryAsync<TResponse>(request);
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
