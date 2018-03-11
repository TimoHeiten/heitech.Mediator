using heitech.MediatorMessenger.Implementation.Messages;
using heitech.MediatorMessenger.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace heitech.MediatorMessenger.Tests.Implementation.Messages
{
    [TestClass]
    public class ExtensionTests
    {
        private readonly RequestObjectMock request = new RequestObjectMock("sender", "r");
        private readonly MessageObjectMock mockMessage = new MessageObjectMock("sender", "r1", "r2");

        [TestMethod]
        public void MessageExtensions_IfInvoke_InvokesActionIfMessageTypeMatchesExcpectedType()
        {
            bool val = false;
            Action action = () => val = true;
            mockMessage.IfInvoke(typeof(MessageObjectMock), action);

            Assert.IsTrue(val);
        }

        [TestMethod]
        public void MessageExtensions_IfInvoke_DoesNotInvokeActionIfMessageTypeDoesNotExpectedType()
        {
            bool val = false;
            Action action = () => val = true;
            mockMessage.IfInvoke(typeof(NotThisTypeOfMessage), action);

            Assert.IsFalse(val);
        }

        [TestMethod]
        public void MessageExtensions_IfInvokeThrowsArgumentExceptionIfMessageTypeIsNotAssignableFromIMessageObject()
            => Assert.ThrowsException<ArgumentException>(() => mockMessage.IfInvoke(typeof(string), () => { }));

        [TestMethod]
        public void MessageExtensions_RequestTryInvokeReturnsFalseIfTypeIsNotRequestType()
        {
            bool success = request.TryInvokeRequest(typeof(NotThisRequest), () => 42, out int i);

            Assert.IsFalse(success);
            Assert.AreEqual(0, i);
        }

        [TestMethod]
        public void MessageExtensions_TryInvokeRequestReturnsTrueOnCorrectTypeAndInvokesFunction()
        {
            bool success = request.TryInvokeRequest(typeof(RequestObjectMock), () => 42, out int i);

            Assert.IsTrue(success);
            Assert.AreEqual(42, i);
        }

        [TestMethod]
        public void MessageExtensions_RequstTryInvokeThrowsArgumentExceptionIfNotAssignable()
            => Assert.ThrowsException<ArgumentException>(() => request.TryInvokeRequest(typeof(string), () => 42, out int i));
    }

    internal class NotThisRequest : IRequestObject<string>
    {
        public string Sender => throw new NotImplementedException();
        public string Receiver => throw new NotImplementedException();
        public Type RequestedType => throw new NotImplementedException();
    }

    internal class NotThisTypeOfMessage : IMessageObject<string>
    {
        public string Sender => throw new NotImplementedException();

        public IEnumerable<string> Receivers => throw new NotImplementedException();

        public bool Equals(IMessageObject<string> other)
        {
            throw new NotImplementedException();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
