using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки дочернего значения</summary>
    /// <typeparam name="TValue">Тип дочернего значения</typeparam>
    /// <typeparam name="TBaseValue">Тип базового значения</typeparam>
    public sealed class NestedValueChecker<TValue, TBaseValue> : ValueChecker<TValue>
    {
        /// <summary>Базовый объект проверки значения</summary>
        public ValueChecker<TBaseValue> BaseValue { get; }

        /// <summary>Инициализация нового объекта проверки вложенного значения</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="BaseChecker">Базовый объект проверки значения</param>
        internal NestedValueChecker(TValue ActualValue, ValueChecker<TBaseValue> BaseChecker) : base(ActualValue) => BaseValue = BaseChecker;

        /// <summary>Проверка дочернего значения</summary>
        /// <param name="Checker">Метод проверки дочернего значения</param>
        /// <returns>Объект проверки базового значения</returns>
        public ValueChecker<TBaseValue> Check(Action<ValueChecker<TValue>> Checker)
        {
            Checker(this);
            return BaseValue;
        }

        /// <summary>Проверка значения на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public ValueChecker<TBaseValue> CheckEquals(TValue ExpectedValue, string Message = null)
        {
            IsEqual(ExpectedValue, Message);
            return BaseValue;
        }

        /// <summary>Проверка значение на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Comparer">Объект сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public ValueChecker<TBaseValue> CheckEquals(TValue ExpectedValue, IEqualityComparer<TValue> Comparer, string Message = null)
        {
            IsEqual(ExpectedValue, Comparer, Message);
            return BaseValue;
        }

        /// <summary>Проверка значение на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Comparer">Метод сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public ValueChecker<TBaseValue> CheckEquals(TValue ExpectedValue, EqualityComparer Comparer, string Message = null)
        {
            IsEqual(ExpectedValue, Comparer, Message);
            return BaseValue;
        }
    }
}