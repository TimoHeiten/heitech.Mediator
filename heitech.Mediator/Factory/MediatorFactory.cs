using heitech.Mediator.Implementation;
using heitech.Mediator.Interface;
using System;

namespace heitech.Mediator.Factory
{
    public class MediatorFactory
    {
        private Lazy<IRegister> register;
        public MediatorFactory() => register = new Lazy<IRegister>(CreateNewMediator, isThreadSafe:true);

        public IRegister CreateSingletonMediator() => register.Value;
        public IRegister CreateNewMediator()
        {
            var mediator = new Implementation.Mediator();
            var register = new Registerer(mediator);
            mediator.SetRegister(register);

            return register;
        }
    }
}
