using heitech.MediatorMessenger.Exceptions;
using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace heitech.MediatorMessenger.Tests
{
    internal class RegistererMock : IRegisterer<string>
    {
        internal Predicate<string> PredicateRegistered;

        public bool IsRegistered(string key) => PredicateRegistered(key);

        public IMediator<string> Mediator => throw new NotImplementedException();
        public void Register(IMessenger<string> messenger) => throw new NotImplementedException();
        public void Unregister(string adress) => throw new NotImplementedException();
        public void Unregister(IMessenger<string> messenger) => throw new NotImplementedException();

        internal int CountGetterCalls { get; private set; }
        internal List<IMessenger<string>> Messengers { get; } = new List<IMessenger<string>>();

        public IMessenger<string> Get(string key)
        {
            if (PredicateRegistered != null && !PredicateRegistered(key))
                throw new MessengerIdentifierNotRegisteredException("abcaffeschnee");

            CountGetterCalls++;
            return Messengers.Single(x => x.MessengerIdentifier == key);
        }
    }
}
