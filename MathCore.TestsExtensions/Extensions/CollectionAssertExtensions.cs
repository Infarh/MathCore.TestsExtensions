using System.Collections.Generic;
using MathCore.Tests.Annotations;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта-помощника проверки <see cref="Assert.That"/></summary>
    public static class CollectionAssertExtensions
    {
        //[NotNull] public static CollectionAssertChecker Collection([NotNull] this CollectionAssert assert, ICollection ActualCollection) => new CollectionAssertChecker(ActualCollection);

        /// <param name="assert">Объект-помощник проверки</param>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        [NotNull]
        public static DoubleCollectionChecker Collection([NotNull] this CollectionAssert assert, ICollection<double> ActualCollection) => new DoubleCollectionChecker(ActualCollection);

        /// <summary>Проверка двумерного массива вещественных чисел</summary>
        /// <param name="assert">Объект-помощник проверки</param>
        /// <param name="ActualArray">Проверяемый двумерный массив</param>
        /// <returns>Объект проверки</returns>
        [NotNull]
        public static DoubleDimensionArrayChecker Collection([NotNull] this CollectionAssert assert, double[,] ActualArray) => new DoubleDimensionArrayChecker(ActualArray);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="assert">Объект-помощник проверки</param>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        [NotNull]
        public static CollectionChecker<T> Collection<T>([NotNull] this CollectionAssert assert, ICollection<T> ActualCollection) => new CollectionChecker<T>(ActualCollection);
    }
}