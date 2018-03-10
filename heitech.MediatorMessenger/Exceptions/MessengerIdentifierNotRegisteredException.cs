using System;

namespace heitech.MediatorMessenger.Exceptions
{
    public class MessengerIdentifierNotRegisteredException : Exception
    {
        internal MessengerIdentifierNotRegisteredException(string message) 
            : base(message)
        { }
    }

}
