using MathCore.Tests.Annotations;
using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки двумерного массива вещественных чисел</summary>
    public class DoubleDimensionArrayChecker
    {
        /// <summary>Проверяемый массив</summary>
        private readonly double[,] _ActualArray;

        /// <summary>Инициализация нового объекта проверки</summary>
        /// <param name="ActualArray">Проверяемый массив</param>
        internal DoubleDimensionArrayChecker(double[,] ActualArray) => _ActualArray = ActualArray;

        /// <summary>По размеру и поэлементно массив эквивалентен ожидаемому массиву</summary>
        /// <param name="ExpectedArray">Ожидаемая коллекция значений</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>

        public void IsEqualTo(double[,] ExpectedArray, string Message = null)
        {
            Service.CheckSeparator(ref Message);
            Assert.AreEqual(ExpectedArray.GetLength(0), _ActualArray.GetLength(0), "{0}{1}", Message, "Размеры массивов не совпадают");
            Assert.AreEqual(ExpectedArray.GetLength(1), _ActualArray.GetLength(1), "{0}{1}", Message, "Размеры массивов не совпадают");

            for (var i = 0; i < _ActualArray.GetLength(0); i++)
                for (var j = 0; j < _ActualArray.GetLength(1); j++)
                {
                    var expected = ExpectedArray[i, j];
                    var actual = _ActualArray[i, j];
                    Assert.AreEqual(expected, actual,
                        "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; error:{5:e3}; rel_error:{6}", 
                        Message, i, j, expected, actual, Math.Abs(expected - actual), Math.Abs(expected - actual) / expected);
                }
        }

        /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
        /// <param name="ExpectedArray">Ожидаемая коллекция значений</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqualTo(double[,] ExpectedArray, double Accuracy, string Message = null)
        {
            Service.CheckSeparator(ref Message);
            Assert.AreEqual(ExpectedArray.GetLength(0), _ActualArray.GetLength(0), "{0}{1}", Message, "Размеры массивов не совпадают");
            Assert.AreEqual(ExpectedArray.GetLength(1), _ActualArray.GetLength(1), "{0}{1}", Message, "Размеры массивов не совпадают");

            for (var i = 0; i < _ActualArray.GetLength(0); i++)
                for (var j = 0; j < _ActualArray.GetLength(1); j++)
                {
                    var expected = ExpectedArray[i, j];
                    var actual = _ActualArray[i, j];
                    Assert.AreEqual(expected, actual, Accuracy,
                        "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; delta:{5}; error:{6:e2}; rel_error:{7}", 
                        Message, i, j, expected, actual, Accuracy, Math.Abs(expected - actual), Math.Abs(expected - actual) / expected);
                }
        }

        /// <summary>Все элементы массива равны ожидаемому значению</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreEqualTo(double ExpectedValue, string Message = null)
        {
            Service.CheckSeparator(ref Message);
            for (var i = 0; i < _ActualArray.GetLength(0); i++)
                for (var j = 0; j < _ActualArray.GetLength(1); j++)
                {
                    var actual = _ActualArray[i, j];
                    Assert.AreEqual(ExpectedValue, actual,
                        "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; error:{5:e3}; rel_error:{6}",
                        Message, i, j, ExpectedValue, actual, Math.Abs(ExpectedValue - actual), Math.Abs(ExpectedValue - actual) / ExpectedValue);
                }
        }

        /// <summary>Все элементы массива равны ожидаемому значению</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreEqualTo(double ExpectedValue, double Accuracy, string Message = null)
        {
            Service.CheckSeparator(ref Message);
            for (var i = 0; i < _ActualArray.GetLength(0); i++)
                for (var j = 0; j < _ActualArray.GetLength(1); j++)
                {
                    var actual = _ActualArray[i, j];
                    Assert.AreEqual(ExpectedValue, actual, Accuracy,
                        "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; delta:{5}; error:{6:e2}; rel_error:{7}",
                        Message, i, j, ExpectedValue, actual, Accuracy, Math.Abs(ExpectedValue - actual), Math.Abs(ExpectedValue - actual) / ExpectedValue);
                }
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
            Service.CheckSeparator(ref Message);
            for (var i = 0; i < _ActualArray.GetLength(0); i++)
                for (var j = 0; j < _ActualArray.GetLength(1); j++)
                {
                    var actual = _ActualArray[i, j];
                    Assert.IsTrue(Condition(actual), "{0}err[{1},{2}]:{3}", Message, i, j, actual);
                }
        }

        /// <summary>Позиционный критерий проверки элементов коллекции</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="i">Индекс строки</param>
        /// <param name="j">Индекс столбца</param>
        /// <returns>Истина, если элемент соответствует критерию проверки</returns>
        public delegate bool PositionElementChecker(double ActualValue, int i, int j);

        /// <summary>Все элементы коллекции удовлетворяют условию</summary>
        /// <param name="Condition">Условие проверки всех элементов</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void ElementsAreSatisfyCondition([NotNull] PositionElementChecker Condition, string Message = null)
        {
            Service.CheckSeparator(ref Message);
            for (var i = 0; i < _ActualArray.GetLength(0); i++)
                for (var j = 0; j < _ActualArray.GetLength(1); j++)
                {
                    var actual = _ActualArray[i, j];
                    Assert.IsTrue(Condition(actual, i, j), "{0}err[{1},{2}]:{3}", Message, i, j, actual);
                }
        }
    }
}