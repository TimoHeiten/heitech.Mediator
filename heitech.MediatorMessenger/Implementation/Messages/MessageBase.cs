using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;

namespace heitech.MediatorMessenger.Implementation.Messages
{
    public abstract class MessageBase<TKey> : IMessageObject<TKey>
    {
        public TKey Sender { get; }
        public IEnumerable<TKey> Receivers { get; }

        protected MessageBase(TKey sender, params TKey[] receivers)
        {
            Sender = sender;
            Receivers = receivers;
        }

        public virtual bool Equals(IMessageObject<TKey> other)
            => Receivers.Equals(other.Receivers) && Sender.Equals(other.Sender);

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider is ICustomFormatter custom)
               return custom.Format(format, null, formatProvider);

            if (format == "r") return "MessageObject with Receivers: " + TostringReceivers();
            else if (format == "s") return $"MessageObject with Sender: " + ToStringSender();
            else if (format == "a") return $"Messageobject with Sender: {ToStringSender()} & Receivers: {TostringReceivers()}";
            else return ToString();
        }

        private string TostringReceivers() => string.Join(",", Receivers);
        private string ToStringSender() => Sender.ToString();
    }
}
