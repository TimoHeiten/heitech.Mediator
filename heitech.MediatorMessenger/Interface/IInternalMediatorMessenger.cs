namespace heitech.MediatorMessenger.Interface
{
    internal interface IInternalMediatorMessenger<TKey> : IMediatorMessenger<TKey>
    {
        void SetRegisterer(IRegisterer<TKey> registerer);
    }
}
