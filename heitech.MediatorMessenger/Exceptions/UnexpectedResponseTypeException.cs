using System;

namespace heitech.MediatorMessenger.Exceptions
{
    public class UnexpectedResponseTypeException : Exception
    {
        public UnexpectedResponseTypeException(string message) 
            : base(message)
        { }
    }
}
