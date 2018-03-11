namespace heitech.MediatorMessenger.Interface
{
    public interface IRegisterer<TKey>
    {
        void Register(IMessenger<TKey> messenger);

        void Unregister(TKey adress);
        void Unregister(IMessenger<TKey> messenger);

        IMediator<TKey> Mediator { get; }

        bool IsRegistered(TKey key);
        IMessenger<TKey> Get(TKey key);
    }
}
