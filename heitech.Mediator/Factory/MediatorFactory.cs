using heitech.Mediator.Implementation;
using heitech.Mediator.Interface;
using System;

namespace heitech.Mediator.Factory
{
    ///<summary>
    /// Use the factory to create an instance of the MediatorRegister
    ///</summary>
    public class MediatorFactory
    {
        private Lazy<IRegister> register;
        public MediatorFactory() 
        {
            register = new Lazy<IRegister>(CreateNewMediator, isThreadSafe:true);
        }

        ///<summary>
        /// Create a Single Instance Mediator
        ///</summary>
        public IRegister CreateSingletonMediator() 
        {
            return register.Value;
        }

        ///<summary>
        /// Create a new IRegister to specify all receivers
        ///</summary>
        public IRegister CreateNewMediator()
        {
            var mediator = new Implementation.Mediator();
            var register = new Registerer(mediator);
            mediator.SetRegister(register);

            return register;
        }
    }
}
