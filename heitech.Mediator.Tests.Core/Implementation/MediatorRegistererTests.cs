using heitech.Mediator.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using heitech.Mediator.Interface;
using System.Threading.Tasks;

namespace heitech.Mediator.Tests.Core.Implementation
{
    [TestClass]
    public class MediatorRegistererTests
    {
        private Spy spy;
        private readonly MediatorStubs stub = new MediatorStubs();
        private readonly MediatorMock mediator = new MediatorMock();

        [TestInitialize]
        public void Init()
        {
            spy = new Spy(mediator);
            Assert.IsTrue(mediator.WasSet);
        }

        [TestMethod]
        public void MediatorRegisterer_IsRegisteredReturnsFalseIfTypeIsNotRegistered()
        {
            Assert.IsFalse(spy.IsRegistered<IMediatable_1>());
            Assert.AreEqual(0, spy.GetCount());
        }

        [TestMethod]
        public void MediatorRegisterer_IsRegisteredReturnsTrueIfTypeIsRegistered()
        {
            spy.Register(stub.Mediatable_1);
            Assert.IsTrue(spy.IsRegistered<IMediatable_1>());
            Assert.AreEqual(1, spy.GetCount());
        }

        [TestMethod]
        public void MediatorRegisterer_NeedsToSpecifyInterfaceKeyOnNonInterfaceRuntimeType()
        {
            Assert.ThrowsException<ArgumentException>(() => spy.Register(stub.NonInterfaceMediatable));

            Assert.AreEqual(0, spy.GetCount());
            spy.Register<IMediatable_1>(stub.NonInterfaceMediatable);
            Assert.AreEqual(1, spy.GetCount());
        }

        [TestMethod]
        public void MediatorRegisterer_RegistersSameTypeTwiceThrowsException()
        {
            spy.Register(stub.Mediatable_1);
            Assert.ThrowsException<TypeAlreadyRegisteredException>(() => spy.Register(stub.Mediatable_1));
        }

        [TestMethod]
        public void MediatorRegisterer_RegistersNonInterfaceThrowsException()
        {
            spy.Register(stub.Mediatable_1);

            Assert.ThrowsException<ArgumentException>(() => spy.Register("abc"));
        }

        [TestMethod]
        public void MediatorRegisterer_RegistersInterfaceAddsToRegisteredCollection()
        {
            Assert.AreEqual(0, spy.GetCount());

            spy.Register(stub.Mediatable_1);
            Assert.AreEqual(1, spy.GetCount());

            spy.Register(stub.Mediatable_2);
            Assert.AreEqual(2, spy.GetCount());
        }

        [TestMethod]
        public void MediatorRegisterer_UnregisterThrowsTypeNotRegisteredExceptionIfTypeIsNotRegistered()
            => Assert.ThrowsException<TypeNotRegisteredException>(() => spy.Unregister(stub.Mediatable_1));

        [TestMethod]
        public void MediatorRegisterer_UnregisterRemovseTypeFromCollection()
        {
            Assert.AreEqual(0, spy.GetCount());
            spy.Register(stub.Mediatable_1);
            spy.Register(stub.Mediatable_2);
            Assert.AreEqual(2, spy.GetCount());

            spy.Unregister(stub.Mediatable_1);
            Assert.AreEqual(1, spy.GetCount());
            spy.Unregister(stub.Mediatable_2);
            Assert.AreEqual(0, spy.GetCount());
        }

        [TestMethod]
        public void MediatorRegisterer_GetThrowsTypeNotRegistered()
            => Assert.ThrowsException<TypeNotRegisteredException>(() => spy.Get<IMediatable_1>());

        [TestMethod]
        public void MediatorRegisterer_GetReturnsInstanceOfRegisteredType()
        {
            spy.Register(stub.Mediatable_1);
            var instance = spy.Get<IMediatable_1>();
        }

        [TestMethod]
        public void MediatorRegisterer_GetReturnsThrowsTypeNotFoundOnMultipleInterfaceInheritance()
        {
            spy.Register<IMediatable_1>(stub.Mediatble_3);
            Assert.ThrowsException<TypeNotRegisteredException>(() => spy.Get<IMediatable_2>());
        }

        private class Spy : Mediator.Implementation.Registerer
        {
            internal Spy(IInternalMediator mediatorInstance) 
                : base(mediatorInstance)
            { }

            internal int GetCount() => Mediatables.Count;
        }

        private class MediatorMock : IInternalMediator
        {
            internal bool WasSet;
            public void SetRegister(IRegister register) => WasSet = true;

            #region
            public void Command<T>(Action<T> action)
            {
                throw new NotImplementedException();
            }

            public Task CommandAsync<T>(Func<T, Task> asyncAction)
            {
                throw new NotImplementedException();
            }

            public bool IsRegistered<T>() where T : class
            {
                throw new NotImplementedException();
            }

            public TResult Query<T, TResult>(Func<T, TResult> query)
            {
                throw new NotImplementedException();
            }

            public Task<TResult> QueryAsync<T, TResult>(Func<T, Task<TResult>> queryAsync)
            {
                throw new NotImplementedException();
            }
            #endregion  
        }
    }
}
