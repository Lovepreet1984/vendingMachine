using NUnit.Framework;
using VendingMachine.Domain;

namespace VendingMachine.Tests;

[TestFixture]
public class VendingMachineTests
{
    private VendingMachine.Domain.VendingMachine _vm = null!;

    [SetUp]
    public void SetUp()
    {
        _vm = new VendingMachine.Domain.VendingMachine();
    }

    [Test]
    public void Display_WhenNoCoins_ShowsInsertCoin()
    {
        Assert.That(_vm.GetDisplay(), Is.EqualTo("INSERT COIN"));
    }

    [TestCase(5.00, 21.21, 0.05, TestName = "AcceptCoin_Nickel_IncreasesBy5c")]
    [TestCase(2.268, 17.91, 0.10, TestName = "AcceptCoin_Dime_IncreasesBy10c")]
    [TestCase(5.670, 24.26, 0.25, TestName = "AcceptCoin_Quarter_IncreasesBy25c")]
    public void AcceptCoin_ValidCoins_UpdateAmount(
        decimal weight, decimal diameter, decimal expectedAdd)
    {
        _vm.AcceptCoin(new CoinSample(weight, diameter));
        Assert.That(_vm.CurrentAmount, Is.EqualTo(expectedAdd).Within(0.0001m));
        Assert.That(_vm.GetDisplay(), Is.EqualTo($"${expectedAdd:0.00}"));
        Assert.That(_vm.CoinReturn, Is.Empty);
    }

    [TestCase(2.50, 19.05, CoinKind.Penny, TestName = "AcceptCoin_Penny_Rejected")]
    [TestCase(3.00, 21.00, CoinKind.Unknown, TestName = "AcceptCoin_Unknown_Rejected")]
    public void AcceptCoin_InvalidCoins_AreRejected(
        decimal weight, decimal diameter, CoinKind expectedKind)
    {
        _vm.AcceptCoin(new CoinSample(weight, diameter));
        Assert.That(_vm.CurrentAmount, Is.EqualTo(0m));
        Assert.That(_vm.CoinReturn, Has.Count.EqualTo(1));
        Assert.That(_vm.CoinReturn[0], Is.EqualTo(expectedKind));
        Assert.That(_vm.GetDisplay(), Is.EqualTo("INSERT COIN"));
    }

    [Test]
    public void SelectProduct_InsufficientFunds_ShowsPriceOnce()
    {

        _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));


        _vm.SelectProduct(Product.Candy);


        Assert.That(_vm.GetDisplay(), Is.EqualTo("PRICE $0.65"));

        Assert.That(_vm.GetDisplay(), Is.EqualTo("$0.25"));

        Assert.That(_vm.CurrentAmount, Is.EqualTo(0.25m));
        Assert.That(_vm.LastDispensedProduct, Is.Null);
    }

    [Test]
    public void SelectProduct_ExactlyEnough_ShowsThankYouThenReset()
    {

        for (int i = 0; i < 4; i++)
            _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));

        Assert.That(_vm.GetDisplay(), Is.EqualTo("$1.00"));

        _vm.SelectProduct(Product.Cola);


        Assert.That(_vm.GetDisplay(), Is.EqualTo("THANK YOU"));


        Assert.That(_vm.GetDisplay(), Is.EqualTo("INSERT COIN"));
        Assert.That(_vm.CurrentAmount, Is.EqualTo(0m));
        Assert.That(_vm.LastDispensedProduct, Is.EqualTo(Product.Cola));
    }

    [Test]
    public void SelectProduct_WithMoreThanEnough_StillDispensesAndResets()
    {

        _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));
        _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));
        _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));
        _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));
        _vm.AcceptCoin(new CoinSample(5.670m, 24.26m));

        _vm.SelectProduct(Product.Candy); 

  
        Assert.That(_vm.GetDisplay(), Is.EqualTo("THANK YOU"));
        Assert.That(_vm.GetDisplay(), Is.EqualTo("INSERT COIN"));
        Assert.That(_vm.CurrentAmount, Is.EqualTo(0m));
        Assert.That(_vm.LastDispensedProduct, Is.EqualTo(Product.Candy));
    }

    [Test]
    public void Display_Tolerance_AcceptsSlightMeasurementVariance()
    {

        _vm.AcceptCoin(new CoinSample(5.70m, 24.20m));
        Assert.That(_vm.CurrentAmount, Is.EqualTo(0.25m).Within(0.0001m));
    }
}
