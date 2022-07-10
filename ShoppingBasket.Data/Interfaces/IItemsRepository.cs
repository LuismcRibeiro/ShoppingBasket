using ShoppingList.Domain.Model;

namespace ShoppingBasket.Data.Repositories.Interfaces
{
    public interface IItemsRepository
    {
        public Item GetItem(string name);
    }
}