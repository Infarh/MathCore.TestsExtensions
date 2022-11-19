using System.Runtime.CompilerServices;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace Microsoft.VisualStudio.TestTools.UnitTesting.Extensions;

public static class ObjectTestingExtensions
{
    public static ValueChecker<T> AssertNotNull<T>(this T value, [CallerArgumentExpression("value")] string? Message = null) => 
        value.AssertThatValue().AsNotNull(Message);

    /// <summary>Результат является истинным</summary>
    /// <param name="value">Проверяемое выражение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки выражения типа <see cref="bool"/></returns>
    public static ValueChecker<bool> AssertTrue(this bool value, [CallerArgumentExpression("value")] string? Message = null) =>
        Assert.That
           .Value(value)
           .IsEqual(true, Message);

    /// <summary>Результат является ложным</summary>
    /// <param name="value">Проверяемое выражение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки выражения типа <see cref="bool"/></returns>
    public static ValueChecker<bool> AssertFalse(this bool value, [CallerArgumentExpression("value")] string? Message = null) =>
        Assert.That
           .Value(value)
           .IsEqual(false, Message);

    /// <summary>Проверка выражения</summary>
    /// <typeparam name="T">Тип значения</typeparam>
    /// <param name="value">Проверяемое значение</param>
    /// <returns>Объект проверки значения</returns>
    public static ValueChecker<T> AssertThatValue<T>(this T value) => Assert.That.Value(value);

