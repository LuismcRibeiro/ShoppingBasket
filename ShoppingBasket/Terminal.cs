using Microsoft.Extensions.Hosting;
using ShoppingBasket.Application.Services.Interfaces;
using ShoppingList.Domain.Model;

namespace ShoppingBasket.Presentation
{
    internal static class Terminal
    {
        private const string shoppingbasketCommand = "shoppingbasket ";

        public static void ShoppingList(IHost host)
        {
            Greeting();

            var validResponse = false;

            while (!validResponse)
            {
                var input = ReadInput();
                var billing = CalculateBilling(input, host);
                validResponse = WriteResponse(billing);
            }
        }

        private static void Greeting()
        {
            Console.WriteLine("Welcome to our Shopping Basket terminal!");
            Console.WriteLine($"To get started please write \"{shoppingbasketCommand}\" followed by the items you wish to buy separated by space");
            Console.WriteLine($"example: {shoppingbasketCommand} bread apples soup bread");
        }

        public static List<string> ReadInput()
        {
            var requestedItems = new List<string>();

            var validCommandGiven = false;

            while (!validCommandGiven)
            {
                var userInput = Console.ReadLine();

                if (!userInput.Contains(shoppingbasketCommand))
                {
                    Console.WriteLine("Im sorry, the provided input was not valid, please refer to the provided example for help.");

                    continue;
                }

                var unparsedItems = userInput.Replace(shoppingbasketCommand, "");

                var parsedItems = unparsedItems?.Split(" ");

                if (parsedItems == null || !parsedItems.Any())
                {
                    Console.WriteLine("Im sorry, the provided input was not valid, please refer to the provided example for help.");
                    continue;
                }

                requestedItems.AddRange(unparsedItems.Split(" "));

                validCommandGiven = true;
            }

            return requestedItems;
        }

        private static ShoppingBilling? CalculateBilling(List<string> requestedItems, IHost host)
        {
            var shoppingBasketService = (IShoppingBasketService)host?.Services?.GetService(typeof(IShoppingBasketService));

            return shoppingBasketService?.CalculateShoppingList(requestedItems);
        }

        private static bool WriteResponse(ShoppingBilling shoppingBilling)
        {
            Console.WriteLine("Shopping Cost");

            if (shoppingBilling.ItemsNotFound.Any())
            {
                Console.WriteLine($"Whoops! something went wrong, can't find the following item(s)");

                foreach (var notFoundItem in shoppingBilling.ItemsNotFound)
                {
                    Console.WriteLine(notFoundItem);
                }

                return false;
            }
            else
            {
                Console.WriteLine($"Subtotal: €{shoppingBilling.Subtotal}");

                if (!shoppingBilling.AppliedDiscounts.Any())
                {
                    Console.WriteLine("(No offers available)");
                }

                foreach (var appliedDiscount in shoppingBilling.AppliedDiscounts)
                {
                    Console.WriteLine($"{appliedDiscount.ItemName} {appliedDiscount.DiscountPercentage}% off: -€{appliedDiscount.SavedValue}");
                }

                Console.WriteLine($"Total: €{shoppingBilling.Total}");

                return true;
            }
        }
    }
}