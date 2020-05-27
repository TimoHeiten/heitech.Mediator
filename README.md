# heitech.Mediator

Mediator Design pattern that allows for automatic discovery
- Just add an IOutlet, IOutlet<T> or IOutlet<T, R> or all of them to a Handler Type.
- Use the IServiceCollection to add the components with the services.AddMediator() call
- see below code for inspiration or see the heitech.Mediator.Example app for more details


## Why would you want this?
With either of these implementations you encapsualte the method calls/object interactions of multiple registered items. The 

The best part is you can decouple two or more assemblies and make them only dependent on the assembly that uses the Mediator (and in case of the complex one implements all messages/interceptors)

A good use is if you have an asp.net web API and use the controllers only as delegates to some form of use cases. You can utilize the complex Mediator and create simple Message Objects in the controller and invoke them by calling 
```csharp
public ActionResult Post(Model model)
{
    var messageResult = await mediator.CommandAsync(new UseCaseMessage(model));
    return messageResult.Success
           ? Ok(model)
           : BadRequest(model);
}
```
The added benefit is that you only need to change Messageobjects receiver if you want to add more functionality and maybe adjust the Messenger Registration.
This decouples your Controllers from your implementations and use cases.

## Use of simple Mediator:

```csharp
// define an outlet with one, two or all three interfaces
// can be as many as you like it to be, just implement a different handler method for each one and make sure the types do not // overlap on a single class

public class TestOutlet : IOutlet, 
                          IOutlet<OutletType>,
                          IOutlet<OutletType, OutletResult>
{
    public async Task<OperationResult> ExecuteCommandAsync()
    {
        await Task.Delay(10);

        return OperationResult.Success(new { Message = "Outlet one is working fine "});
    }

    public Task<OperationResult> ExecuteOperationAsync(OutletType obj)
    {
        System.Console.WriteLine("Outlet with type also works");
        return Task.FromResult<OperationResult>(OperationResult.Success(obj));
    }

    public Task<OperationResult<OutletResult>> ExecuteFunctionAsync(OutletType obj)
    {
        System.Console.WriteLine("calling typed operationresult");
        return Task.FromResult
        (
            OperationResult<OutletResult>.Success
            (
                new OutletResult 
                {
                     ResultMessage = "result from typed operationresult"
                }
            )
        );

    }
    string IOutlet.OutletKey => "test";
}

// add it via AddMediator, it will automatically resolve all IOutlet versions
// then get hold of a service provider and 
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

```

// beware, not fully developed as of today
## Use of complex Mediator: (not included in Nuget package)
(implementation of messageobjectBase, MessengerBase and Utils is todo, only scaffolding ready)

The complex Mediator uses a generic Key to let you decide how you want to reference it.
First create a IMessenger that handles all ReceiveCommand, Receivequery etc. on incoming Message/RequestObjects
Implement your own IMessageobject or derive from abstract baseclass MessageBase. (analog for RequestObject) 
```csharp
class MyMessenger : IMessenger<string>
{
    public string MessengerIdentifier => "anyName";
    public void ReceiveCommand(IMessageObject<string> message)    
      => message.IfInvoke<MyExpectedType>(x => x.ActionOnExpectedMessage());     
}

class MyMessenger2 : MessengerBase<string>
{
    public string MessengerIdentifier => "anyOtherName";
    
    protected override void InitializeMessages()
      => AddMessage<MyMessage>(m => ConsoleWriteLine($"intercept message {m}"));
}

class MyMessage : IMessageObject<string>
{
  string Sender {get;}
  IEnumerable<string> Receivers => new string[]{ "anyName", "anyOtherName"};
}
```

Then instantiate the Mediator, register your messenger(s), and send a MessageObject as arguement for mediator.Command.
```csharp
using heitech.MediatorMessenger.Factory;
using heitech.MediatorMessenger.Interface;

var factory = new MediatorMessengerFactory<string>();
IMediator<string> mediator = factory.CreateNewMediator();

mediator.Register(new MyMessenger());
mediator.Register(new MyMessenger2());
mediator.Command(new MyMessageObject<string>());
// same for Async, and Queries
// both the MyMessenger, and MyMessenger2 ReceiveCommand methods are called, because they are listed as receivers.

```
The benefit of the second approach is the flexibility that one gets with the possible interception at the Receiver level of the message. (see IMessenger<string> in example above, or the code of MessengerBase<string>). You could also derive the MessageObject and let them implement interceptable Behavior. The MessengerBase<T> is extremely flexible in its approach. You can override all its Receive_xx methods.
