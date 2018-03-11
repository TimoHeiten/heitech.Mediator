using heitech.MediatorMessenger.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace heitech.MediatorMessenger.Tests.Factory
{
    [TestClass]
    public class MediatorMessengerFactoryTests
    {
        private MediatorMessenger.Factory.MediatorMessengerFactory<string> factory = new MediatorMessenger.Factory.MediatorMessengerFactory<string>();

        [TestMethod]
        public void MediatorFactory_ReturnsIRegisterInterface()
        {
            var mediator = factory.CreateSingletonMediator();

            Assert.IsTrue(typeof(IRegisterer<string>).IsAssignableFrom(mediator.GetType()));
        }

        [TestMethod]
        public void MediatorFactory_ReturnsSameObjectOnCreate()
        {
            var mediator = factory.CreateSingletonMediator();
            var mediator2ndInstance = factory.CreateSingletonMediator();

            Assert.AreSame(mediator, mediator2ndInstance);
        }

        [TestMethod]
        public void MediatorFactory_RegisterMediatorNotNull()
        {
            var register = factory.CreateSingletonMediator();
            Assert.IsNotNull(register.Mediator);
        }

        [TestMethod]
        public void MediatorFactory_Returns_IRegisterInterfaceOnCreateNewMediator()
        {
            Type mediatorInterfaceType = factory.CreateNewMediator().GetType();

            Assert.IsTrue(typeof(IRegisterer<string>).IsAssignableFrom(mediatorInterfaceType));
        }

        [TestMethod]
        public void MediatorFactory_Returns_Not_SameObjectOnCreateNewMediator()
        {
            var mediator = factory.CreateNewMediator();
            var mediator2ndInstance = factory.CreateNewMediator();

            Assert.AreNotSame(mediator, mediator2ndInstance);
        }
    }
}
