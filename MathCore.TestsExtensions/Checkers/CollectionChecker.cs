using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

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
        public void IsEqualTo([NotNull] ICollection<T> ExpectedCollection, string Message = null)
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
        public void IsEqualTo([NotNull] ICollection<T> ExpectedCollection, EqualityComparer Comparer, string Message = null)
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
        public void IsEqualTo([NotNull] ICollection<T> ExpectedCollection, EqualityPositionalComparer Comparer, string Message = null)
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
        public void Contains(T item, string Message = null) => Assert.IsTrue(ActualValue.Contains(item), "{0}Коллекция не содержит элемент {1}", Message.AddSeparator(), item);

        /// <summary>Проверка, что коллекция содержит элемент, удовлетворяющий указанному критерию</summary>
        /// <param name="Predicate">Критерий проверки наличия элемента в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
        public void Contains(Func<T, bool> Predicate, string Message = null) => Assert.IsTrue(ActualValue.Any(Predicate), "{0}Коллекция не содержит элемент, удовлетворяющий заданным параметрам", Message.AddSeparator());

        /// <summary>Проверка, что указанного элемента нет в коллекции</summary>
        /// <param name="item">Элемент, которого не должно быть в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void NotContains(T item, string Message = null) => Assert.IsTrue(!ActualValue.Contains(item), "{0}Коллекция содержит элемент {1}", Message.AddSeparator(), item);

        /// <summary>Проверка, что коллекция НЕ содержит элемент, удовлетворяющий указанному критерию</summary>
        /// <param name="Predicate">Критерий проверки наличия элемента в коллекции</param>
        /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
        public void NotContains(Func<T, bool> Predicate, string Message = null) => Assert.IsFalse(ActualValue.Any(Predicate), "{0}Коллекция не содержит элемент, удовлетворяющий заданным параметрам", Message.AddSeparator());

        /// <summary>Выполнение проверки для всех элементов коллекции</summary>
        /// <param name="Check">Метод проверки значения</param>
        /// <returns>Исходный объект проверки коллекции</returns>
        public CollectionChecker<T> All(Action<ValueChecker<T>> Check)
        {
            foreach (var item in ActualValue)
                Check(new ValueChecker<T>(item));
            return this;
        }

        /// <summary>Выполнение проверки для всех элементов коллекции</summary>
        /// <param name="Check">Позиционный метод проверки значения</param>
        /// <returns>Исходный объект проверки коллекции</returns>
        public CollectionChecker<T> All(Action<ValueChecker<T>, int> Check)
        {
            var index = 0;
            foreach (var item in ActualValue)
                Check(new ValueChecker<T>(item), index++);
            return this;
        }
    }

    /// <summary>Объект проверки коллекции</summary>
    public class CollectionChecker
    {
        /// <summary>Проверяемая коллекция</summary>
        private readonly ICollection _ActualCollection;

        /// <summary>Инициализация нового объекта проверки коллекции</summary>
        /// <param name="ActualCollection">Проверяемая коллекция</param>
        internal CollectionChecker(ICollection ActualCollection) => _ActualCollection = ActualCollection;

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection ExpectedCollection, string Message = null)
        {
            Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count);

            var expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            var actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            Service.CheckSeparator(ref Message);
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                Assert.AreEqual(expected, actual, "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index++, expected, actual);
            }

        }

        /// <summary>Метод сравнения значений элементов коллекции</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
        public delegate bool EqualityComparer(object ExpectedValue, object ActualValue);

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Comparer">Метод сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection ExpectedCollection, EqualityComparer Comparer, string Message = null)
        {
            Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count);

            var expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            var actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            Service.CheckSeparator(ref Message);
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                Assert.IsTrue(
                    Comparer(expected, actual),
                    "{0}error[{1}]: ожидалось({2}), получено({3})",
                    Message, index++, expected, actual);
            }

        }

        /// <summary>Метод сравнения значений элементов коллекции</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="Index">Индекс проверяемого элемента</param>
        /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
        public delegate bool EqualityPositionalComparer(object ExpectedValue, object ActualValue, int Index);

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Comparer">Метод сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection ExpectedCollection, EqualityPositionalComparer Comparer, string Message = null)
        {
            Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count);

            var expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            var actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            Service.CheckSeparator(ref Message);
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index++, expected, actual);
            }

        }

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Comparer">Объект сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection ExpectedCollection, IEqualityComparer Comparer, string Message = null)
        {
            Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count);

            var expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            var actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            Service.CheckSeparator(ref Message);
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                Assert.IsTrue(
                    Comparer.Equals(expected, actual),
                    "{0}error[{1}]: ожидалось({2}), получено({3})",
                    Message, index++, expected, actual);
            }

        }

        /// <summary>Минимальное значение в коллекции</summary>
        /// <param name="Selector">Метод оценки элемента коллекции</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Max(Func<object, double> Selector) => Assert.That.Value(_ActualCollection.Cast<object>().Max(Selector));

        /// <summary>Минимальное значение в коллекции</summary>
        /// <param name="Selector">Метод оценки элемента коллекции</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Min(Func<object, double> Selector) => Assert.That.Value(_ActualCollection.Cast<object>().Min(Selector));

        /// <summary>Среднее значение в коллекции</summary>
        /// <param name="Selector">Метод оценки элемента коллекции</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Average(Func<object, double> Selector) => Assert.That.Value(_ActualCollection.Cast<object>().Average(Selector));
    }
}