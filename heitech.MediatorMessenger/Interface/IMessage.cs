using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Interface
{
    internal interface IMessage
    {
        void Invoke(object o);
        Task InvokeAsync(object o);
    }
}
