using System;
using System.Collections.Generic;

namespace heitech.MediatorMessenger.Interface
{
    ///<summary>
    /// Derive from this to make a message eligible for the mediator
    ///</summary>
    public interface IMessageObject<TKey> : IFormattable, IEquatable<IMessageObject<TKey>>
    {
        TKey Sender { get; }
        IEnumerable<TKey> Receivers { get; }
    }
}
