using heitech.MediatorMessenger.Interface;
using System;

namespace heitech.MediatorMessenger.Factory
{
    public class MediatorMessengerFactory<TKey>
    {
        private Lazy<IRegisterer<TKey>> register;
        public MediatorMessengerFactory() 
            => register = new Lazy<IRegisterer<TKey>>(CreateNewMediator, isThreadSafe: true);

        public IRegisterer<TKey> CreateSingletonMediator() => register.Value;
        public IRegisterer<TKey> CreateNewMediator()
        {
            var mediator = new Implementation.Mediator.Mediator<TKey>();
            var register = new Implementation.Registerer.Registerer<TKey>(mediator);
            mediator.SetRegisterer(register);

            return register;
        }
    }
}
