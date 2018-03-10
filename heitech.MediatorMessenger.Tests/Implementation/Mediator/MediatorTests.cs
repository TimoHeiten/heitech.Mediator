using heitech.MediatorMessenger.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Tests.Implementation.Mediator
{
    [TestClass]
    public class MediatorTests
    {
        private readonly RegistererMock reg = new RegistererMock();

        private readonly MediatorMessenger.Implementation.Mediator.Mediator<string> mediator =
            new MediatorMessenger.Implementation.Mediator.Mediator<string>();

        private readonly MessageObjectMock message = new MessageObjectMock("sender", "receiver_1", "receiver_2");
        private readonly MessengerMock messenger_1 = new MessengerMock("receiver_1");
        private readonly MessengerMock messenger_2 = new MessengerMock("receiver_2");
        //MediatorMessenger_

        [TestInitialize]
        public void Init()
        {
            reg.Messengers.Add(messenger_1);
            reg.Messengers.Add(messenger_2);
            mediator.SetRegisterer(reg);
        }

        [TestMethod]
        public void MediatorMessenger_CommandSendsMessageToAllReceivers()
        {
            mediator.Command(message);
            reg.PredicateRegistered = key => key.Contains("receiver");
            Assert.AreEqual(2, reg.CountGetterCalls);
        }

        [TestMethod]
        public void MediatorMessenger_ThrowsExceptionOnCommandIfReceiverIsNotRegistered()
        {
            reg.PredicateRegistered = key => key == "receiver_1";
            Assert.ThrowsException<MessengerIdentifierNotRegisteredException>(() => mediator.Command(message));
        }

        [TestMethod]
        public async Task MediatorMessenger_ThrowsExceptionOnAsyncCommandIfNotRegistered()
        {
            reg.PredicateRegistered = key => key == "receiver_1";
            await Assert.ThrowsExceptionAsync<MessengerIdentifierNotRegisteredException>(() => mediator.CommandAsync(message));
        }

        [TestMethod]
        public async Task MediatorMessenger_RunsCommandAsyncInParallel()
        {
            await Task.Run(() => Assert.Fail());
        }

        [TestMethod]
        public async Task MediatorMessenger_CommandAsyncCallsToAllMessengers()
        {
            await Task.Run(() => Assert.Fail());
        }
    }
}
