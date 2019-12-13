using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки дочернего значения</summary>
    /// <typeparam name="TValue">Тип дочернего значения</typeparam>
    /// <typeparam name="TBaseValue">Тип  базового значения</typeparam>
    public sealed class ChildAssertEqualsChecker<TValue, TBaseValue> : AssertEqualsChecker<TValue>
    {
        private readonly AssertEqualsChecker<TBaseValue> _BaseChecker;

        internal ChildAssertEqualsChecker(TValue ActualValue, AssertEqualsChecker<TBaseValue> BaseChecker) : base(ActualValue) => _BaseChecker = BaseChecker;

        /// <summary>Проверка дочернего значения</summary>
        /// <param name="Checker">Метод проверки дочернего значения</param>
        /// <returns>Объект проверки базового значения</returns>
        public AssertEqualsChecker<TBaseValue> Check(Action<AssertEqualsChecker<TValue>> Checker)
        {
            Checker(this);
            return _BaseChecker;
        }
    }
}