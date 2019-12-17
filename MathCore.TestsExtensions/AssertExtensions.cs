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
        public static AssertEqualsChecker<T> Value<T>(this Assert that, T ActualValue) => new AssertEqualsChecker<T>(ActualValue);

        /// <summary>Проверка вещественного значения</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки вещественных значений</returns>
        public static AssertDoubleEqualsChecker Value(this Assert that, double ActualValue) => new AssertDoubleEqualsChecker(ActualValue);

        /// <summary>Проверка целочисленного значения</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки целочисленных значений</returns>
        public static AssertIntEqualsChecker Value(this Assert that, int ActualValue) => new AssertIntEqualsChecker(ActualValue);

        /// <summary>Проверка действия</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="action">Проверяемое действие</param>
        /// <returns>Объект проверки исключения</returns>
        public static AssertActionChecker Method(this Assert that, Action action) => new AssertActionChecker(action);

        /// <summary>Проверка действия</summary>
        /// <typeparam name="TValue">Тип параметра действия</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="action">Проверяемое действие</param>
        /// <param name="value">Параметр</param>
        /// <returns>Объект проверки исключения</returns>
        public static AssertActionChecker<TValue> Method<TValue>(this Assert that, Action<TValue> action, TValue value) => new AssertActionChecker<TValue>(action, value);

        /// <summary>Проверка функции</summary>
        /// <typeparam name="TResult">Тип результата функции</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="function">Проверяемая функция</param>
        /// <returns>Объект проверки исключения</returns>
        public static AssertFunctionChecker<TResult> Method<TResult>(this Assert that, Func<TResult> function) => new AssertFunctionChecker<TResult>(function);

        /// <summary>Проверка функции</summary>
        /// <typeparam name="TResult">Тип результата функции</typeparam>
        /// <typeparam name="TValue">Тип параметра функции</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="function">Проверяемая функция</param>
        /// <param name="value">Значение, передаваемое в функцию</param>
        /// <returns>Объект проверки исключения</returns>
        public static AssertFunctionChecker<TValue, TResult> Method<TValue, TResult>(this Assert that, Func<TValue, TResult> function, TValue value) => new AssertFunctionChecker<TValue, TResult>(function, value);
    }
}
