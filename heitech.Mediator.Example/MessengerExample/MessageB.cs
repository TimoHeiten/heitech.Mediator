using heitech.MediatorMessenger.Implementation.Messages;

namespace heitech.Mediator.Example.MessengerExample
{
    internal class MessageB : MessageBase<string>
    {
        internal MessageB() 
            : base("Program", new string[] { "B" })
        { }
    }
}
