using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace heitech.Mediator.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // mediator demonstration
            // see TestOutlet.cs for Outlet Implementation
            // outlets are automatically registered.

            // create service collection
            var services = new ServiceCollection();
            services.AddMediator();

            // call all in service scope
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // calls simple outlet
                var opResult = mediator.ExecuteOutletAsync("test").Result; 
                System.Console.WriteLine(opResult.IsSuccess);
                var first = opResult.Result;
                System.Console.WriteLine(first);

                // calls typed outlet with untyped OperationResult
                var outletType = new OutletType { Message = "typed outlet" };
                opResult = mediator.ExecuteOutletAsync<OutletType>(outletType).Result;
                System.Console.WriteLine(opResult.IsSuccess);
                System.Console.WriteLine(opResult.Result);

                // // calls typed outlet with untyped OperationResult
                outletType = new OutletType { Message = "typed outlet" };
                var opResult2 = mediator.ExecuteOutletAsync<OutletType, OutletResult>(outletType).Result;
                System.Console.WriteLine(opResult2.IsSuccess);
                // // typed result
                System.Console.WriteLine(opResult2.Value.GetType());
                System.Console.WriteLine(opResult2.Value);
            }
        }

    }
}
