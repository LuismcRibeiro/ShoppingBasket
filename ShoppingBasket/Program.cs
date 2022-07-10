using ShoppingBasket.Presentation;
using System.Text;
{
    var host = Startup.CreateHostBuilder(args).Build();
    Console.OutputEncoding = Encoding.Default;

    Terminal.ShoppingList(host);

    Environment.Exit(0);
}