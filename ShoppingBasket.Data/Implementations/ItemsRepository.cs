using ShoppingBasket.Data.Repositories.Interfaces;
using ShoppingList.Domain.Model;

namespace ShoppingBasket.Data.Repositories.Implementations
{
    // This class serves as an emulation of a Database repository - should this project evolve further and created the need for a better storage device.
    // For time constraint reasons, we will be storing Domain.Model entities in the Database, this should be replaced by Data level entity models and some form of mapping should be effected
    public class ItemsRepository : IItemsRepository
    {
        private readonly Dictionary<string, Item> inMemoryDatabase = new Dictionary<string, Item>
        {
            { "soup", new Item{ Name = "soup", Price = 0.65 } },
            { "bread", new Item{ Name = "bread", Price = 0.80 } },
            { "milk", new Item{ Name = "milk", Price = 1.30 } },
            { "apples", new Item{ Name = "apples", Price = 1 } }

        };

        public Item GetItem(string name)
        {
            Item item = null;

            if (!this.inMemoryDatabase.TryGetValue(name,out item))
            {
                //possibly log something here for investigation puposes
            }

            return item;
        }
    }
}