namespace ShoppingList.Domain.Model
{
    public class Promotion
    {
        public string ItemName { get; set; }

        public int DiscountPercentage { get; set; }

        public Func<IEnumerable<Item>, bool>? IsActive { get; set; }
    }
}