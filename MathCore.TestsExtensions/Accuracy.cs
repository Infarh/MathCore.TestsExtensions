namespace Microsoft.VisualStudio.TestTools.UnitTesting;

public static class Accuracy
{
    public static IEqualityComparer<double> Eps(double eps) => new AccuracyComparer(eps);
    public static IEqualityComparer<int> Eps(int eps) => new AccuracyComparer(eps);

    public static IEqualityComparer<T> Equals<T>(Func<T, T, bool> Comparer, Func<T, int> Hasher) => new AccuracyEqualityComparer<T>(Comparer, Hasher);
    public static IComparer<T> Compare<T>(Comparison<T> Comparer) => new AccuracyComparer<T>(Comparer);
}

public readonly struct AccuracyComparer : 
    IEqualityComparer<double>, IComparer<double>, 
    IEqualityComparer<int>, IComparer<int>
{
    private double Eps { get; init; }

    public AccuracyComparer(double Eps) =>
        this.Eps = Eps switch
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

public class AccuracyEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _Comparer;
    private readonly Func<T, int> _Hasher;

    public AccuracyEqualityComparer(Func<T, T, bool> Comparer, Func<T, int> Hasher)
    {
        _Comparer = Comparer;
        _Hasher = Hasher;
    }

    public bool Equals(T x, T y) => _Comparer(x, y);

    public int GetHashCode(T obj) => _Hasher(obj);
}

public class AccuracyComparer<T> : IComparer<T>
{
    private readonly Comparison<T> _Comparer;

    public AccuracyComparer(Comparison<T> Comparer) => _Comparer = Comparer;

    public int Compare(T x, T y) => _Comparer(x, y);
}
