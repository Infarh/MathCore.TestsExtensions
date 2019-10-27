using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта-помощника проверки <see cref="Assert.That"/></summary>
    public static class CollectionAssertExtensions
    {
        //public static CollectionAssertChecker Collection(this CollectionAssert assert, ICollection ActualCollection) => new CollectionAssertChecker(ActualCollection);

        /// <param name="assert">Объект-помощник проверки</param>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public static DoubleCollectionAssertChecker Collection(this CollectionAssert assert, ICollection<double> ActualCollection) => new DoubleCollectionAssertChecker(ActualCollection);

        /// <summary>Проверка двумерного массива вещественных чисел</summary>
        /// <param name="assert">Объект-помощник проверки</param>
        /// <param name="ActualArray">Проверяемый двумерный массив</param>
        /// <returns>Объект проверки</returns>
        public static DoubleDemensionArrayAssertChecker Collection(this CollectionAssert assert, double[,] ActualArray) => new DoubleDemensionArrayAssertChecker(ActualArray);

        /// <summary>Проверка коллекции</summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="assert">Объект-помощник проверки</param>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        /// <returns>Объект проверки</returns>
        public static CollectionAssertChecker<T> Collection<T>(this CollectionAssert assert, ICollection<T> ActualCollection) => new CollectionAssertChecker<T>(ActualCollection);
    }
}