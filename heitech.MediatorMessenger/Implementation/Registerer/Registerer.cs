using heitech.MediatorMessenger.Exceptions;
using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("heitech.MediatorMessenger.Tests")]

namespace heitech.MediatorMessenger.Implementation.Registerer
{
    internal class Registerer<TKey> : IRegisterer<TKey>
    {
        public IMediator<TKey> Mediator { get; }
        protected Dictionary<TKey, IMessenger<TKey>> Messengers { get; }
            = new Dictionary<TKey, IMessenger<TKey>>();

        internal Registerer(IInternalMediatorMessenger<TKey> mediator)
        {
            Mediator = mediator;
            mediator.SetRegisterer(this);
        }

        public void Register(IMessenger<TKey> messenger)
        {
            TKey key = messenger.MessengerIdentifier;
            if (!Messengers.ContainsKey(key))
            {
                Messengers.Add(messenger.MessengerIdentifier, messenger);
            }
            else throw new MessengerIdentifierAlreadyRegisteredException($"{key.ToString()} already registered");
        }

        public void Unregister(TKey address)
        {
            if (Messengers.ContainsKey(address))
            {
                Messengers.Remove(address);
            }
            else throw new MessengerIdentifierNotRegisteredException($"{address.ToString()} not registered");
        }

        public void Unregister(IMessenger<TKey> messenger)
        {
            Unregister(messenger.MessengerIdentifier);
        }

        public bool IsRegistered(TKey key) 
        {
            return Messengers.ContainsKey(key);
        }

        public IMessenger<TKey> Get(TKey key)
        {
            if (Messengers.TryGetValue(key, out IMessenger<TKey> messenger))
            {
                return messenger;
            }
            else 
            {
                throw new MessengerIdentifierNotRegisteredException($"{key.ToString()} not registered");
            }
        }
    }
}
