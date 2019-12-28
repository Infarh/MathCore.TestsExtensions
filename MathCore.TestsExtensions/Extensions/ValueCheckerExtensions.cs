using System;
using System.Collections.Generic;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки значения</summary>
    public static class ValueCheckerExtensions
    {
        /// <summary>Проверка значения как коллекции элементов</summary>
        /// <param name="Checker">Объект проверки одиночного значения</param>
        /// <typeparam name="T">Тип проверяемого значения, которое является коллекцией объектов типа <typeparamref name="TItem"/></typeparam>
        /// <typeparam name="TItem">Тип элементов проверяемой коллекции</typeparam>
        /// <returns>Объект проверки коллекции</returns>
        [NotNull]
        public static CollectionChecker<TItem> Are<T, TItem>([NotNull] this ValueChecker<T> Checker) where T : ICollection<TItem> => new CollectionChecker<TItem>(Checker.ActualValue);
    }
}