    /// <summary>Проверка что вещественное значение равно указанному ожидаемому</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ActualValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertEquals(
        this double value,
        double ActualValue,
        [CallerArgumentExpression("value")]
        string? Message = null) =>
        (DoubleValueChecker)Assert.That
           .Value(value)
           .IsEqual(ActualValue, Message);

    public static ValueChecker<T> AssertEquals<T>(
        this T value, 
        T ActualValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .IsEqual(ActualValue, Message);

    public static DoubleValueChecker AssertEquals(
        this double value, 
        double ActualValue, 
        double Eps, 
        [CallerArgumentExpression("value")] 
        string? Message = null) =>
        (DoubleValueChecker)Assert.That
           .Value(value)
           .IsEqual(ActualValue, Eps, Message);

    public static CollectionChecker<T> AssertThatCollection<T>(this ICollection<T> collection) => 
        Assert.That
           .Collection(collection);

    public static CollectionChecker<T> AssertCount<T>(this T[] collection, int Count, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .CountEquals(Count, Message);

    public static CollectionChecker<T> AssertCount<T>(this List<T> collection, int Count, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .CountEquals(Count, Message);

    public static CollectionChecker<T> AssertCount<T>(this ICollection<T> collection, int Count, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .CountEquals(Count, Message);

    public static CollectionChecker<T> AssertIsEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsEmpty(Message);

    public static CollectionChecker<T> AssertIsEmpty<T>(this T[] collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsEmpty(Message);

    public static CollectionChecker<T> AssertIsEmpty<T>(this List<T> collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsEmpty(Message);

    public static CollectionChecker<T> AssertIsNotEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsNotEmpty(Message);

    public static CollectionChecker<T> AssertIsNotEmpty<T>(this T[] collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsNotEmpty(Message);

    public static CollectionChecker<T> AssertIsNotEmpty<T>(this List<T> collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsNotEmpty(Message);

    public static CollectionChecker<T> AssertIsSingle<T>(this ICollection<T> collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsSingleItem(Message);

    public static CollectionChecker<T> AssertIsSingle<T>(this T[] collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsSingleItem(Message);

    public static CollectionChecker<T> AssertIsSingle<T>(this List<T> collection, [CallerArgumentExpression("collection")] string? Message = null) => 
        Assert.That
           .Collection(collection)
           .IsSingleItem(Message);

    public static CollectionChecker<T> AssertEquals<T>(this ICollection<T> collection, params T[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args);

    public static DoubleCollectionChecker AssertEquals<T>(this ICollection<double> collection, params double[] args) =>
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

    public static DoubleCollectionChecker AssertEquals<T>(
        this ICollection<double> collection,
        IEqualityComparer<double> Comparer, 
        params double[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /* ------------------------------------------------------------------------------------------------------------- */

    public static ReadOnlyCollectionChecker<T> AssertThatCollection<T>(this IReadOnlyCollection<T> collection) =>
        Assert.That
           .Collection(collection);

    public static ReadOnlyCollectionChecker<T> AssertCount<T>(this IReadOnlyCollection<T> collection, int Count, [CallerArgumentExpression("collection")] string? Message = null) =>
        Assert.That
           .Collection(collection)
           .CountEquals(Count, Message);

    public static ReadOnlyCollectionChecker<T> AssertIsEmpty<T>(this IReadOnlyCollection<T> collection, [CallerArgumentExpression("collection")] string? Message = null) =>
        Assert.That
           .Collection(collection)
           .IsEmpty(Message);

    public static ReadOnlyCollectionChecker<T> AssertIsNotEmpty<T>(this IReadOnlyCollection<T> collection, [CallerArgumentExpression("collection")] string? Message = null) =>
        Assert.That
           .Collection(collection)
           .IsNotEmpty(Message);

    public static ReadOnlyCollectionChecker<T> AssertIsSingle<T>(this IReadOnlyCollection<T> collection, [CallerArgumentExpression("collection")] string? Message = null) =>
        Assert.That
           .Collection(collection)
           .IsSingleItem(Message);

    public static ReadOnlyCollectionChecker<T> AssertEquals<T>(this IReadOnlyCollection<T> collection, params T[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args);

    public static DoubleReadOnlyCollectionChecker AssertEquals<T>(this IReadOnlyCollection<double> collection, params double[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args);

    public static ReadOnlyCollectionChecker<T> AssertEquals<T>(
        this IReadOnlyCollection<T> collection,
        IEqualityComparer<T> Comparer,
        params T[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    public static DoubleReadOnlyCollectionChecker AssertEquals<T>(
        this IReadOnlyCollection<double> collection,
        IEqualityComparer<double> Comparer,
        params double[] args) =>
        Assert.That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /* ------------------------------------------------------------------------------------------------------------- */

    public static EnumerableChecker<T> AssertThatEnumerable<T>(this IEnumerable<T> items) => 
        Assert.That
           .Enumerable(items);

    public static EnumerableChecker<T> AssertEnumerableIsEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string? Message = null) => 
        Assert.That
           .Enumerable(items)
           .IsEmpty(Message);

    public static EnumerableChecker<T> AssertEnumerableIsNotEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string? Message = null) => 
        Assert.That
           .Enumerable(items)
           .IsNotEmpty(Message);

    public static EnumerableChecker<T> AssertEnumerableIsSingleItem<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string? Message = null) => 
        Assert.That
           .Enumerable(items)
           .IsSingleItem(Message);

    public static EnumerableChecker<T> AssertEnumerableCount<T>(this IEnumerable<T> items, int ExpectedCount, [CallerArgumentExpression("items")] string? Message = null) => 
        Assert.That
           .Enumerable(items)
           .IsItemsCount(ExpectedCount, Message);

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

    public static DoubleValueChecker AssertLessThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessThan(ExpectedValue, Message);
    
    public static DoubleValueChecker AssertLessThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessThan(ExpectedValue, Accuracy, Message);
    
    public static DoubleValueChecker AssertLessOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessOrEqualsThan(ExpectedValue, Message);
    
    public static DoubleValueChecker AssertLessOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .LessOrEqualsThan(ExpectedValue, Accuracy, Message);
    
    public static DoubleValueChecker AssertGreaterThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterThan(ExpectedValue, Message);
    
    public static DoubleValueChecker AssertGreaterThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterThan(ExpectedValue, Accuracy, Message);
    
    public static DoubleValueChecker AssertGreaterOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterOrEqualsThan(ExpectedValue, Message);
    
    public static DoubleValueChecker AssertGreaterOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression("value")] 
        string? Message = null) => 
        Assert.That
           .Value(value)
           .GreaterOrEqualsThan(ExpectedValue, Accuracy, Message);
}
