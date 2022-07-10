using ShoppingList.Domain.Model;

namespace ShoppingBasket.Application.Services.Interfaces
{
    public interface IShoppingBasketService
    {
        public ShoppingBilling CalculateShoppingList(IEnumerable<string> requestedItems);
    }
}