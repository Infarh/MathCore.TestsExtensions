using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки дочернего значения коллекции</summary>
    /// <typeparam name="TValue">Тип элементов дочерней коллекции</typeparam>
    /// <typeparam name="TBaseValue">Тип базового значения</typeparam>
    public sealed class NestedCollectionValueChecker<TValue, TBaseValue> : CollectionChecker<TValue>
    {
        /// <summary>Базовый объект проверки значения</summary>
        private readonly ValueChecker<TBaseValue> _BaseChecker;

        /// <summary>Инициализация нового объекта проверки дочернего значения коллекции</summary>
        /// <param name="ActualCollection">Проверяемое значение коллекции</param>
        /// <param name="BaseChecker">Базовый объект проверки значения</param>
        internal NestedCollectionValueChecker(System.Collections.Generic.ICollection<TValue> ActualCollection, ValueChecker<TBaseValue> BaseChecker) : base(ActualCollection) => _BaseChecker = BaseChecker;

        /// <summary>Проверка дочернего значения коллекции</summary>
        /// <param name="Checker">Метод проверки дочернего значения коллекции</param>
        /// <returns>Объект проверки базового значения</returns>
        public ValueChecker<TBaseValue> Check(Action<CollectionChecker<TValue>> Checker)
        {
            Checker(this);
            return _BaseChecker;
        }
    }
}