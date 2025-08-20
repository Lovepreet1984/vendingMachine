using System.Globalization;

namespace VendingMachine.Domain;

public enum Product
{
    Cola,  
    Chips,  
    Candy   
}

public enum CoinKind
{
    Unknown = 0,
    Penny,
    Nickel,
    Dime,
    Quarter
}

/// <summary>
/// Simulated physical coin sample (weight in grams, diameter in mm).
/// </summary>
public readonly record struct CoinSample(decimal WeightGrams, decimal DiameterMm);

public static class Pricing
{
    public static readonly IReadOnlyDictionary<Product, decimal> Prices = new Dictionary<Product, decimal>
    {
        [Product.Cola] = 1.00m,
        [Product.Chips] = 0.50m,
        [Product.Candy] = 0.65m
    };
}

internal static class MoneyFmt
{
    public static string Dollars(decimal value) =>
        "$" + value.ToString("0.00", CultureInfo.InvariantCulture);
}
