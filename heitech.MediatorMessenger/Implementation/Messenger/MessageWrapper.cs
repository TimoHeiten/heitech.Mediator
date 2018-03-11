using heitech.MediatorMessenger.Interface;
using System;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Implementation.Messenger
{
    internal class MessageWrapper<TParameter> : IMessage
         where TParameter : class
    {
        private Action<TParameter> paramAction;
        private Func<TParameter, Task> actionAsync;
        internal MessageWrapper<TParameter> Init(Action<TParameter> action)
        {
            this.paramAction = action;
            return this;
        }

        internal MessageWrapper<TParameter> InitAsync(Func<TParameter, Task> actionAsnc)
        {
            this.actionAsync = actionAsnc;
            return this;
        }

        public void Invoke(object o)
        {
            if (paramAction == null)
                throw new InvalidOperationException($"the message was registered for async use : {GetType()}");

            TParameter param = ValidateParam(o);
            paramAction.Invoke(o as TParameter);
        }

        public async Task InvokeAsync(object o)
        {
            if (actionAsync == null)
                throw new InvalidOperationException($"the message was not registered for async use : {GetType()}");

            TParameter param = ValidateParam(o);
            await actionAsync.Invoke(param);
        }


        private TParameter ValidateParam(object o)
        {
            if (o is TParameter param)
                return param;
            else
                throw new ArgumentException($"{typeof(TParameter)} was expected for Message, but type {o.GetType()} was injected");
        }
    }
}
