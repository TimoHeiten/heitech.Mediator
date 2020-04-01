using heitech.Mediator.Exceptions;
using heitech.Mediator.Interface;
using System;
using System.Collections.Generic;

namespace heitech.Mediator.Implementation
{
    ///<summary>
    /// Implementation of the IRegister
    ///</summary>
    internal class Registerer : IRegister
    {
        protected Dictionary<Type, object> Mediatables { get; } = new Dictionary<Type, object>();

        ///<summary>
        ///associated Mediator implementation
        ///</summary>
        public IMediator Mediator { get; }
        internal Registerer(IInternalMediator mediatorInstance)
        {
            Mediator = mediatorInstance;
            mediatorInstance.SetRegister(this);
        }

        public bool IsRegistered<T>() 
            where T : class
        {
            return Mediatables.ContainsKey(typeof(T));
        }

        public void Register<T>(T mediatable)
            where T : class
        {
            Type type = typeof(T);
            DoInterfaceAssertion(type);
            if (!Mediatables.ContainsKey(type))
            {
                Mediatables.Add(type, mediatable);
            }
            else
            {
                throw new TypeAlreadyRegisteredException($"{type.Name} is already registered with Mediator");
            }
        }

        private void DoInterfaceAssertion(Type t)
        {
            if (!t.IsInterface)
                throw new ArgumentException($"{t.Name} must implement an interface, yet does not");
        }

        public void Unregister<T>(T mediatable) 
            where T : class
        {
            Type type = typeof(T);
            if (Mediatables.ContainsKey(type))
            {
                Mediatables.Remove(type);
            }
            else
            {
                ThrowTypeNotRegistered(type);
            }
        }

        public T Get<T>()
        {
            if (Mediatables.TryGetValue(typeof(T), out object obj))
            {
                return (T)obj;
            }
            else
            {
                ThrowTypeNotRegistered(typeof(T));
                return default(T);  // just to satisfy compiler
            }
        }

        private void ThrowTypeNotRegistered(Type type)
            => throw new TypeNotRegisteredException($"{type.Name} not registered with Mediator");
    }
}
