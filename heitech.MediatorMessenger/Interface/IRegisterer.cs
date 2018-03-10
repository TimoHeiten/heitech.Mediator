namespace heitech.MediatorMessenger.Interface
{
    public interface IRegisterer<TKey>
    {
        void Register(IMessenger<TKey> messenger);

        void Unregister(TKey adress);
        void Unregister(IMessenger<TKey> messenger);

        IMediatorMessenger<TKey> Mediator { get; }

        bool IsRegistered(TKey key);
    }
}
