using System;

namespace heitech.Mediator.Exceptions
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message) 
            : base(message)
        { }
    }
}
