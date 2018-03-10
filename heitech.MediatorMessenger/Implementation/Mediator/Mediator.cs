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
            => ForAllReceivers(message.Receivers)
            .ForAll(x => x.ReceiveCommand(message));

        private IEnumerable<IMessenger<TKey>> ForAllReceivers(IEnumerable<TKey> receivers)
        {
            foreach (TKey identifier in receivers)
                yield return registerer.Get(identifier);
        }

        public async Task CommandAsync(IMessageObject<TKey> message)
        {
            var tasks = new List<Task>();
            ForAllReceivers(message.Receivers).ForAll(x => tasks.Add(x.ReceiveCommandAsync(message)));

            await Task.WhenAll(tasks);
        }

        public TResponse Query<TResponse>(IRequestObject<TKey> request)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> QueryAsync<TResponse>(IRequestObject<TKey> request)
        {
            throw new NotImplementedException();
        }

        public void SetRegisterer(IRegisterer<TKey> registerer) => this.registerer = registerer;
    }

    // todo use heitech.Linq_Xt
    internal static class LinqExt
    {
        internal static void ForAll<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource item in source) action(item);
        }
    }
}
