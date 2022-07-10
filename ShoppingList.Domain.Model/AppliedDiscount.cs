namespace ShoppingList.Domain.Model
{
    public class AppliedDiscount
    {
        public string ItemName { get; set; }

        public int DiscountPercentage { get; set; }

        public double SavedValue { get; set; }
    }
}