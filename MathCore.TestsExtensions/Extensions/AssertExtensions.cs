using System;
// ReSharper disable UnusedParameter.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Класс методов-расширений для объекта-помощника проверки <see cref="Assert.That"/></summary>
    public static class AssertExtensions
    {
        /// <summary>Проверка значения</summary>
        /// <typeparam name="T">ТИп проверяемого значения</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки</returns>
        public static ValueChecker<T> Value<T>(this Assert that, T ActualValue) => new ValueChecker<T>(ActualValue);

        /// <summary>Проверка вещественного значения</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки вещественных значений</returns>
        public static DoubleValueChecker Value(this Assert that, double ActualValue) => new DoubleValueChecker(ActualValue);

        /// <summary>Проверка целочисленного значения</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки целочисленных значений</returns>
        public static IntValueChecker Value(this Assert that, int ActualValue) => new IntValueChecker(ActualValue);

        /// <summary>Проверка действия</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="action">Проверяемое действие</param>
        /// <returns>Объект проверки исключения</returns>
        public static ActionChecker Method(this Assert that, Action action) => new ActionChecker(action);

        /// <summary>Проверка действия</summary>
        /// <typeparam name="TValue">Тип параметра действия</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="action">Проверяемое действие</param>
        /// <param name="value">Параметр</param>
        /// <returns>Объект проверки исключения</returns>
        public static ActionChecker<TValue> Method<TValue>(this Assert that, Action<TValue> action, TValue value) => new ActionChecker<TValue>(action, value);

        /// <summary>Проверка функции</summary>
        /// <typeparam name="TResult">Тип результата функции</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="function">Проверяемая функция</param>
        /// <returns>Объект проверки исключения</returns>
        public static FunctionChecker<TResult> Method<TResult>(this Assert that, Func<TResult> function) => new FunctionChecker<TResult>(function);

        /// <summary>Проверка функции</summary>
        /// <typeparam name="TResult">Тип результата функции</typeparam>
        /// <typeparam name="TValue">Тип параметра функции</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="function">Проверяемая функция</param>
        /// <param name="value">Значение, передаваемое в функцию</param>
        /// <returns>Объект проверки исключения</returns>
        public static FunctionChecker<TValue, TResult> Method<TValue, TResult>(this Assert that, Func<TValue, TResult> function, TValue value) => new FunctionChecker<TValue, TResult>(function, value);
    }
}
