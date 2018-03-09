using heitech.Mediator.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace heitech.Mediator.Tests.Core.Implementation
{
    [TestClass]
    public class MediatorTests
    {
        readonly RegisterMock mock = new RegisterMock();
        readonly MediatorStubs stubs = new MediatorStubs();

        readonly Mediator.Implementation.Mediator mediator = new Mediator.Implementation.Mediator();

        [TestInitialize]
        public void Init() => mediator.SetRegister(mock);


        [TestMethod]
        public void Mediator_DelegatesIsRegisteredToRegisterer()
        {
            Assert.IsFalse(mediator.IsRegistered<IMediatable_1>());
            mock._IsRegistered = true;
            Assert.IsTrue(mediator.IsRegistered<IMediatable_1>());
        }

        [TestMethod]
        public void Mediator_CommandInvokesActionOnSpecifiedKey()
        {
            var mediatable = SetMockResult<Mediatable_uno>();
            mediator.Command<IMediatable_1>(m => m.Action());

            Assert.IsTrue((mock.Result as Mediatable_uno).WasInvoked);
        }

        [TestMethod]
        public async Task Mediator_AsyncCommandInvokesActionWithTaskOnSpecifiedKey()
        {
            var mediatable = SetMockResult<Mediatable_uno>();
            await mediator.CommandAsync<IMediatable_1>(m => m.AsyncAction());

            Assert.IsTrue(mediatable.WasInvokedAsync);
        }

        [TestMethod]
        public void Mediator_CommandWithParamInvokesAction()
        {
            var mediatable = SetMockResult<Mediatable_uno>();
            mediator.Command<IMediatable_1>(m => m.ActionWithParam("abcaffeschnee"));

            Assert.IsTrue(mediatable.WasInvoked);
            Assert.AreEqual("abcaffeschnee", mediatable.Input);
        }

        [TestMethod]
        public void Mediator_QueryReturnsResultFromInterfaceInvokedFunc()
        {
            var m = SetMockResult<Mediatable_dos>();
            m.Result = "abc";
            string result = mediator.Query<IMediatable_2, string>(x => x.Func<string>());

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public async Task Mediator_QueryAsyncReturnsResultFromAsyncMethod()
        {
            var med = SetMockResult<Mediatable_dos>();
            med.Result = "abc";
            string result = await mediator.QueryAsync<IMediatable_2, string>(async x => await x.FuncAsync<string>());

            Assert.AreEqual("abc", result);
        }


        private T SetMockResult<T>()
        {
            Type t = typeof(T);
            object result = null;
            if (typeof(IMediatable_1).IsAssignableFrom(t))
                result = stubs.Mediatable_1;
            else if (typeof(IMediatable_2).IsAssignableFrom(t))
                result = stubs.Mediatable_2;

            mock.Result = result;
            return (T)result;
        }


        private class RegisterMock : IRegister
        {
            public IMediator Mediator => throw new NotImplementedException();

            internal object Result { get; set; }
            internal bool _IsRegistered { get; set; }
            internal bool WasRegistered { get; private set; }
            internal Type RegisteredType { get; private set; }

            internal bool WasUnregistered { get; private set; }
            internal Type UnRegisteredType { get; private set; }

            public T Get<T>() => (T)Result;
            public bool IsRegistered<T>() where T : class => _IsRegistered;

            public void Register<T>(T mediatable) where T : class
            {
                WasRegistered = true;
                RegisteredType = typeof(T);
            }

            public void Unregister<T>(T mediatable) where T : class
            {
                WasUnregistered = true;
                UnRegisteredType = typeof(T);
            }
        }
    }
}
