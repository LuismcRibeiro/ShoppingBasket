using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingBasket.Data.Gateways.Interfaces;
using ShoppingBasket.Data.Repositories.Interfaces;
using Moq;
using System;

namespace ServiceTests
{
    [TestClass]
    public class ShoppingBasketService
    {
        private Mock<IItemsRepository> mockItemsRepository;

        [TestInitialize]
        private void Initialize() 
        {
        
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
