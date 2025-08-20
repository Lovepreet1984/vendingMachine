namespace VendingMachine.Domain;

/// <summary>
/// Identifies coins by physical characteristics (approximate US coin specs).
/// </summary>
public sealed class CoinClassifier
{
    // US coin reference values (approximate)
    private const decimal PennyWeight = 2.50m;
    private const decimal PennyDiameter = 19.05m;

    private const decimal NickelWeight = 5.00m;
    private const decimal NickelDiameter = 21.21m;

    private const decimal DimeWeight = 2.268m;
    private const decimal DimeDiameter = 17.91m;

    private const decimal QuarterWeight = 5.670m;
    private const decimal QuarterDiameter = 24.26m;

    // Tolerances (to simulate measurement inaccuracy)
    private const decimal WeightTolerance = 0.15m;   // grams
    private const decimal DiameterTolerance = 0.20m; // mm

    public CoinKind Identify(CoinSample sample)
    {
        if (Matches(sample, NickelWeight, NickelDiameter))  return CoinKind.Nickel;
        if (Matches(sample, DimeWeight, DimeDiameter))      return CoinKind.Dime;
        if (Matches(sample, QuarterWeight, QuarterDiameter))return CoinKind.Quarter;
        if (Matches(sample, PennyWeight, PennyDiameter))    return CoinKind.Penny;
        return CoinKind.Unknown;
    }

    private static bool Matches(CoinSample s, decimal refWeight, decimal refDiameter) =>
        Math.Abs(s.WeightGrams - refWeight) <= WeightTolerance &&
        Math.Abs(s.DiameterMm - refDiameter) <= DiameterTolerance;
}
