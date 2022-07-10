using ShoppingBasket.Data.Gateways.Interfaces;
using ShoppingList.Domain.Model;

namespace ShoppingBasket.Data.Gateways.Implementations
{
    //This class will emulate an access to an external service that manages and returns the active promotions
    public class PromotionServiceGateway : IPromotionServiceGateway
    {
        private readonly List<Promotion> InMemoryServiceResponse = new List<Promotion>
        {
           new Promotion
           {
                ItemName = "apples",
                DiscountPercentage = 10,
                IsActive = (IEnumerable<Item> list) =>
                {
                    return true;
                }
           },
           new Promotion
           {
                ItemName = "bread",
                DiscountPercentage = 50,
                IsActive = (IEnumerable<Item> list) =>
                {
                    if (list!= null && list.Where(item => item.Name == "soup").Count() >= 2)
                    {
                        return true;
                    }

                    return false;
                }
           },
        };

        public IEnumerable<Promotion> GetActivePromotions()
        {
            //should protect against a possible service invalid response here

            return this.InMemoryServiceResponse;
        }
    }
}