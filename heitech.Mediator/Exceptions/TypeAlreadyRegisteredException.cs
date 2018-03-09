using System;

namespace heitech.Mediator.Exceptions
{
    public class TypeAlreadyRegisteredException : Exception
    {
        public TypeAlreadyRegisteredException(string message) 
            : base(message)
        { }

        public TypeAlreadyRegisteredException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }
}
