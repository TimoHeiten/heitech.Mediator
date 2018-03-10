using System;

namespace heitech.Mediator.Exceptions
{
    public class TypeAlreadyRegisteredException : Exception
    {
        internal TypeAlreadyRegisteredException(string message) 
            : base(message)
        { }

        internal TypeAlreadyRegisteredException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }
}
