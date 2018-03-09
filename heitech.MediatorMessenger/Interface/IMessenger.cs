using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Interface
{
    public interface IMessenger<TKey>
    {
        TKey MessengerIdentifier { get; }
        void ReceiveCommand(IMessageObject<TKey> message);
        object ReceiveQuery(IRequestObject<TKey> request);

        Task ReceiveCommandAsync(IMessageObject<TKey> message);
        Task<object> ReceiveQueryAsync(IRequestObject<TKey> request);

        IMessenger<TKey> Initialize();
    }
}
