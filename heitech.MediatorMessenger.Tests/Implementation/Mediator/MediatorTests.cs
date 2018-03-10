using heitech.MediatorMessenger.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Tests.Implementation.Mediator
{
    [TestClass]
    public class MediatorTests
    {
        private readonly RegistererMock reg = new RegistererMock();

        private readonly MediatorMessenger.Implementation.Mediator.Mediator<string> mediator =
            new MediatorMessenger.Implementation.Mediator.Mediator<string>();
        private readonly RequestObjectMock request = new RequestObjectMock("sender", "receiver_1");
        private readonly MessageObjectMock message = new MessageObjectMock("sender", "receiver_1", "receiver_2");

        private readonly MessengerMock messenger_1 = new MessengerMock("receiver_1");
        private readonly MessengerMock messenger_2 = new MessengerMock("receiver_2");

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
            Assert.IsTrue(messenger_1.ReceivedCommand);
            Assert.IsTrue(messenger_2.ReceivedCommand);
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
        public async Task MediatorMessenger_CommandAsyncCallsToAllMessengers()
        {
            await mediator.CommandAsync(message);
            reg.PredicateRegistered = key => key.Contains("receiver");
            Assert.AreEqual(2, reg.CountGetterCalls);
            Assert.IsTrue(messenger_1.ReceivedAsyncCommand);
            Assert.IsTrue(messenger_2.ReceivedAsyncCommand);
        }


        //########## Queries #############
        [TestMethod]
        public void MediatorMessenger_QueryThrowsExceptionIfMessengerNotRegistered()
        {
            reg.PredicateRegistered = key => false;
            Assert.ThrowsException<MessengerIdentifierNotRegisteredException>(() => mediator.Query<string>(request));
        }

        [TestMethod]
        public void MediatorMessenger_QueryInvokesOnRequestedItemAndReturnsResponse()
        {
            messenger_1.Result = "result";
            string result = mediator.Query<string>(request);

            Assert.AreEqual("result", result);
            Assert.IsTrue(messenger_1.ReceivedFunc);
            Assert.IsFalse(messenger_2.ReceivedFunc);
        }

        [TestMethod]
        public void MediatorMessenger_QueryThrowsUnexpectedTypeException_IfResponseTypeDoesNotMatchExpectedType()
        {
            messenger_1.Result = 42;
            Assert.ThrowsException<UnexpectedResponseTypeException>(() => mediator.Query<string>(request));
        }

        [TestMethod]
        public void MediatorMessenger_QueryReturnsNullThrowsArgumentException()
        {
            messenger_1.Result = null;
            Assert.ThrowsException<ArgumentException>(() => mediator.Query<string>(request));
        }

        [TestMethod]
        public void MediatorMessenger_ExpectedResultWorksDownCastAndUpcast()
        {
            messenger_1.Result = new NextExpected();

            mediator.Query<IExpectedResponse>(request);
            Assert.IsTrue(messenger_1.ReceivedFunc);
            Assert.IsFalse(messenger_2.ReceivedFunc);

            mediator.Query<ExpectedResponse>(request);
        }

        [TestMethod]
        public async Task MediatorMessenger_QueryAsyncThrowsExceptionIfMEssengerNotRegistered()
        {
            reg.PredicateRegistered = key => false;

            await Assert.ThrowsExceptionAsync<MessengerIdentifierNotRegisteredException>(() => mediator.QueryAsync<string>(request));
        }

        [TestMethod]
        public async Task MediatorMessenger_QueryAsyncReturnsExpectedResult()
        {
            messenger_1.Result = "abc";
            await mediator.QueryAsync<string>(request);
            
            Assert.IsTrue(messenger_1.ReceivedFuncAsync);
            Assert.IsFalse(messenger_2.ReceivedFuncAsync);
        }

        [TestMethod]
        public async Task MediatorMessenger_QueryAsyncThrowsUnexpectedTypeExceptionOnNotMatchingReturnType()
        {
            messenger_1.Result = 42;
            await Assert.ThrowsExceptionAsync<UnexpectedResponseTypeException>(() => mediator.QueryAsync<string>(request));
        }

        [TestMethod]
        public async Task MediatorMessenger_QueryAsyncResponseIsNullThrowsArgumentException()
        {
            messenger_1.Result = null;
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => mediator.QueryAsync<string>(request));
        }

        [TestMethod]
        public async Task MediatorMessenger_QueryAsyncDownAndUpcastWork()
        {
            messenger_1.Result = new NextExpected();
            await mediator.QueryAsync<IExpectedResponse>(request);
            await mediator.QueryAsync<ExpectedResponse>(request);

            Assert.IsTrue(messenger_1.ReceivedFuncAsync);
            Assert.IsFalse(messenger_2.ReceivedFuncAsync);
        }


        [TestMethod]
        public async Task AllMediatorInvocablesThrowArgumentExceptionOnNullArgument()
        {
            Assert.ThrowsException<ArgumentException>(() => mediator.Command(null));
            Assert.ThrowsException<ArgumentException>(() => mediator.Query<string>(null));
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => mediator.CommandAsync(null));
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => mediator.QueryAsync<string>(null));
        }

        private interface IExpectedResponse { }
        private class NextExpected : ExpectedResponse { }
        private class ExpectedResponse : IExpectedResponse { }
    }
}
