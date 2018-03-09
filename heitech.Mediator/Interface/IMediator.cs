using System;
using System.Threading.Tasks;

namespace heitech.Mediator.Interface
{
    public interface IMediator
    {
        bool IsRegistered<T>() where T : class;

        void Command<T>(Action<T> action);
        Task CommandAsync<T>(Func<T, Task> asyncAction);

        TResult Query<T, TResult>(Func<T, TResult> query);
        Task<TResult> QueryAsync<T, TResult>(Func<T, Task<TResult>> queryAsync);
    }
}
