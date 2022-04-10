// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Класс методов-расширений для объекта-помощника проверки <see cref="Assert.That"/></summary>
public static class AssertExtensions
{
    /// <summary>Проверка значения</summary>
    /// <typeparam name="T">ТИп проверяемого значения</typeparam>
    /// <param name="that">Объект-помощник проверки</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Объект проверки</returns>
    public static ValueChecker<T> Value<T>(this Assert that, T ActualValue) => new(ActualValue);

    /// <summary>Проверка вещественного значения</summary>
    /// <param name="that">Объект-помощник проверки</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Объект проверки вещественных значений</returns>
    public static DoubleValueChecker Value(this Assert that, double ActualValue) => new(ActualValue);

    /// <summary>Проверка действия</summary>
    /// <param name="that">Объект-помощник проверки</param>
    /// <param name="action">Проверяемое действие</param>
    /// <returns>Объект проверки исключения</returns>
    public static ActionChecker Method(this Assert that, Action action) => new(action);

    /// <summary>Проверка действия</summary>
    /// <typeparam name="TValue">Тип параметра действия</typeparam>
    /// <param name="that">Объект-помощник проверки</param>
    /// <param name="value">Параметр</param>
    /// <param name="action">Проверяемое действие</param>
    /// <returns>Объект проверки исключения</returns>
    public static ActionChecker<TValue> Method<TValue>(this Assert that, TValue value, Action<TValue> action) => new(action, value);

    /// <summary>Проверка функции</summary>
    /// <typeparam name="TResult">Тип результата функции</typeparam>
    /// <param name="that">Объект-помощник проверки</param>
    /// <param name="function">Проверяемая функция</param>
    /// <returns>Объект проверки исключения</returns>
    public static FunctionChecker<TResult> Method<TResult>(this Assert that, Func<TResult> function) => new(function);

    /// <summary>Проверка функции</summary>
    /// <typeparam name="TResult">Тип результата функции</typeparam>
    /// <typeparam name="TValue">Тип параметра функции</typeparam>
    /// <param name="that">Объект-помощник проверки</param>
    /// <param name="value">Значение, передаваемое в функцию</param>
    /// <param name="function">Проверяемая функция</param>
    /// <returns>Объект проверки исключения</returns>
    public static FunctionChecker<TValue, TResult> Method<TValue, TResult>(this Assert that, TValue value, Func<TValue, TResult> function) => new(function, value);

    #region Collection

    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualCollection">Проверяемая коллекция</param>
    /// <returns>Объект проверки</returns>
    public static DoubleCollectionChecker Collection(this Assert assert, ICollection<double> ActualCollection) => new(ActualCollection);

    /// <summary>Проверка двумерного массива вещественных чисел</summary>
    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualArray">Проверяемый двумерный массив</param>
    /// <returns>Объект проверки</returns>
    public static DoubleDimensionArrayChecker Collection(this Assert assert, double[,] ActualArray) => new(ActualArray);

    /// <summary>Проверка коллекции</summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualCollection">Проверяемая коллекция</param>
    /// <returns>Объект проверки</returns>
    public static CollectionChecker<T> Collection<T>(this Assert assert, ICollection<T> ActualCollection) => new(ActualCollection);

    #endregion

    #region Enumerable

    /// <summary>Проверка коллекции</summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="assert">Объект-помощник проверки</param>
    /// <param name="ActualEnumerable">Проверяемая коллекция</param>
    /// <returns>Объект проверки</returns>
    public static EnumerableChecker<T> Enumerable<T>(this Assert assert, IEnumerable<T> ActualEnumerable) => new(ActualEnumerable);

    #endregion
}