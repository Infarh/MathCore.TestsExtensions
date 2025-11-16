// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global

using System.Collections.ObjectModel;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Класс методов-расширений для объекта-помощника проверки <see cref="Assert.That"/></summary>
public static class AssertExtensions
{
    /// <param name="that">Объект-помощник проверки</param>
    extension(Assert that)
    {
        /// <summary>Проверка значения</summary>
        /// <typeparam name="T">ТИп проверяемого значения</typeparam>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки</returns>
        public ValueChecker<T> Value<T>(T ActualValue) => new(ActualValue);

        /// <summary>Проверка вещественного значения</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки вещественных значений</returns>
        public DoubleValueChecker Value(double ActualValue) => new(ActualValue);

        /// <summary>Проверка действия</summary>
        /// <param name="action">Проверяемое действие</param>
        /// <returns>Объект проверки исключения</returns>
        public ActionChecker Method(Action action) => new(action);

        /// <summary>Проверка действия</summary>
        /// <typeparam name="T">Тип параметра действия</typeparam>
        /// <param name="value">Параметр</param>
        /// <param name="action">Проверяемое действие</param>
        /// <returns>Объект проверки исключения</returns>
        public ActionChecker<T> Method<T>(T value, Action<T> action) => new(action, value);

        /// <summary>Проверка функции</summary>
        /// <typeparam name="T">Тип результата функции</typeparam>
        /// <param name="function">Проверяемая функция</param>
        /// <returns>Объект проверки исключения</returns>
        public FunctionChecker<T> Method<T>(Func<T> function) => new(function);

        /// <summary>Проверка функции</summary>
        /// <typeparam name="TResult">Тип результата функции</typeparam>
        /// <typeparam name="TValue">Тип параметра функции</typeparam>
        /// <param name="value">Значение, передаваемое в функцию</param>
        /// <param name="function">Проверяемая функция</param>
        /// <returns>Объект проверки исключения</returns>
        public FunctionChecker<TValue, TResult> Method<TValue, TResult>(TValue value, Func<TValue, TResult> function) => new(function, value);
    }

    #region Collection

    /// <param name="assert">Объект-помощник проверки</param>
    extension(Assert assert)
    {
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public DoubleCollectionChecker Collection(ICollection<double> ActualCollection) => new(ActualCollection);

        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public DoubleCollectionChecker Collection(double[] ActualCollection) => new(ActualCollection);

        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public DoubleCollectionChecker Collection(List<double> ActualCollection) => new(ActualCollection);

        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public DoubleReadOnlyCollectionChecker Collection(IReadOnlyCollection<double> ActualCollection) => new(ActualCollection);

        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public DoubleReadOnlyCollectionChecker Collection(ReadOnlyCollection<double> ActualCollection) => new(ActualCollection);

        /// <summary>Проверка двумерного массива вещественных чисел</summary>
        /// <param name="ActualArray">Проверяемый двумерный массив</param>
        /// <returns>Объект проверки</returns>
        public DoubleDimensionArrayChecker Collection(double[,] ActualArray) => new(ActualArray);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public CollectionChecker<T> Collection<T>(ICollection<T> ActualCollection) => new(ActualCollection);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public CollectionChecker<T> Collection<T>(T[] ActualCollection) => new(ActualCollection);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public CollectionChecker<T> Collection<T>(List<T> ActualCollection) => new(ActualCollection);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public ReadOnlyCollectionChecker<T> Collection<T>(IReadOnlyCollection<T> ActualCollection) => new(ActualCollection);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public ReadOnlyCollectionChecker<T> Collection<T>(ReadOnlyCollection<T> ActualCollection) => new(ActualCollection);
    }

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