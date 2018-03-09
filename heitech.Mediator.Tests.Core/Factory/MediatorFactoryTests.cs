using heitech.Mediator.Factory;
using heitech.Mediator.Interface;
using heitech.Mediator.Tests.Core.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace heitech.Mediator.Tests.Core.Factory
{
    [TestClass]
    public class MediatorFactoryTests
    {
        private MediatorFactory factory = new MediatorFactory();

        [TestMethod]
        public void MediatorFactory_ReturnsIRegisterInterface()
        {
            var mediator = factory.CreateSingletonMediator();

            Assert.IsTrue(typeof(IRegister).IsAssignableFrom(mediator.GetType()));
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
        public void MediatorFactory_MediatorIsRegisteredDoesNotThrowException()
        {
            var mediator = factory.CreateSingletonMediator();
            Assert.IsFalse(mediator.Mediator.IsRegistered<IMediatable_1>());
        }

        [TestMethod]
        public void MediatorFactory_Returns_IRegisterInterfaceOnCreateNewMediator()
        {
            Type mediatorInterfaceType = factory.CreateNewMediator().GetType();

            Assert.IsTrue(typeof(IRegister).IsAssignableFrom(mediatorInterfaceType));
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
