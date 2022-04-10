// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта-помощника проверки <see cref="Assert.That"/></summary>
public static class CollectionAssertExtensions
{
    //public static CollectionAssertChecker Collection(this CollectionAssert assert, ICollection ActualCollection) => new CollectionAssertChecker(ActualCollection);

    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualCollection">Проверяемая коллекция</param>
    /// <returns>Объект проверки</returns>
    public static DoubleCollectionChecker Collection(this CollectionAssert assert, ICollection<double> ActualCollection) => new(ActualCollection);

    /// <summary>Проверка двумерного массива вещественных чисел</summary>
    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualArray">Проверяемый двумерный массив</param>
    /// <returns>Объект проверки</returns>
    public static DoubleDimensionArrayChecker Collection(this CollectionAssert assert, double[,] ActualArray) => new(ActualArray);

    /// <summary>Проверка коллекции</summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualCollection">Проверяемая коллекция</param>
    /// <returns>Объект проверки</returns>
    public static CollectionChecker<T> Collection<T>(this CollectionAssert assert, ICollection<T> ActualCollection) => new(ActualCollection);
}