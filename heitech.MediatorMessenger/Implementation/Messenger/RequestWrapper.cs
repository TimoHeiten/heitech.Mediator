using heitech.MediatorMessenger.Interface;
using System;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Implementation.Messenger
{
    internal class RequestWrapper<TKey, TRequestObject, TResult> : IRequest
       where TRequestObject : class, IRequestObject<TKey>
    {
        private Func<TRequestObject, TResult> func;
        private Func<TRequestObject, Task<TResult>> asyncFunc;
        internal RequestWrapper<TKey, TRequestObject, TResult> Init(Func<TRequestObject, TResult> func)
        {
            this.func = func;
            return this;
        }

        internal RequestWrapper<TKey, TRequestObject, TResult> InitAsync(Func<TRequestObject, Task<TResult>> func)
        {
            asyncFunc = func;
            return this;
        }

        public object InvokeRequest(object request)
        {
            if (func == null)
                throw new InvalidOperationException("invoke sync was not initialized");

            TRequestObject _request = ValidateRequestObejct(request);
            return func.Invoke(_request);
        }

        public async Task<object> InvokeRequestAsync(object request)
        {
            if (asyncFunc == null)
                throw new InvalidOperationException("invoke async was not initialized");

            TRequestObject _request = ValidateRequestObejct(request);
            return await asyncFunc.Invoke(_request);
        }

        private TRequestObject ValidateRequestObejct(object request)
        {
            if (request is TRequestObject param)
                return param;
            else
                throw new ArgumentException($"{typeof(TRequestObject)} was expected for Message, but type {request.GetType()} was injected");
        }
    }
}

