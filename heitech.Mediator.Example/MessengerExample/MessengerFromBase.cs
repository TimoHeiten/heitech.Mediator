using heitech.MediatorMessenger.Implementation.Messenger;

namespace heitech.Mediator.Example.MessengerExample
{
    internal class MessengerFromBase : MessengerBase<string>
    {
        public override string MessengerIdentifier => "fromBase";

        protected override void InitializeMessages()
        {
            // todo
        }
    }
}
