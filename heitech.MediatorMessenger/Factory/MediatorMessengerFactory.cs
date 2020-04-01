using heitech.MediatorMessenger.Interface;
using System;

namespace heitech.MediatorMessenger.Factory
{
    ///<summary>
    /// Mediator with actual Messengers to decouple message and implementation
    ///</summary>
    public class MediatorMessengerFactory<TKey>
    {
        private Lazy<IRegisterer<TKey>> register;
        public MediatorMessengerFactory() 
        {
            register = new Lazy<IRegisterer<TKey>>(CreateNewMediator, isThreadSafe: true);
        }

        ///<summary>
        /// Create a New Singleton IRegister<TKey>
        ///</summary>
        public IRegisterer<TKey> CreateSingletonMediator() 
        {
            return register.Value;
        }

        ///<summary>
        /// Create a New IRegister<TKey>
        ///</summary>
        public IRegisterer<TKey> CreateNewMediator()
        {
            var mediator = new Implementation.Mediator.Mediator<TKey>();
            var register = new Implementation.Registerer.Registerer<TKey>(mediator);
            mediator.SetRegisterer(register);

            return register;
        }
    }
}
