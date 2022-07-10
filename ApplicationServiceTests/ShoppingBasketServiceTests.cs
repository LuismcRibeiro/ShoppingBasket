using Moq;
using ShoppingBasket.Application.Services.Implementations;
using ShoppingBasket.Data.Gateways.Interfaces;
using ShoppingBasket.Data.Repositories.Interfaces;
using ShoppingList.Domain.Model;

namespace ApplicationServiceTests
{
    [TestClass]
    public class ShoppingBasketServiceTests
    {
        private Mock<IItemsRepository> mockItemsRepository;
        private Mock<IPromotionServiceGateway> mockPromotionServiceGateway;

        private ShoppingBasketService shoppingBasketService;

        [TestInitialize]
        public void Initialize() 
        {
            this.mockItemsRepository = new Mock<IItemsRepository>();
            this.mockPromotionServiceGateway = new Mock<IPromotionServiceGateway>();

            this.shoppingBasketService = new ShoppingBasketService(this.mockItemsRepository.Object, this.mockPromotionServiceGateway.Object);
        
        }

        [TestMethod]
        public void ShoppingBasketService_CalculateShoppingList_NoDataInRepository_ReturnItemsNotFoundAndDoesNotCallPromotionService()
        {
            //Arrange
            this.mockItemsRepository.Setup(m => m.GetItem(It.IsAny<string>())).Returns(null as Item);

            //Act
            var response = this.shoppingBasketService.CalculateShoppingList(new List<string> { "bread" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ItemsNotFound.Any(), "should return items not found");

            this.mockPromotionServiceGateway
                .Verify(mock => mock.GetActivePromotions(),
                    Times.Never,
                    "With invalid items, no call to promotions service should be made");
        }

        [TestMethod]
        public void ShoppingBasketService_CalculateShoppingList_SeveralItemsNoPromotions_ReturnResponse()
        {
            //Arrange
            var breadName = "bread";
            var soupName = "soup";

            var breadItem = new Item
            {
                Name = breadName,
                Price = 20
            };

            var soupItem = new Item
            {
                Name = soupName,
                Price = 10
            };

            this.mockItemsRepository
                .Setup(m => m.GetItem(It.Is<string>(item => item == breadName)))
                .Returns(breadItem);

            this.mockItemsRepository
                .Setup(m => m.GetItem(It.Is<string>(item => item == soupName)))
                .Returns(soupItem);

            this.mockPromotionServiceGateway.Setup(m => m.GetActivePromotions()).Returns(new List<Promotion>());

            //Act
            var response = this.shoppingBasketService.CalculateShoppingList(new List<string> { breadName, soupName });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.ItemsNotFound.Any(), "should not return items not found");

            var expectedPrice = breadItem.Price + soupItem.Price;

            Assert.AreEqual(expectedPrice, response.Total, "Total should match the expected value");
            Assert.AreEqual(expectedPrice, response.Subtotal, "SubTotal should match the expected value");
            Assert.IsFalse(response.AppliedDiscounts.Any(), "should not have any applied promotions");

            this.mockPromotionServiceGateway
                .Verify(mock => mock.GetActivePromotions(),
                    Times.Once,
                    "Should call promotion service once");
        }

        [TestMethod]
        public void ShoppingBasketService_CalculateShoppingList_SeveralItemsWithActivePromotion_ReturnResponseWithDiscount()
        {
            //Arrange
            var breadName = "bread";
            var soupName = "soup";

            var breadItem = new Item
            {
                Name = breadName,
                Price = 20
            };

            var soupItem = new Item
            {
                Name = soupName,
                Price = 10
            };

            this.mockItemsRepository
                .Setup(m => m.GetItem(It.Is<string>(item => item == breadName)))
                .Returns(breadItem);

            this.mockItemsRepository
                .Setup(m => m.GetItem(It.Is<string>(item => item == soupName)))
                .Returns(soupItem);

            var promotion = new Promotion
            {
                ItemName = breadName,
                DiscountPercentage = 50,
                IsActive = (IEnumerable<Item> list) =>
                {
                    return true;
                }
            };

            this.mockPromotionServiceGateway.Setup(m => m.GetActivePromotions())
                .Returns(new List<Promotion> { promotion });

            //Act
            var response = this.shoppingBasketService.CalculateShoppingList(new List<string> { breadName, soupName });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.ItemsNotFound.Any(), "should not return items not found");

            var expectedFullPrice = breadItem.Price + soupItem.Price;
            var breadDiscountPrice = Math.Round((breadItem.Price * (100 - promotion.DiscountPercentage)) / 100, 2);

            var expectedFinalPrice = breadDiscountPrice + soupItem.Price;

            Assert.AreEqual(expectedFullPrice, response.Subtotal, "SubTotal should match the expected value");
            Assert.AreEqual(expectedFinalPrice, response.Total, "Total should match the expected value");
            Assert.IsTrue(response.AppliedDiscounts.Any(), "should have applied promotions");

            var breadPromotion = response.AppliedDiscounts.FirstOrDefault(d => d.ItemName == breadName);

            Assert.AreEqual(promotion.DiscountPercentage, breadPromotion.DiscountPercentage);

            this.mockPromotionServiceGateway
                .Verify(mock => mock.GetActivePromotions(),
                    Times.Once,
                    "Should call promotion service once");
        }
    }
}