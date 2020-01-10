using System.Collections.Generic;
// ReSharper disable UnusedType.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки перечислений</summary>
    /// <typeparam name="T">Тип элементов перечисления</typeparam>
    public sealed class EnumerableChecker<T> : ValueChecker<IEnumerable<T>>
    {
        /// <summary>Инициализация нового объекта <see cref="EnumerableChecker{T}"/></summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        internal EnumerableChecker(IEnumerable<T> ActualValue) : base(ActualValue) { }
    }
}
