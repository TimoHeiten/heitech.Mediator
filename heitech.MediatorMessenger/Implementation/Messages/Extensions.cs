using heitech.MediatorMessenger.Interface;
using System;

namespace heitech.MediatorMessenger.Implementation.Messages
{
    public static class Extensions
    {
        public static void IfInvoke<TKey, ExpectedMessageType>(this IMessageObject<TKey> message, Action action)
            where ExpectedMessageType : IMessageObject<TKey>
        {
            message.IfInvoke(typeof(ExpectedMessageType), action);
        }

        public static void IfInvoke<TKey>(this IMessageObject<TKey> message, Type exptecedMessageType, Action action)
        {
            Type msgType = message.GetType();
            ThrowOnNotAssignable(typeof(IMessageObject<TKey>), exptecedMessageType);

            if (msgType == exptecedMessageType)
            {
                    action();
            }
        }

        public static bool TryInvokeRequest<TKey, TResult, TExpectedRequest>(this IRequestObject<TKey> requestObject, Func<TResult> func, out TResult result)
            where TExpectedRequest : IRequestObject<TKey>
        {
            return requestObject.TryInvokeRequest(typeof(TExpectedRequest), func, out result);
        }

        public static bool TryInvokeRequest<TKey, TResult>(this IRequestObject<TKey> requestObject, Type expectedRequest, Func<TResult> func, out TResult result)
        {
            ThrowOnNotAssignable(typeof(IRequestObject<TKey>), expectedRequest);

            bool isSuccess = false;
            result = default(TResult);
            Type rqtType = requestObject.GetType();

            if (rqtType == expectedRequest)
            {
                result = func();
                isSuccess = true;
            }

            return isSuccess;
        }

        private static void ThrowOnNotAssignable(Type assignableFrom, Type expected)
        {
            if (!assignableFrom.IsAssignableFrom(expected))
                throw new ArgumentException($"given Type to test must be assignable from {assignableFrom.Name}");
        }
    }
}
