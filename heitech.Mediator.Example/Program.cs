using heitech.Mediator.Example.MessengerExample;
using heitech.Mediator.Interface;
using heitech.MediatorMessenger.Interface;
using System;
using System.Threading.Tasks;

namespace heitech.Mediator.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            DemonstrateSimpleMediator();

            Console.WriteLine("");
            Console.WriteLine("now the complex one:");
            Console.ReadKey();
            DemonstrateComplexMediator();
            Console.ReadKey();
        }

        private static void DemonstrateComplexMediator()
        {
            var factory = new MediatorMessenger.Factory.MediatorMessengerFactory<string>();
            IRegisterer<string> reg = factory.CreateNewMediator();
            IMediator<string> mediator = reg.Mediator;

            var messenger_A = new Messenger_A();
            var messenger_B = new Messenger_B("this was sent from B to A", mediator);

            reg.Register(messenger_A);
            reg.Register(messenger_B);

            // sends this to Messenger_B
            // which in turn sends a command To messenger_a that handles it with writing to the console
            mediator.Command(new MessageB());
        }

        private static void DemonstrateSimpleMediator()
        {
            var register = new Mediator.Factory.MediatorFactory().CreateNewMediator();
            register.Register<IAmATestInterface>(new AmATestClass());
            IMediator mediator = register.Mediator;

            mediator.Command<IAmATestInterface>(m => m.Action());
            mediator.CommandAsync<IAmATestInterface>(m => m.ActionAsync()).Wait();

            Console.WriteLine(mediator.Query<IAmATestInterface, string>(m => m.Result()));
        }

        private interface IAmATestInterface
        {
            void Action();
            Task ActionAsync();

            string Result();
            Task<string> TaskResult();
        }

        private class AmATestClass : IAmATestInterface
        {
            public AmATestClass()
            { }

            public void Action()
                => Console.WriteLine("called via Command from mediator");

            public Task ActionAsync()
            {
                Console.WriteLine("CommandAsync from mediator");
                return Task.CompletedTask;
            }

            public string Result() => "Result from " + GetType().Name;
            public Task<string> TaskResult() => Task.FromResult("Async Result from " + GetType().Name);
        }
    }
}
