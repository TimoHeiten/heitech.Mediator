using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Interface
{
    internal interface IRequest
    {
        object InvokeRequest(object request);
        Task<object> InvokeRequestAsync(object request);
    }
}
