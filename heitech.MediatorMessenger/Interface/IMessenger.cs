using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Interface
{
    public interface IMessenger<TKey>
    {
        TKey MessengerIdentifier { get; }
        void ReceiveCommand(IMessageObject<TKey> message);
        TResult ReceiveQuery<TResult>(IRequestObject<TKey> request);

        Task ReceiveCommandAsync(IMessageObject<TKey> message);
        Task<TResult> ReceiveQueryAsync<TResult>(IRequestObject<TKey> request);
    }
}
