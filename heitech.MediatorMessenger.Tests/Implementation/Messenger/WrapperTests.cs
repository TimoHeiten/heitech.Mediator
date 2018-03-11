using heitech.MediatorMessenger.Implementation.Messenger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace heitech.MediatorMessenger.Tests.Implementation.Messenger
{
    [TestClass]
    public class WrapperTests
    {
        private readonly MessageWrapper<string> wrapper = new MessageWrapper<string>();

        [TestMethod]
        public void Wrapper_IsInitAsyncThrowsInvalidOperationExceptionOnInvoke()
        {
            wrapper.InitAsync(m => Task.CompletedTask);
            Assert.ThrowsException<InvalidOperationException>(() => wrapper.Invoke("abc"));
        }

        [TestMethod]
        public void Wrapper_InvokesMethodIfCorrectInitialized()
        {
            bool wasInvoked = false;
            wrapper.Init(m => wasInvoked = true);
            wrapper.Invoke("abc");

            Assert.IsTrue(wasInvoked);
        }

        [TestMethod]
        public void Wrapper_ThrowsArgumentExceptionOnInvokeIfTypeDoesNotMatchRegisteredParam()
        {
            wrapper.Init(m => { });
            Assert.ThrowsException<ArgumentException>(() => wrapper.Invoke(42));
        }

        [TestMethod]
        public async Task Wrapper_IsInitThrowsInvalidOperationExceptionOnInvokeAsync()
        {
            wrapper.Init(m => { });
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => wrapper.InvokeAsync("abc"));
        }

        [TestMethod]
        public async Task Wrapper_InvokesAsyncIfCorrectInitialized()
        {
            bool wasInvoked = false;
            wrapper.InitAsync(m => { wasInvoked = true; return Task.CompletedTask; });
            await wrapper.InvokeAsync("abc");

            Assert.IsTrue(wasInvoked);
        }

        [TestMethod]
        public async Task Wrapper_ThrowsArgumentExceptionnOnInvokeAsyncIfTypeDoesNotMatchRegisteredParam()
        {
            wrapper.InitAsync(m => Task.CompletedTask);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => wrapper.InvokeAsync(42));
        }
    }
}
