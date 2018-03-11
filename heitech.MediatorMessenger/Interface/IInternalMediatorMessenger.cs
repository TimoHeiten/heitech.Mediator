namespace heitech.MediatorMessenger.Interface
{
    internal interface IInternalMediatorMessenger<TKey> : IMediator<TKey>
    {
        void SetRegisterer(IRegisterer<TKey> registerer);
    }
}
