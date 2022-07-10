namespace ShoppingList.Domain.Model
{
    public class ShoppingBilling
    {
        public double Subtotal { get; set; }

        public double Total { get; set; }

        public List<AppliedDiscount> AppliedDiscounts { get; set; } = new List<AppliedDiscount>();

        public List<string> ItemsNotFound { get; set; } = new List<string>();
    }
}