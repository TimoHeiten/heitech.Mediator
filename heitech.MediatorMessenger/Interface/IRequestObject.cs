﻿namespace heitech.MediatorMessenger.Interface
{
    public interface IRequestObject<TKey>
    {
        TKey Sender { get; }
        TKey Receiver { get; }
    }
}
