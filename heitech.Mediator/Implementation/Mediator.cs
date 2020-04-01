using heitech.Mediator.Interface;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("heitech.Mediator.Tests.Core")]

namespace heitech.Mediator.Implementation
{
    ///<summary>
    /// Mediator implementation to handle commands and queries. Make Interfaces widely available as a sort of Facade
    ///</summary>
    internal class Mediator : IInternalMediator
    {
        private IRegister register;
        public void SetRegister(IRegister register)
            => this.register = register;

        public bool IsRegistered<T>() where T : class
            => register.IsRegistered<T>();

        public void Command<T>(Action<T> action) 
            => action(Get<T>());

        public Task CommandAsync<T>(Func<T, Task> asyncAction) 
            => asyncAction(Get<T>());

        public TResult Query<T, TResult>(Func<T, TResult> query) 
            => query(Get<T>());

        public Task<TResult> QueryAsync<T, TResult>(Func<T, Task<TResult>> queryAsync)
            => queryAsync(Get<T>());

        private T Get<T>() 
            => register.Get<T>();
    }
}
