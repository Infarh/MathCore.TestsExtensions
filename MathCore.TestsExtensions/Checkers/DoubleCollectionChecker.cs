using System;
using System.Collections.Generic;
using System.Linq;
using MathCore.Tests.Annotations;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки коллекций вещественных чисел</summary>
    public class DoubleCollectionChecker
    {
        /// <summary>Объект проверки равенства элементов вещественной коллекции чисел с заданной точностью</summary>
        public sealed class EqualityCheckerWithAccuracy : IDisposable
        {
            /// <summary>Проверяемая коллекция значение</summary>
            private readonly ICollection<double> _ActualValues;
            /// <summary>Значения, с которыми требуется провести сравнение</summary>
            private readonly ICollection<double> _ExpectedValues;
            /// <summary>Проверка на неравенство</summary>
            private readonly bool _Not;

            private bool _IsChecked;

            /// <summary>Инициализация нового объекта сравнения чисел с заданной точностью</summary>
            /// <param name="ActualValues">Проверяемая коллекция</param>
            /// <param name="ExpectedValues">Требуемые значения</param>
            /// <param name="Not">Проверять на неравенство</param>
            internal EqualityCheckerWithAccuracy(ICollection<double> ActualValues, double[] ExpectedValues, bool Not = false)
            {
                _ActualValues = ActualValues;
                _ExpectedValues = ExpectedValues;
                _Not = Not;
            }

            /// <summary>Проверка с задаваемой точностью</summary>
            /// <param name="Accuracy">Точность сравнения</param>
            /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
            public void WithAccuracy(double Accuracy, string Message = null)
            {
                _IsChecked = true;

                IEnumerator<double> expected_collection_enumerator = null;
                IEnumerator<double> actual_collection_enumerator = null;

                try
                {
                    expected_collection_enumerator = _ExpectedValues.GetEnumerator();
                    actual_collection_enumerator = _ActualValues.GetEnumerator();

                    var index = 0;
                    Service.CheckSeparator(ref Message);

                    if (_Not)
                        while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                        {
                            var expected = expected_collection_enumerator.Current;
                            var actual = actual_collection_enumerator.Current;
                            var delta = Math.Abs(expected - actual);
                            Assert.AreNotEqual(
                                expected, actual, Accuracy,
                                "{0}error[{1}]: ожидалось({2}), получено({3}), accuracy:{4}, err:{5:e3}; rel.err:{6}",
                                Message, index++, expected, actual, Accuracy, delta, delta / expected);
                        }
                    else
                        while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                        {
                            var expected = expected_collection_enumerator.Current;
                            var actual = actual_collection_enumerator.Current;
                            var delta = Math.Abs(expected - actual);
                            Assert.AreEqual(
                                expected, actual, Accuracy,
                                "{0}error[{1}]: ожидалось({2}), получено({3}), accuracy:{4}, err:{5:e3}; rel.err:{6}",
                                Message, index++, expected, actual, Accuracy, delta, delta / expected);
                        }
                }
                finally
                {
                    expected_collection_enumerator?.Dispose();
                    actual_collection_enumerator?.Dispose();
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Ожидание>")]
            void IDisposable.Dispose()
            {
                if (_IsChecked) return;
                Assert.Fail($"Проверка на {(_Not ? "не" : null)}равенство не выполнена");
            }
        }

        /// <summary>Проверяемая коллекция</summary>
        [NotNull] private readonly ICollection<double> _ActualCollection;

        /// <summary>Инициализация нового объекта проверки коллекции вещественных чисел</summary>
        /// <param name="ActualCollection"></param>
        internal DoubleCollectionChecker([NotNull] ICollection<double> ActualCollection) => _ActualCollection = ActualCollection;

        /// <summary>Проверка значений коллекции на равенство</summary>
        /// <param name="ExpectedValues">Ожидаемые значения</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public EqualityCheckerWithAccuracy ValuesAreEqualTo([NotNull] params double[] ExpectedValues)
        {
            Assert.That
               .Value(_ActualCollection.Count)
               .IsEqual(ExpectedValues.Length, $"Размер коллекции {_ActualCollection.Count} не совпадает с ожидаемым размером {ExpectedValues.Length}"); ;

            return new EqualityCheckerWithAccuracy(_ActualCollection, ExpectedValues);
        }

        /// <summary>Проверка значений коллекции на равенство</summary>
        /// <param name="ExpectedValues">Ожидаемые значения</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public EqualityCheckerWithAccuracy ValuesAreNotEqualTo([NotNull] params double[] ExpectedValues)
        {
            Assert.That
               .Value(_ActualCollection.Count)
               .IsEqual(ExpectedValues.Length, $"Размер коллекции {_ActualCollection.Count} не совпадает с ожидаемым размером {ExpectedValues.Length}"); ;

            return new EqualityCheckerWithAccuracy(_ActualCollection, ExpectedValues, Not: true);
        }

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
            Assert.That
               .Value(_ActualCollection.Count)
               .IsEqual(ExpectedCollection.Count, $"Размер коллекции {_ActualCollection.Count} не совпадает с ожидаемым размером {ExpectedCollection.Count}"); ;

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

        /// <summary>Позиционный критерий проверки элементов коллекции</summary>
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

        /// <summary>Максимальное значение в коллекции</summary>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Max() => Assert.That.Value(_ActualCollection.Max());

        /// <summary>Минимальное значение в коллекции</summary>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Min() => Assert.That.Value(_ActualCollection.Min());

        /// <summary>Среднее значение в коллекции</summary>
        /// <returns>Объект проверки вещественного значения</returns>
        public DoubleValueChecker Average() => Assert.That.Value(_ActualCollection.Average());
    }
}