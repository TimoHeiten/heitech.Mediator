using heitech.MediatorMessenger.Implementation.Messenger;
using heitech.MediatorMessenger.Interface;
using System;
using System.Linq;

namespace heitech.Mediator.Example.MessengerExample
{
    internal class Messenger_A : MessengerBase<string>
    {
        public override string MessengerIdentifier => "A";
        internal Messenger_A(bool initializeMessages = true)
            : base(initializeMessages)
        { }

        protected override void InitializeMessages()
            => AddMessage<MessageA_WithContent>(a => System.Console.WriteLine($"this just came in from: {a.GetType().Name} with recipeint: "+ 
                $"{a.Receivers.ElementAt(0)} {Environment.NewLine}with message: {a.MessageFromB}"));
    }

    internal class Messenger_B : MessengerBase<string>
    {
        string msg;
        public override string MessengerIdentifier => "B";

        internal Messenger_B(string msg, IMediator<string> medi)
            : base(true)
        {
            this.msg = msg;
            this.mediator = medi;
        }

        private readonly IMediator<string> mediator;

        public Messenger_B(IMediator<string> mediator)
            => this.mediator = mediator;

        protected override void InitializeMessages()
            => AddMessage<MessageB>(b => mediator.Command(new MessageA_WithContent(msg)));
    }
}
