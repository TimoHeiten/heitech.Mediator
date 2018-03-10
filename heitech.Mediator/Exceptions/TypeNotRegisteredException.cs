using System;

namespace heitech.Mediator.Exceptions
{
    public class TypeNotRegisteredException : Exception
    {
        internal TypeNotRegisteredException(string message) 
            : base(message)
        { }
    }
}
