using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки дочернего значения</summary>
    /// <typeparam name="TValue">Тип дочернего значения</typeparam>
    /// <typeparam name="TBaseValue">Тип базового значения</typeparam>
    public sealed class NestedValueChecker<TValue, TBaseValue> : ValueChecker<TValue>
    {
        /// <summary>Базовый объект проверки значения</summary>
        private readonly ValueChecker<TBaseValue> _BaseChecker;

        /// <summary>Инициализация нового объекта проверки вложенного значения</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="BaseChecker">Базовый объект проверки значения</param>
        internal NestedValueChecker(TValue ActualValue, ValueChecker<TBaseValue> BaseChecker) : base(ActualValue) => _BaseChecker = BaseChecker;

        /// <summary>Проверка дочернего значения</summary>
        /// <param name="Checker">Метод проверки дочернего значения</param>
        /// <returns>Объект проверки базового значения</returns>
        public ValueChecker<TBaseValue> Check(Action<ValueChecker<TValue>> Checker)
        {
            Checker(this);
            return _BaseChecker;
        }
    }
}