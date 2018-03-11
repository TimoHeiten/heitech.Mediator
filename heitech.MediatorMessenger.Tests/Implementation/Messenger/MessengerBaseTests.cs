using heitech.MediatorMessenger.Implementation.Messenger;
using heitech.MediatorMessenger.Interface;
using heitech.MediatorMessenger.Tests.Implementation.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace heitech.MediatorMessenger.Tests.Implementation.Messenger
{
    [TestClass]
    public class MessengerBaseTests
    {
        private readonly Messenger messenger = new Messenger();

        // boo reflection in unit test, boo
        [TestMethod]
        public void MessengerBase_IsNotInitializedWithFalseAsCtorInput()
        {
            var m = new Messenger(false);
            var dict = (Dictionary<Type, IMessage>)typeof(MessengerBase<string>)
                .GetField("messages", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(m);
            Assert.AreEqual(0, dict.Count);
        }

        [TestInitialize]
        public void Init()
        {
            var dict = (Dictionary<Type, IMessage>)typeof(MessengerBase<string>)
                .GetField("messages", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(messenger);
            Assert.AreEqual(1, dict.Count);
        }


        [TestMethod]
        public void MessengerBase_InitializesMessageThatIsNotAddedThrowsKeyNotFoundException()
            => Assert.ThrowsException<KeyNotFoundException>(() => messenger.ReceiveCommand(new NotThisTypeOfMessage()));

        [TestMethod]
        public void MessengerBase_InitializedMessage_IsInvoked()
        {
            messenger.ReceiveCommand(new MessageObjectMock("messenger"));
            Assert.IsTrue(messenger.MessageObjectInvoked);
        }

        [TestMethod]
        public void MessengerBase_RequestIsInvoked_IfRegistered()
        {
            int _42 = messenger.ReceiveQuery<int>(new RequestObjectMock("s", "messenger"));
            Assert.AreEqual(42, _42);
        }

        [TestMethod]
        public void MessengerBase_MultipleFuncWithSameRequestArePossible()
        {
            int _42 = messenger.ReceiveQuery<int>(new RequestObjectMock("s", "messenger"));
            Assert.AreEqual(42, _42);

            string s = messenger.ReceiveQuery<string>(new RequestObjectMock("s", "messenger"));
            Assert.AreEqual("result", s);
        }

        [TestMethod]
        public void MessengerBase_RequestThatIsNotAddedTHrowsKeyNotFOundException()
            => Assert.ThrowsException<KeyNotFoundException>(() => (int)messenger.ReceiveQuery<int>(new NotThisRequest()));

        private class Messenger : MessengerBase<string>
        {
            internal Messenger(bool initializeMessages = true) 
                : base(initializeMessages)
            { }

            public override string MessengerIdentifier => "messenger";

            internal bool MessageObjectInvoked { get; private set; }
            internal bool RequstObjectInvoked { get; private set; }
            protected override void InitializeMessages()
            {
                AddRequest<RequestObjectMock, int>(m => 42);
                AddRequest<RequestObjectMock, string>(m => "result");
                AddMessage<MessageObjectMock>(m => MessageObjectInvoked = true);
            }
        }
    }
}
