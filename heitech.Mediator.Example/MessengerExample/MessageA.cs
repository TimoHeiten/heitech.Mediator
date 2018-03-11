using heitech.MediatorMessenger.Implementation.Messages;

namespace heitech.Mediator.Example.MessengerExample
{
    internal class MessageA_WithContent : MessageBase<string>
    {
        internal string MessageFromB { get; }

        internal MessageA_WithContent(string msg) 
            : base("B", new string[] { "A" })
        {
            MessageFromB = msg;
        }
    }
}
