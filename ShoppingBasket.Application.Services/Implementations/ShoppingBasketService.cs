using ShoppingBasket.Application.Services.Interfaces;
using ShoppingBasket.Data.Gateways.Interfaces;
using ShoppingBasket.Data.Repositories.Interfaces;
using ShoppingList.Domain.Model;

namespace ShoppingBasket.Application.Services.Implementations
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly IItemsRepository itemsRepository;
        private readonly IPromotionServiceGateway promotionServiceGateway;

        public ShoppingBasketService(IItemsRepository itemsRepository, IPromotionServiceGateway promotionServiceGateway)
        {
            this.itemsRepository = itemsRepository;
            this.promotionServiceGateway = promotionServiceGateway;
        }

        public ShoppingBilling CalculateShoppingList(IEnumerable<string> requestedItems)
        {
            var shoppingBilling = new ShoppingBilling();

            var itemList = new List<Item>();

            foreach (var itemRequest in requestedItems)
            {
                var item = this.itemsRepository.GetItem(itemRequest);

                if (item == null)
                {
                    shoppingBilling.ItemsNotFound.Add(itemRequest);
                }
                else
                {
                    itemList.Add(item);
                }
            }

            //No point in calculating further if we only wish to report not found items
            if (shoppingBilling.ItemsNotFound.Any())
            {
                return shoppingBilling;
            }

            this.TallyShoppingList(shoppingBilling, itemList);

            return shoppingBilling;
        }

        private void TallyShoppingList(ShoppingBilling shoppingBilling, List<Item> itemList)
        {
            //this could be made async and be called alongside the repository call
            var promotions = this.promotionServiceGateway.GetActivePromotions();

            foreach (var item in itemList)
            {
                shoppingBilling.Subtotal += item.Price;
                var priceAfterDiscount = item.Price;

                //Judgement call: if more than one promotion applies for the same item will only select the one with most discount
                var appliedPromotion = promotions?
                    .Where(promotion => promotion.ItemName.Equals(item.Name) && promotion.IsActive(itemList))
                    .OrderByDescending(promotion => promotion.DiscountPercentage)
                    .FirstOrDefault();

                if (appliedPromotion != null)
                {
                    priceAfterDiscount = Math.Round((item.Price * (100 - appliedPromotion.DiscountPercentage)) / 100, 2);

                    shoppingBilling.AppliedDiscounts.Add(new AppliedDiscount
                    {
                        ItemName = item.Name,
                        DiscountPercentage = appliedPromotion.DiscountPercentage,
                        SavedValue = Math.Round(item.Price - priceAfterDiscount, 2)
                    });
                }

                shoppingBilling.Total += priceAfterDiscount;

            }
        }
    }
}