using System;
using VendingMachine.Domain;

internal class Program
{
    private static void Main()
    {
        var vm = new VendingMachine.Domain.VendingMachine();

        Console.WriteLine("Vending Machine Demo");
        Console.WriteLine("--------------------");
        Console.WriteLine("Commands:");
        Console.WriteLine(" put the coin nickel|dime|quarter|penny");
        Console.WriteLine(" product you can select cola|chips|candy");
        Console.WriteLine("  display");
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(input)) continue;
            if (input is "exit" or "quit") break;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0];

            switch (cmd)
            {
                case "coin":
                    if (parts.Length < 2) { Console.WriteLine("Usage: coin nickel|dime|quarter|penny"); break; }
                    var sample = parts[1] switch
                    {
                        "nickel"  => new CoinSample(5.00m, 21.21m),
                        "dime"    => new CoinSample(2.268m, 17.91m),
                        "quarter" => new CoinSample(5.670m, 24.26m),
                        "penny"   => new CoinSample(2.50m, 19.05m),
                        _ => new CoinSample(3.00m, 21.00m) // unknown
                    };
                    vm.AcceptCoin(sample);
                    Console.WriteLine($"Display: {vm.GetDisplay()}");
                    if (vm.CoinReturn.Count > 0)
                    {
                        Console.WriteLine($"Coin return: {vm.CoinReturn[^1]}");
                    }
                    break;

                case "select":
                    if (parts.Length < 2) { Console.WriteLine("Usage: select cola|chips|candy"); break; }
                    if (!Enum.TryParse<Product>(parts[1], true, out var product))
                    {
                        Console.WriteLine("Unknown product. Use cola|chips|candy.");
                        break;
                    }
                    vm.SelectProduct(product);
                    Console.WriteLine($"Display: {vm.GetDisplay()}");
                    Console.WriteLine($"Next display: {vm.GetDisplay()}"); // shows amount or INSERT COIN
                    break;

                case "display":
                    Console.WriteLine($"Display: {vm.GetDisplay()}");
                    break;

                case "return":
                    if (vm.CoinReturn.Count == 0) Console.WriteLine("Coin return is empty.");
                    else Console.WriteLine($"Coin return last: {vm.CoinReturn[^1]} (total items: {vm.CoinReturn.Count})");
                    break;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
