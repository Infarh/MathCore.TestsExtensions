using System;
using System.Linq;
using System.Collections.Generic;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки коллекции</summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    public class CollectionChecker<T>
    {
        /// <summary>Проверяемая коллекция</summary>
        public ICollection<T> ActualValue { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public CollectionAssert And => CollectionAssert.That;

        /// <summary>Инициализация нового объекта проверки коллекции</summary>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        internal CollectionChecker(ICollection<T> ActualCollection) => ActualValue = ActualCollection;

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Исходный объект проверки значений</returns>
        public CollectionChecker<T> IsEqualTo([NotNull] ICollection<T> ExpectedCollection, string Message = null)
        {
            Assert.That
               .Value(ActualValue.Count)
               .IsEqual(ExpectedCollection.Count, $"Размер коллекции {ActualValue.Count} не совпадает с ожидаемым размером {ExpectedCollection.Count}");

            IEnumerator<T> expected_collection_enumerator = null;
            IEnumerator<T> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedCollection.GetEnumerator();
                actual_collection_enumerator = ActualValue.GetEnumerator();

                var index = 0;
                Service.CheckSeparator(ref Message);
                while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                {
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    Assert.AreEqual(expected, actual, "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index++, expected, actual);
                }
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }

            return this;
        }

        /// <summary>Метод сравнения значений элементов коллекции</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
        public delegate bool EqualityComparer(T ExpectedValue, T ActualValue);

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Comparer">Метод проверки элементов коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public CollectionChecker<T> IsEqualTo([NotNull] ICollection<T> ExpectedCollection, EqualityComparer Comparer, string Message = null)
        {
            Assert.That
               .Value(ActualValue.Count)
               .IsEqual(ExpectedCollection.Count, $"Размер коллекции {ActualValue.Count} не совпадает с ожидаемым размером {ExpectedCollection.Count}");

            IEnumerator<T> expected_collection_enumerator = null;
            IEnumerator<T> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedCollection.GetEnumerator();
                actual_collection_enumerator = ActualValue.GetEnumerator();

                var index = 0;
                Service.CheckSeparator(ref Message);
                while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                {
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    Assert.IsTrue(Comparer(expected, actual), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index++, expected, actual);
                }
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }
            return this;
        }

        /// <summary>Метод сравнения значений элементов коллекции</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="Index">Индекс проверяемого элемента</param>
        /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
        public delegate bool EqualityPositionalComparer(T ExpectedValue, T ActualValue, int Index);

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Comparer">Метод проверки элементов коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public CollectionChecker<T> IsEqualTo([NotNull] ICollection<T> ExpectedCollection, EqualityPositionalComparer Comparer, string Message = null)
        {
            Assert.That.Value(ActualValue.Count).IsEqual(ExpectedCollection.Count);

            IEnumerator<T> expected_collection_enumerator = null;
            IEnumerator<T> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedCollection.GetEnumerator();
                actual_collection_enumerator = ActualValue.GetEnumerator();

                var index = 0;
                Service.CheckSeparator(ref Message);
                while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                {
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index++, expected, actual);
                }
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }
            return this;
        }

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Comparer">Объект проверки элементов коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection<T> ExpectedCollection, IEqualityComparer<T> Comparer, string Message = null)
        {
            Assert.That.Value(ActualValue.Count).IsEqual(ExpectedCollection.Count);

            IEnumerator<T> expected_collection_enumerator = null;
            IEnumerator<T> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedCollection.GetEnumerator();
                actual_collection_enumerator = ActualValue.GetEnumerator();

                var index = 0;
                Service.CheckSeparator(ref Message);
                while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                {
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    Assert.IsTrue(Comparer.Equals(expected, actual),
                        "{0}error[{1}]: ожидалось({2}), получено({3})",
                        Message, index++, expected, actual);
                }
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }
        }

        /// <summary>Проверка коллекции на совпадение с указанным набором значений</summary>
        /// <param name="items">Значения, из которых составлена коллекция</param>
        /// <returns>Исходный объект проверки значений</returns>
        public CollectionChecker<T> IsEqualTo(params T[] items)
        {
            CountEquals(items.Length);

            var index = 0;
            foreach (var value in ActualValue)
                Assert.That.Value(value).IsEqual(items[index], $"item[{index++}]");

            return this;
        }

        /// <summary>Проверка коллекции на совпадение с указанным набором значений</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <param name="items">Значения, из которых составлена коллекция</param>
        /// <returns>Исходный объект проверки значений</returns>
        public CollectionChecker<T> IsEqualTo(string Message, params T[] items)
        {
            CountEquals(items.Length);

            var index = 0;
            foreach (var value in ActualValue)
                Assert.That.Value(value).IsEqual(items[index], $"item[{index++}]{Message}");

            return this;
        }

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedItems">Ожидаемый набор элементов</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Исходный объект проверки значений</returns>
        public CollectionChecker<T> IsEqualTo(IEnumerable<T> ExpectedItems, string Message = null)
        {
            IEnumerator<T> expected_collection_enumerator = null;
            IEnumerator<T> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedItems.GetEnumerator();
                actual_collection_enumerator = ActualValue.GetEnumerator();

                var index = 0;
                Service.CheckSeparator(ref Message);
                bool actual_move_next, expected_move_next;
                while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
                {
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    Assert.AreEqual(expected, actual, "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index++, expected, actual);
                }
                if (actual_move_next != expected_move_next)
                    throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }

            return this;
        }

        /// <summary>Првоерка на соответствие размера коллекции ожидаемому значению</summary>
        /// <param name="ExpectedCount">Ожидаемый размер коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Исходный объект проверки значений</returns>
        public CollectionChecker<T> CountEquals(int ExpectedCount, string Message = null)
        {
            var actual_count = ActualValue.Count;
            if(actual_count != ExpectedCount)
                throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {actual_count} не соответствует ожидаемому {ExpectedCount}");
            return this;
        }

        /// <summary>Максимальное значение в коллекции</summary>
        /// <param name="Selector">Метод оценки элемента коллекции</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Max(Func<T, double> Selector) => Assert.That.Value(ActualValue.Max(Selector));

        /// <summary>Минимальное значение в коллекции</summary>
        /// <param name="Selector">Метод оценки элемента коллекции</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Min(Func<T, double> Selector) => Assert.That.Value(ActualValue.Min(Selector));

        /// <summary>Среднее значение в коллекции</summary>
        /// <param name="Selector">Метод оценки элемента коллекции</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Average(Func<T, double> Selector) => Assert.That.Value(ActualValue.Average(Selector));

        /// <summary>Проверка, что коллекция содержит указанный элемент</summary>
        /// <param name="item">Элемент, который должен быть найден в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public CollectionChecker<T> Contains(T item, string Message = null)
        {
            Assert.IsTrue(ActualValue.Contains(item), "{0}Коллекция не содержит элемент {1}", Message.AddSeparator(), item);
            return this;
        }

        /// <summary>Проверка, что коллекция содержит элемент, удовлетворяющий указанному критерию</summary>
        /// <param name="Predicate">Критерий проверки наличия элемента в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
        public CollectionChecker<T> Contains(Func<T, bool> Predicate, string Message = null)
        {
            Assert.IsTrue(ActualValue.Any(Predicate), "{0}Коллекция не содержит элемент, удовлетворяющий заданным параметрам", Message.AddSeparator());
            return this;
        }

        /// <summary>Проверка, что указанного элемента нет в коллекции</summary>
        /// <param name="item">Элемент, которого не должно быть в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public CollectionChecker<T> NotContains(T item, string Message = null)
        {
            Assert.IsTrue(!ActualValue.Contains(item), "{0}Коллекция содержит элемент {1}", Message.AddSeparator(), item);
            return this;
        }

        /// <summary>Проверка, что коллекция НЕ содержит элемент, удовлетворяющий указанному критерию</summary>
        /// <param name="Predicate">Критерий проверки наличия элемента в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
        public CollectionChecker<T> NotContains(Func<T, bool> Predicate, string Message = null)
        {
            Assert.IsFalse(ActualValue.Any(Predicate), "{0}Коллекция не содержит элемент, удовлетворяющий заданным параметрам", Message.AddSeparator());
            return this;
        }

        /// <summary>Выполнение проверки элементов коллекции</summary>
        /// <param name="Check">Метод проверки элементов коллекции</param>
        /// <returns>Исходный объект проверки коллекции</returns>
        public CollectionChecker<T> AllItems(Action<ValueChecker<T>> Check)
        {
            foreach (var value in ActualValue.Select(value => new ValueChecker<T>(value)))
                Check(value);
            return this;
        }

        /// <summary>Выполнение проверки элементов коллекции</summary>
        /// <param name="Check">Метод проверки элементов коллекции с учётом порядкового номера</param>
        /// <returns>Исходный объект проверки коллекции</returns>
        public CollectionChecker<T> AllItems(Action<ValueChecker<T>, int> Check)
        {
            var i = 0;
            foreach (var value in ActualValue.Select(value => new ValueChecker<T>(value)))
                Check(value, i++);
            return this;
        }

        /// <summary>Объект проверки числа элементов</summary>
        public ValueChecker<int> ItemsCount => new ValueChecker<int>(ActualValue.Count);

        /// <summary>Проверка на соответствие размера коллекции ожидаемому значению</summary>
        /// <param name="ExpectedCount">Ожидаемое значение размера коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
        /// <returns>Объект проверки коллекции</returns>
        public CollectionChecker<T> IsItemsCount(int ExpectedCount, string Message = null)
        {
            var count = ActualValue.Count;
            if(count != ExpectedCount)
                throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {count} не совпадает с ожидаемым {ExpectedCount}");
            return this;
        }

        /// <summary>Проверка - коллекция должна быть пуста</summary>
        /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
        /// <returns>Объект проверки коллекции</returns>
        public CollectionChecker<T> IsEmpty(string Message = null)
        {
            var count = ActualValue.Count;
            if (count != 0)
                throw new AssertFailedException($"{Message.AddSeparator()}Число элементов коллекции {count} не равно 0 - коллекция не пуста");
            return this;
        }

        /// <summary>Проверка - коллекция должна быть не пуста</summary>
        /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
        /// <returns>Объект проверки коллекции</returns>
        public CollectionChecker<T> IsNotEmpty(string Message = null)
        {
            if (ActualValue.Count == 0)
                throw new AssertFailedException($"{Message.AddSeparator()}Коллекция пуста");
            return this;
        }

        /// <summary>Проверка - коллекция должна содержать один единственный элемент</summary>
        /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
        /// <returns>Объект проверки коллекции</returns>
        public CollectionChecker<T> IsSingleItem(string Message = null)
        {
            var count = ActualValue.Count;
            if (count != 1)
                throw new AssertFailedException($"{Message.AddSeparator()}Число элементов коллекции {count} не равно 1 - коллекция содержит не один единственный элемент");
            return this;
        }
    }
}