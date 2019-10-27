using System;
using System.Collections.Generic;
using MathCore.Tests.Annotations;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки коллекций вещественных чисел</summary>
    public class DoubleCollectionAssertChecker
    {
        /// <summary>Проверяемая коллекция</summary>
        [NotNull] private readonly ICollection<double> _ActualCollection;

        /// <summary>Инициализация нового объекта проверки коллекции вещественных чисел</summary>
        /// <param name="ActualCollection"></param>
        internal DoubleCollectionAssertChecker([NotNull] ICollection<double> ActualCollection) => _ActualCollection = ActualCollection;

        /// <summary>Проверка на эквивалентность с задаваемым набором значений</summary>
        /// <param name="ExpectedValues">Ожидаемые значения коллекции</param>
        public void ValuesAreEqual([NotNull] params double[] ExpectedValues) => IsEqualTo(ExpectedValues);

        /// <summary>Проверка на эквивалентность с задаваемым набором значений</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <param name="ExpectedValues">Ожидаемые значения коллекции</param>
        public void ValuesAreEqual(string Message, [NotNull] params double[] ExpectedValues) => IsEqualTo(ExpectedValues);

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection<double> ExpectedCollection, string Message = null)
        {
            Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count);

            IEnumerator<double> expected_collection_enumerator = null;
            IEnumerator<double> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedCollection.GetEnumerator();
                actual_collection_enumerator = _ActualCollection.GetEnumerator();

                var index = 0;
                Service.CheckSeparator(ref Message);
                while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                {
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    var delta = Math.Abs(expected - actual);
                    Assert.AreEqual(
                        expected, actual,
                        "{0}error[{1}]: ожидалось({2}), получено({3}), err:{4:e3}; rel.err:{5}",
                        Message, index++, expected, actual, delta, delta / expected);
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
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo([NotNull] ICollection<double> ExpectedCollection, double Accuracy, string Message = null)
        {
            Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count, "Размеры коллекций не совмадают");

            IEnumerator<double> expected_collection_enumerator = null;
            IEnumerator<double> actual_collection_enumerator = null;
            try
            {
                expected_collection_enumerator = ExpectedCollection.GetEnumerator();
                actual_collection_enumerator = _ActualCollection.GetEnumerator();

                var index = -1;
                Service.CheckSeparator(ref Message);
                while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                {
                    index++;
                    var expected = expected_collection_enumerator.Current;
                    var actual = actual_collection_enumerator.Current;
                    var delta = Math.Abs(expected - actual);
                    Assert.AreEqual(
                        expected, actual, Accuracy,
                        "{0}error[{1}]: ожидалось({2}), получено({3}), eps:{4}, err:{5:e3}; rel.err:{6}",
                        Message, index, expected, actual, Accuracy, delta, delta / expected);
                }
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }
        }

        /// <summary>Все элементы коллекции равны заданному значению</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreEqualTo(double ExpectedValue, string Message = null)
        {
            var index = 0;
            foreach (var actual_value in _ActualCollection)
                Assert.AreEqual(ExpectedValue, actual_value, 
                    "{0}error[{1}]:{2:e2}", Message.AddSeparator(), index++, Math.Abs(ExpectedValue - actual_value));
        }

        /// <summary>Все элементы коллекции равны заданному значению</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy"></param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreEqualTo(double ExpectedValue, double Accuracy, string Message = null)
        {
            var index = 0;
            foreach (var actual_value in _ActualCollection)
                Assert.AreEqual(ExpectedValue, actual_value, Accuracy, 
                    "{0}error[{1}]:{2:e2}, eps:{3}", Message.AddSeparator(), index++, Math.Abs(ExpectedValue - actual_value), Accuracy);
        }

        /// <summary>Критерий проверки элементов коллекции</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Истина, если элемент соответствует критерию проверки</returns>
        public delegate bool ElementChecker(double ActualValue);

        /// <summary>Все элементы коллекции удовлетворяют условию</summary>
        /// <param name="Condition">Условие проверки всех элементов</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreSatisfyCondition([NotNull] ElementChecker Condition, string Message = null)
        {
            var index = 0;
            Service.CheckSeparator(ref Message);
            foreach (var actual_value in _ActualCollection) 
                Assert.IsTrue(Condition(actual_value), "{0}err.value[{1}]:{2}", Message, index++, actual_value);
        }

        /// <summary>Позиционынй критерий проверки элементов коллекции</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="ItemIndex">Индекс проверяемого элемента в коллекции</param>
        /// <returns>Истина, если элемент соответствует критерию проверки</returns>
        public delegate bool PositionElementChecker(double ActualValue, int ItemIndex);

        /// <summary>Все элементы коллекции удовлетворяют условию</summary>
        /// <param name="Condition">Условие проверки всех элементов</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreSatisfyCondition([NotNull] PositionElementChecker Condition, string Message = null)
        {
            var index = 0;
            Service.CheckSeparator(ref Message);
            foreach (var actual_value in _ActualCollection)
                Assert.IsTrue(Condition(actual_value, index), "{0}err.value[{1}]:{2}", Message, index++, actual_value);
        }
    }
}