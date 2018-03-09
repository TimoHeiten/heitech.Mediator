using System;
using System.Collections.Generic;
using System.Text;

namespace heitech.MediatorMessenger.Interface
{
    public interface IRegisterer<TKey>
    {
        bool Register(IMessenger<TKey> messenger);
        bool Unregister(IMessenger<TKey> messenger);
        bool Unregister(TKey adress);
    }
}
