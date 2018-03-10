using heitech.MediatorMessenger.Interface;
using System;
using System.Collections.Generic;

namespace heitech.MediatorMessenger.Tests
{
    internal class MessageObjectMock : IMessageObject<string>
    {
        public string Sender { get; }
        public IEnumerable<string> Receivers { get; }

        public MessageObjectMock(string sender, params string[] receivers)
        {
            Sender = sender;
            Receivers = receivers;
        }

        public bool Equals(IMessageObject<string> other) => other == this;
        public string ToString(string format, IFormatProvider formatProvider) => ToString();
    }
}
