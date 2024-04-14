namespace Microsoft.VisualStudio.TestTools.UnitTesting;

public static class Accuracy
{
    public static IEqualityComparer<double> Eps(double eps) => new AccuracyComparer(eps);
    public static IEqualityComparer<int> Eps(int eps) => new AccuracyComparer(eps);

    public static IEqualityComparer<T> Equals<T>(Func<T, T, bool> Comparer, Func<T, int> Hasher) => new AccuracyEqualityComparer<T>(Comparer, Hasher);
    public static IComparer<T> Compare<T>(Comparison<T> Comparer) => new AccuracyComparer<T>(Comparer);
}

public readonly struct AccuracyComparer(double Eps) :
    IEqualityComparer<double>, IComparer<double>,
    IEqualityComparer<int>, IComparer<int>
{
    private double Eps { get; init; } = Eps switch
    {
        < 0 => throw new ArgumentOutOfRangeException(nameof(Eps), Eps, "Значение точности не должно быть меньше нуля"),
        double.NaN => throw new ArgumentException("Значение точности не должно быть NaN", nameof(Eps)),
        _ => Eps
    };

    public bool Equals(double x, double y) => Math.Abs(x - y) <= Eps;

    public int GetHashCode(double x) => x is double.NaN 
        ? x.GetHashCode() 
        : (Math.Round(x * Eps) / Eps).GetHashCode();

    public int Compare(double x, double y)
    {
        var delta = x - y;
        if (delta is double.NaN)
            throw new InvalidOperationException("Сравнение с NaN")
            {
                Data =
                {
                    { nameof(x), x },
                    { nameof(y), y },
                }
            };
        return Math.Abs(delta) <= Eps 
            ? 0 
            : Math.Sign(delta);
    }

    public bool Equals(int x, int y) => Math.Abs(x - y) <= Eps;

    public int GetHashCode(int x) => (Math.Round(x * Eps) / Eps).GetHashCode();

    public int Compare(int x, int y)
    {
        var delta = x - y;
        return Math.Abs(delta) <= Eps
            ? 0
            : Math.Sign(delta);
    }
}

public class AccuracyEqualityComparer<T>(Func<T, T, bool> Comparer, Func<T, int> Hasher) : IEqualityComparer<T>
{
    public bool Equals(T x, T y) => Comparer(x, y);

    public int GetHashCode(T obj) => Hasher(obj);
}

public class AccuracyComparer<T>(Comparison<T> Comparer) : IComparer<T>
{
    public int Compare(T x, T y) => Comparer(x, y);
}
