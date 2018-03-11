using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Interface
{
    public interface IMediator<TKey>
    {
        void Command(IMessageObject<TKey> message);
        Task CommandAsync(IMessageObject<TKey> message);

        TResponse Query<TResponse>(IRequestObject<TKey> request);
        Task<TResponse> QueryAsync<TResponse>(IRequestObject<TKey> request);
    }
}
