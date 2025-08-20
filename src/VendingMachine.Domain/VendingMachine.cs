namespace VendingMachine.Domain;


public sealed class VendingMachine
{
    private readonly CoinClassifier _classifier = new();

    private decimal _currentAmount = 0m;


    private string? _transientMessage;


    public List<CoinKind> CoinReturn { get; } = new();


    public Product? LastDispensedProduct { get; private set; }


    private static readonly IReadOnlyDictionary<CoinKind, decimal> CoinValues =
        new Dictionary<CoinKind, decimal>
        {
            [CoinKind.Nickel] = 0.05m,
            [CoinKind.Dime] = 0.10m,
            [CoinKind.Quarter] = 0.25m
        };

    public string GetDisplay()
    {
        if (_transientMessage is not null)
        {
            var msg = _transientMessage;
            _transientMessage = null;
            return msg!;
        }

        if (_currentAmount <= 0m)
            return "INSERT COIN";

        return MoneyFmt.Dollars(_currentAmount);
    }

    public void AcceptCoin(CoinSample sample)
    {
        var kind = _classifier.Identify(sample);

        if (CoinValues.TryGetValue(kind, out var value))
        {
            _currentAmount += value;
        }
        else
        {
            CoinReturn.Add(kind);
        }
    }


    public void SelectProduct(Product product)
    {
        var price = Pricing.Prices[product];

        if (_currentAmount >= price)
        {
            _currentAmount = 0m;
            LastDispensedProduct = product;
            _transientMessage = "THANK YOU";
        }
        else
        {
            _transientMessage = $"PRICE {MoneyFmt.Dollars(price)}";
        }
    }

    public decimal CurrentAmount => _currentAmount;
}
