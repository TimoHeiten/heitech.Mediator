using System;

namespace heitech.MediatorMessenger.Exceptions
{
    public class MessengerIdentifierAlreadyRegisteredException : Exception
    {
        internal MessengerIdentifierAlreadyRegisteredException(string message) 
            : base(message)
        { }

        internal MessengerIdentifierAlreadyRegisteredException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }

}
