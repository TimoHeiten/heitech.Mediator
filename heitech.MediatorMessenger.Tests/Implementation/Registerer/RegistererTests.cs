using heitech.MediatorMessenger.Exceptions;
using heitech.MediatorMessenger.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace heitech.MediatorMessenger.Tests.Implementation.Registerer
{
    [TestClass]
    public class RegistererTests
    {
        private readonly MediatorMock mediator = new MediatorMock();
        private readonly MessengerMock messenger = new MessengerMock();
        private Spy registerer;

        [TestInitialize]
        public void Init()
        {
            registerer = new Spy(mediator);
            Assert.IsTrue(mediator.WasRegistererSet);
        }


        [TestMethod]
        public void MessengerRegisterer_RegisterMessengerRegistersExactMessengerWithKey()
        {
            Assert.AreEqual(0, registerer.Count);

            RegisterMock();

            Assert.AreEqual(1, registerer.Count);
        }

        [TestMethod]
        public void MessengerRegisterer_RegisterSameMessengerIdentifierTwiceThrowsException()
        {
            RegisterMock();
            Assert.ThrowsException<MessengerIdentifierAlreadyRegisteredException>(() => registerer.Register(messenger));
        }

        [TestMethod]
        public void MessengerRegisterer_UnregisterRemovesRegisteredMessenger()
        {
            Assert.AreEqual(0, registerer.Count);
            RegisterMock();
            Assert.AreEqual(1, registerer.Count);

            registerer.Unregister(messenger);
            Assert.AreEqual(0, registerer.Count);
        }

        private void RegisterMock() => registerer.Register(messenger);

        [TestMethod]
        public void MessengerRegisterer_UnregisterForNotRegisteredMessengerThrowsException()
            => Assert.ThrowsException<MessengerIdentifierNotRegisteredException>(() => registerer.Unregister(messenger));

        [TestMethod]
        public void MessengerRegisterer_UnregisterByKeyRemovesMessenger()
        {
            RegisterMock();
            registerer.Unregister(messenger.MessengerIdentifier);
            Assert.AreEqual(0, registerer.Count);
        }

        [TestMethod]
        public void MessengerRegisterer_UnregisterByKeyThrowsExceptionOnNotRegisteredMessenger()
            => Assert.ThrowsException<MessengerIdentifierNotRegisteredException>(() => registerer.Unregister(messenger.MessengerIdentifier));

        [TestMethod]
        public void MessengerRegisterer_IsRegisteredReturnsFalseIfKeyIsNotRegsitered()
            => Assert.IsFalse(registerer.IsRegistered("Mock"));

        [TestMethod]
        public void MessengerRegisterer_IsRegisteredReturnsTrueIfKeyIsRegistered()
        {
            RegisterMock();

            Assert.IsTrue(registerer.IsRegistered("Mock"));
        }


        private class Spy : MediatorMessenger.Implementation.Registerer.Registerer<string>
        {
            internal Spy(IInternalMediatorMessenger<string> mediator) : base(mediator)
            { }

            internal int Count => Messengers.Count;
        }
    }
}
