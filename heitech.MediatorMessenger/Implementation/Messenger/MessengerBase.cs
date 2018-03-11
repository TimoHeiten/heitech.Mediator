using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Implementation.Messenger
{
    public abstract class MessengerBase<TKey> : IMessenger<TKey>
    {
        protected Dictionary<Type, Action<object>> commands = new Dictionary<Type, Action<object>>();
        protected Dictionary<Type, Func<Task>> asyncCommands = new Dictionary<Type, Func<Task>>();

        protected Dictionary<Type, object> queries = new Dictionary<Type, object>();
        protected Dictionary<Type, object> asyncQueries = new Dictionary<Type, object>();

        public TKey MessengerIdentifier { get; }
        public MessengerBase(TKey key) => MessengerIdentifier = key;

        public abstract IMessenger<TKey> Initialize();

        protected IMessenger<TKey> AddCommand(Type msgKey, Action<object> action)
        {
            commands.Add(msgKey, action);
            return this;
        }

        protected IMessenger<TKey> AddAsyncCommand(Type msgKey, Func<Task> func)
        {
            asyncCommands.Add(msgKey, func);
            return this;
        }

        /// <summary>
        /// Add a Func with your expected return type
        /// make sure it matches exactly else a CastException is to be expected.
        /// </summary>
        protected IMessenger<TKey> AddQuery(Type msgKey, object func)
        {
            queries.Add(msgKey, func);
            return this;
        }

        // todo register FuncFinderInvoker like object.

        /// <summary>
        /// Add a Func with your expected return type from Task
        /// make sure it matches exactly else a CastException is to be expected.
        /// </summary>
        protected IMessenger<TKey> AddAsyncQuery(Type msgKey, object funcTask)
        {
            asyncQueries.Add(msgKey, funcTask);
            return this;
        }

        public void ReceiveCommand(IMessageObject<TKey> message)
            => commands[message.GetType()](message);

        public Task ReceiveCommandAsync(IMessageObject<TKey> message)
            => asyncCommands[message.GetType()]();

        public object ReceiveQuery(IRequestObject<TKey> request)
            => new object();
        // does not work=> queries[request.GetType()]();

        public Task<object> ReceiveQueryAsync(IRequestObject<TKey> request)
            => Task.FromResult(new object());
    }
}
