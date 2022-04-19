using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Extensions;

public static class TestingExtensions
{
    public static ValueChecker<T> AssertThatValue<T>(this T value) => Assert.That.Value(value);

    public static ValueChecker<T> AssertEquals<T>(
        this T value, 
        T ActualValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .IsEqual(ActualValue, Message);

    public static ValueChecker<double> AssertEquals(
        this double value, 
        double ActualValue, 
        double Eps, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .IsEqual(ActualValue, Eps, Message);

    public static CollectionChecker<T> AssertThatCollection<T>(this ICollection<T> collection) => 
        Assert.That
           .Collection(collection);

    public static CollectionChecker<T> AssertEquals<T>(this ICollection<T> collection, params T[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args);

    public static CollectionChecker<T> AssertEquals<T>(
        this ICollection<T> collection,
        IEqualityComparer<T> Comparer, 
        params T[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    public static EnumerableChecker<T> AssertThatEnumerable<T>(this IEnumerable<T> items) => 
        Assert.That
           .Enumerable(items);

    public static EnumerableChecker<T> AssertEquals<T>(this IEnumerable<T> items, params T[] values) => 
        Assert.That
           .Enumerable(items)
           .IsEqualTo(values);

    public static EnumerableChecker<T> AssertEquals<T>(
        this IEnumerable<T> items, 
        IEqualityComparer<T> Comparer, 
        params T[] values) => 
        Assert.That
           .Enumerable(items)
           .IsEqualTo(values, Comparer);

    public static ValueChecker<double> AssertLessThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessThan(ExpectedValue, Message);
    
    public static ValueChecker<double> AssertLessThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessThan(ExpectedValue, Accuracy, Message);
    
    public static ValueChecker<double> AssertLessOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessOrEqualsThan(ExpectedValue, Message);
    
    public static ValueChecker<double> AssertLessOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessOrEqualsThan(ExpectedValue, Accuracy, Message);
    
    public static ValueChecker<double> AssertGreaterThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterThan(ExpectedValue, Message);
    
    public static ValueChecker<double> AssertGreaterThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterThan(ExpectedValue, Accuracy, Message);
    
    public static ValueChecker<double> AssertGreaterOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterOrEqualsThan(ExpectedValue, Message);
    
    public static ValueChecker<double> AssertGreaterOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterOrEqualsThan(ExpectedValue, Accuracy, Message);
}
