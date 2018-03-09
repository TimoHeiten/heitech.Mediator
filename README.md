# heitech.Mediator

Mediator Design pattern with two flavors:
- A simple one which allows to call interface methods of registered interfaces.
- A complex one that deals with Messageobjects that each registered Type can intercept and use in its on right. Registered Types need to implement the IMessenger interface.

## Use of simple Mediator:

```csharp
using heitech.Mediator.Factory;
using heitech.Mediator.Interface;

...
  var factory = new MediatorFactory();
  IRegister register = fac.CreateNewMediator();
  register.Register<IMyInterface>();

  IMediator mediator = register.Mediator;

  mediator.Command<IMyInterface>(m_interface => m_interface.MyAction());
  mediator.CommandAsync<IMyInterface>(m_interface => m_interface.MyActionAsync()).Wait();

  mediator.Query<IMyInterface, TResult>(m_interface => m_interface.MyFunc<TResult>());
  mediator.QueryAsync<IMyInterface, Task<TResult>>(m_interface => m_interface.MyFuncAsync<TResult>());
...             

```
Exceptions during registration / resolving 
- TypeAlreadyRegisteredException 
- TypeNotRegisteredException
- ArgumentException


## Use of complex Mediator:
(implementation is todo, only scaffolding ready)

The complex Mediator uses a generic Key to let you decide how you want to reference it.
First create a IMessenger that handles all ReceiveCommand, Receivequery etc. on incoming Message/RequestObjects
Implement your own IMessageobject or derive from abstract baseclass MessageBase. (analog for RequestObject) 
```csharp
class MyMessenger : IMessenger<string>
{
    string Identifier{get;}
    public void ReceiveCommand(IMessageObject<string> message)
    {
      message.IfInvoke<MyExpectedType>(x => x.ActionOnExpectedMessage());
      ...
    }
}

class MyMessage : IMessageObject<string>
{
  Type Sender {get;}
  IEnumerable<TKey> Receiver {get;}
}
```

Then instantiate the Mediator, register your messenger and create a MessageObject.
```csharp
using heitech.MediatorMessenger.Factory;
using heitech.MediatorMessenger.Interface;

var factory = new MediatorMessengerFactory();
IMediatorMessenger<string> mediator = factory.Get<string>();

mediator.Register(new MyMessenger());
mediator.Command(new MyMessageObject<string>());
// same for Async, and Queries
```
The benefit of the second approach is the flexibility that one gets with the possible interception at the Receiver level of the message. In future releases, there will be a 
