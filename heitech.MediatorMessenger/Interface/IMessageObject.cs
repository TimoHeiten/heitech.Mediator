using System;
using System.Collections.Generic;

namespace heitech.MediatorMessenger.Interface
{
    public interface IMessageObject<TKey> : IFormattable, IEquatable<IMessageObject<TKey>>
    {
        TKey Sender { get; }
        IEnumerable<TKey> Receivers { get; }
    }
}
