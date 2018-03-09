using System.Threading.Tasks;

namespace heitech.Mediator.Tests.Core.Implementation
{
    internal class MediatorStubs
    {
        internal IMediatable_1 Mediatable_1 { get; } = new Mediatable_uno();
        internal IMediatable_2 Mediatable_2 { get; } = new Mediatable_dos();

        internal Mediatable_trees Mediatble_3 { get; } = new Mediatable_trees();

        internal Mediatable_uno NonInterfaceMediatable { get; } = new Mediatable_uno();
    }

    internal interface IMediatable_1
    {
        void Action();
        Task AsyncAction();

        void ActionWithParam(object input);
    }

    internal interface IMediatable_2
    {
        TResult Func<TResult>();
        Task<TResult> FuncAsync<TResult>();
    }

    internal class Mediatable_uno : IMediatable_1
    {
        internal bool WasInvoked;
        internal object Input;

        public void Action() => WasInvoked = true;
        public void ActionWithParam(object input)
        {
            WasInvoked = true;
            Input = input;
        }

        internal bool WasInvokedAsync;
        public Task AsyncAction()
        {
            WasInvokedAsync = true;
            return Task.CompletedTask;
        }
    }
    internal class Mediatable_dos : IMediatable_2
    {
        internal object Result;
        public TResult Func<TResult>() => (TResult)Result;
        public Task<TResult> FuncAsync<TResult>() => Task.FromResult((TResult)Result);
    }

    internal class Mediatable_trees : IMediatable_1, IMediatable_2
    {
        public void Action() => throw new System.NotImplementedException();
        public Task AsyncAction() => throw new System.NotImplementedException();
        public TResult Func<TResult>() => throw new System.NotImplementedException();
        public void ActionWithParam(object input) => throw new System.NotImplementedException();
        public Task<TResult> FuncAsync<TResult>() => throw new System.NotImplementedException();
    }
}
