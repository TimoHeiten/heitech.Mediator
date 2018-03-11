using System;

namespace heitech.MediatorMessenger.Interface
{
    internal interface IRequestResultContainer
    {
        bool HasRequestedTypeAndExpectedResult(Type req, Type exp);
    }
}
