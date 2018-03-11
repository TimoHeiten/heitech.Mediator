using heitech.Mediator.Interface;
using System;
using System.Threading.Tasks;

namespace heitech.Mediator.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            DemonstrateSimpleMediator();

            Console.ReadKey();
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
