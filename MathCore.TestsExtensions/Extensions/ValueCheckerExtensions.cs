// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки значения</summary>
public static class ValueCheckerExtensions
{
    /// <summary>Проверка значения как коллекции элементов</summary>
    /// <param name="Checker">Объект проверки одиночного значения</param>
    /// <typeparam name="T">Тип проверяемого значения, которое является коллекцией объектов типа <typeparamref name="TItem"/></typeparam>
    /// <typeparam name="TItem">Тип элементов проверяемой коллекции</typeparam>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<TItem> Are<T, TItem>(this ValueChecker<T> Checker) where T : ICollection<TItem> => new(Checker.ActualValue);

    /// <summary>Выполнение проверки элементов коллекции</summary>
    /// <param name="Checker">Объект проверки одиночного значения</param>
    /// <param name="Check">Метод проверки элементов коллекции с учётом порядкового номера</param>
    /// <returns>Исходный объект проверки коллекции</returns>
    public static ValueChecker<T> Items<T, TItem>(this ValueChecker<T> Checker, Action<ValueChecker<TItem>, int> Check) where T : IReadOnlyList<TItem>
    {
        var collection = Checker.ActualValue;
        var count = collection.Count;
        for (var i = 0; i < count; i++)
            Check(new ValueChecker<TItem>(collection[i]), i);

        return Checker;
    }

    /// <summary>Выполнение проверки элементов коллекции</summary>
    /// <param name="Checker">Объект проверки одиночного значения</param>
    /// <param name="Check">Метод проверки элементов коллекции</param>
    /// <returns>Исходный объект проверки коллекции</returns>
    public static ValueChecker<T> Items<T, TItem>(this ValueChecker<T> Checker, Action<ValueChecker<TItem>> Check) where T : IReadOnlyList<TItem>
    {
        foreach (var checker in Checker.ActualValue.Select(c => new ValueChecker<TItem>(c)))
            Check(checker);

        return Checker;
    }
}