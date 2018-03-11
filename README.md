# heitech.Mediator

Mediator Design pattern with two flavors:
- A simple one which allows to call interface methods of registered interfaces.
- A complex one that deals with Messageobjects that each registered Type can intercept and use in its on right. Registered Types need to implement the IMessenger interface.

## Why would you want this?
With either of these implementations you encapsualte the method calls/object interactions of multiple registered items. The simple one helps with prototyping or small projects, and the complex one lets you intercept object interactions in different and powerful ways.

The best part is you can decouple two or more assemblies and make them only dependent on the assembly that uses the Mediator (and in case of the complex one implements all messages/interceptors)

## Use of simple Mediator:

```csharp
using heitech.Mediator.Factory;
using heitech.Mediator.Interface;

...
  var factory = new MediatorFactory();
  IRegister register = factory.CreateNewMediator();
  register.Register<IMyInterface>(new MyImplementation());

  IMediator mediator = register.Mediator;

  mediator.Command<IMyInterface>(m_interface => m_interface.MyAction());
  mediator.CommandAsync<IMyInterface>(m_interface => m_interface.MyActionAsync()).Wait();

  TResult result = mediator.Query<IMyInterface, TResult>(m_interface => m_interface.MyFunc<TResult>());
  result = mediator.QueryAsync<IMyInterface, Task<TResult>>(m_interface => m_interface.MyFuncAsync<TResult>()).Result();
...             

```
Exceptions during registration / resolving 
- TypeAlreadyRegisteredException 
- TypeNotRegisteredException
- ArgumentException


## Use of complex Mediator:
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
