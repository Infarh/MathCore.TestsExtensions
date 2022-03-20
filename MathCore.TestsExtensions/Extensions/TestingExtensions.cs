namespace Microsoft.VisualStudio.TestTools.UnitTesting.Extensions;

public static class TestingExtensions
{
    public static ValueChecker<T> AssertThatValue<T>(this T value) => Assert.That.Value(value);
    public static ValueChecker<T> AssertEquals<T>(this T value, T ActualValue) => Assert.That.Value(value).IsEqual(ActualValue);
    public static ValueChecker<double> AssertEquals(this double value, double ActualValue, double Eps) => Assert.That.Value(value).IsEqual(ActualValue, Eps);

    public static CollectionChecker<T> AssertThatCollection<T>(this ICollection<T> collection) => Assert.That.Collection(collection);

    public static CollectionChecker<T> AssertEquals<T>(this ICollection<T> collection, params T[] args) =>
        Assert.That.Collection(collection).IsEqualTo(args);

    public static EnumerableChecker<T> AssertThatEnumerable<T>(this IEnumerable<T> items) => Assert.That.Enumerable(items);

    public static EnumerableChecker<T> AssertEquals<T>(this IEnumerable<T> items, params T[] values) => Assert.That.Enumerable(items).IsEqualTo(values);

    public static ValueChecker<double> AssertLessThan(this double ActualValue, double ExpectedValue) => Assert.That.Value(ActualValue).LessThan(ExpectedValue);
    public static ValueChecker<double> AssertLessThan(this double ActualValue, double ExpectedValue, double Accuracy) => Assert.That.Value(ActualValue).LessThan(ExpectedValue, Accuracy);
    public static ValueChecker<double> AssertLessOrEqualsThan(this double ActualValue, double ExpectedValue) => Assert.That.Value(ActualValue).LessOrEqualsThan(ExpectedValue);
    public static ValueChecker<double> AssertLessOrEqualsThan(this double ActualValue, double ExpectedValue, double Accuracy) => Assert.That.Value(ActualValue).LessOrEqualsThan(ExpectedValue, Accuracy);
    public static ValueChecker<double> AssertGreaterThan(this double ActualValue, double ExpectedValue) => Assert.That.Value(ActualValue).GreaterThan(ExpectedValue);
    public static ValueChecker<double> AssertGreaterThan(this double ActualValue, double ExpectedValue, double Accuracy) => Assert.That.Value(ActualValue).GreaterThan(ExpectedValue, Accuracy);
    public static ValueChecker<double> AssertGreaterOrEqualsThan(this double ActualValue, double ExpectedValue) => Assert.That.Value(ActualValue).GreaterOrEqualsThan(ExpectedValue);
    public static ValueChecker<double> AssertGreaterOrEqualsThan(this double ActualValue, double ExpectedValue, double Accuracy) => Assert.That.Value(ActualValue).GreaterOrEqualsThan(ExpectedValue, Accuracy);
}
