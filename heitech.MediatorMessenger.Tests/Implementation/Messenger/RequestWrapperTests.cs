using heitech.MediatorMessenger.Implementation.Messenger;
using heitech.MediatorMessenger.Tests.Implementation.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Tests.Implementation.Messenger
{
    [TestClass]
    public class RequestWrapperTests
    {
        private RequestWrapper<string, RequestObjectMock, int> wrapper = new RequestWrapper<string, RequestObjectMock, int>();
        private readonly RequestObjectMock request = new RequestObjectMock("s", "r");
        private readonly NotThisRequest notThisRequest = new NotThisRequest();

        [TestMethod]
        public void RequestWrapper_InitAsync_InvokeSyncThrowsInvalidOperationException()
        {
            wrapper.InitAsync(mock => Task.FromResult(42));
            Assert.ThrowsException<InvalidOperationException>(() => wrapper.InvokeRequest(request));
        }

        [TestMethod]
        public async Task RequestWrapper_InitSync_InvokeAsyncThrowsInvalidOperationException()
        {
            wrapper.Init(mock => 42);
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => wrapper.InvokeRequestAsync(request));
        }

        [TestMethod]
        public void RequestWrapper_WrongRequestObjectThrowsArgumentExceptionOnInvokeSync()
        {
            wrapper.Init(mock => 42);
            Assert.ThrowsException<ArgumentException>(() => wrapper.InvokeRequest(notThisRequest));
        }

        [TestMethod]
        public async Task RequestWrapper_WrongRequestObjectThrowsArgumentExceptionOnInvokeAsync()
        {
            wrapper.InitAsync(mock => Task.FromResult(42));
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => wrapper.InvokeRequestAsync(notThisRequest));
        }

        [TestMethod]
        public void Request_WrapperExecutesOnCorrectInit()
        {
            wrapper.Init(mock => 42);
            object result = wrapper.InvokeRequest(request);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public async Task Request_WrapperExecutesAsyncOnCorrectInitAsync()
        {
            wrapper.InitAsync(mock => Task.FromResult(42));
            object result = await wrapper.InvokeRequestAsync(request);
            Assert.AreEqual(42, result);
        }
    }
}
