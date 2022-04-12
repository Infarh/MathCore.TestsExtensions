using System.Globalization;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Объект проверки двумерного массива вещественных чисел</summary>
public class DoubleDimensionArrayChecker
{
    /// <summary>Проверяемый массив</summary>
    public double[,] ActualValue { get; }

    /// <summary>Инициализация нового объекта проверки</summary>
    /// <param name="ActualArray">Проверяемый массив</param>
    internal DoubleDimensionArrayChecker(double[,] ActualArray) => ActualValue = ActualArray;

    /// <summary>По размеру и поэлементно массив эквивалентен ожидаемому массиву</summary>
    /// <param name="ExpectedArray">Ожидаемая коллекция значений</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>

    public DoubleDimensionArrayChecker IsEqualTo(double[,] ExpectedArray, string? Message = null)
    {
        Service.CheckSeparator(ref Message);
        Assert.AreEqual(ExpectedArray.GetLength(0), ActualValue.GetLength(0), "{0}{1}", Message, "Размеры массивов не совпадают");
        Assert.AreEqual(ExpectedArray.GetLength(1), ActualValue.GetLength(1), "{0}{1}", Message, "Размеры массивов не совпадают");

        for (var i = 0; i < ActualValue.GetLength(0); i++)
            for (var j = 0; j < ActualValue.GetLength(1); j++)
            {
                var expected = ExpectedArray[i, j];
                var actual = ActualValue[i, j];
                Assert.AreEqual(expected, actual,
                    "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}\r\n    err:{5}(rel:{6})",
                    Message, i, j, expected, actual,
                    Math.Abs(expected - actual).ToString("e3", CultureInfo.InvariantCulture),
                    (Math.Abs(expected - actual) / expected).ToString(CultureInfo.InvariantCulture));
            }

        return this;
    }

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedArray">Ожидаемая коллекция значений</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleDimensionArrayChecker IsEqualTo(double[,] ExpectedArray, double Accuracy, string? Message = null)
    {
        if (Accuracy is double.NaN)
            throw new ArgumentException("Значение точности не может быть равно NaN", nameof(Accuracy));

        Service.CheckSeparator(ref Message);
        Assert.AreEqual(ExpectedArray.GetLength(0), ActualValue.GetLength(0), "{0}{1}", Message, "Размеры массивов не совпадают");
        Assert.AreEqual(ExpectedArray.GetLength(1), ActualValue.GetLength(1), "{0}{1}", Message, "Размеры массивов не совпадают");

        for (var i = 0; i < ActualValue.GetLength(0); i++)
            for (var j = 0; j < ActualValue.GetLength(1); j++)
            {
                var expected = ExpectedArray[i, j];
                var actual = ActualValue[i, j];

                if (expected is double.NaN && actual is double.NaN)
                    continue;

                if (expected is double.NaN) throw new AssertFailedException("Ожидаемое значение равно NaN, а полученное - нет");
                if (actual is double.NaN) throw new AssertFailedException("Полученное значение равно NaN, а ожидание - нет");

                var delta = Math.Abs(expected - actual);
                if (delta > Accuracy)
                {
                    FormattableString message = $"{Message}Несовпадение по индексу [{i},{j}]\r\n    ожидалось:{expected}; получено:{actual}\r\n      err:{delta:e3}(rel:{delta / expected:e3})\r\n      eps:{Accuracy}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(expected, actual, Accuracy,
                //    "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; delta:{5}; err:{6}(rel:{7})",
                //    Message, i, j, expected, actual, Accuracy,
                //    Math.Abs(expected - actual).ToString("e3", CultureInfo.InvariantCulture),
                //    (Math.Abs(expected - actual) / expected).ToString(CultureInfo.InvariantCulture));
            }

        return this;
    }

    /// <summary>Все элементы массива равны ожидаемому значению</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleDimensionArrayChecker ElementsAreEqualTo(double ExpectedValue, string? Message = null)
    {
        Service.CheckSeparator(ref Message);
        for (var i = 0; i < ActualValue.GetLength(0); i++)
            for (var j = 0; j < ActualValue.GetLength(1); j++)
            {
                var actual = ActualValue[i, j];

                if (ExpectedValue != actual)
                {
                    var delta = Math.Abs(ExpectedValue - actual);
                    FormattableString message = $"{Message}Несовпадение по индексу [{i},{j}]\r\n    ожидалось:{ExpectedValue}; получено:{actual}\r\n    err:{delta:e3}(rel:{delta / ExpectedValue:e3})";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(ExpectedValue, actual,
                //    "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; err:{5}(rel:{6})",
                //    Message, i, j, ExpectedValue, actual,
                //    Math.Abs(ExpectedValue - actual).ToString("e3", CultureInfo.InvariantCulture),
                //    (Math.Abs(ExpectedValue - actual) / ExpectedValue).ToString(CultureInfo.InvariantCulture));
            }

        return this;
    }

    /// <summary>Все элементы массива равны ожидаемому значению</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleDimensionArrayChecker ElementsAreEqualTo(double ExpectedValue, double Accuracy, string? Message = null)
    {
        Service.CheckSeparator(ref Message);
        for (var i = 0; i < ActualValue.GetLength(0); i++)
            for (var j = 0; j < ActualValue.GetLength(1); j++)
            {
                var actual = ActualValue[i, j];

                var delta = Math.Abs(ExpectedValue - actual);
                if (delta > Accuracy)
                {
                    FormattableString message = $"{Message}Несовпадение по индексу [{i},{j}]\r\n    ожидалось:{ExpectedValue}; получено:{actual}\r\n    delta:{Accuracy}\r\n    err:{delta:e3}(rel:{delta / ExpectedValue:e3})";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(ExpectedValue, actual, Accuracy,
                //    "{0}Несовпадение по индексу [{1},{2}], ожидалось:{3}; получено:{4}; delta:{5}; err:{6}(rel:{7})",
                //    Message, i, j, ExpectedValue, actual, Accuracy,
                //    delta.ToString("e3", CultureInfo.InvariantCulture),
                //    (delta / ExpectedValue).ToString(CultureInfo.InvariantCulture));
            }

        return this;
    }

    /// <summary>Критерий проверки элементов коллекции</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Истина, если элемент соответствует критерию проверки</returns>
    public delegate bool ElementChecker(double ActualValue);

    /// <summary>Все элементы коллекции удовлетворяют условию</summary>
    /// <param name="Condition">Условие проверки всех элементов</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleDimensionArrayChecker ElementsAreSatisfyCondition(ElementChecker Condition, string? Message = null)
    {
        Service.CheckSeparator(ref Message);
        for (var i = 0; i < ActualValue.GetLength(0); i++)
            for (var j = 0; j < ActualValue.GetLength(1); j++)
            {
                var actual = ActualValue[i, j];

                if (!Condition(actual))
                {
                    FormattableString message = $"{Message}err[{i},{j}]:{actual}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Condition(actual), "{0}err[{1},{2}]:{3}", Message, i, j, actual);
            }

        return this;
    }

    /// <summary>Позиционный критерий проверки элементов коллекции</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="i">Индекс строки</param>
    /// <param name="j">Индекс столбца</param>
    /// <return>Истина, если элемент соответствует критерию проверки</return>
    public delegate bool PositionElementChecker(double ActualValue, int i, int j);

    /// <summary>Все элементы коллекции удовлетворяют условию</summary>
    /// <param name="Condition">Условие проверки всех элементов</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleDimensionArrayChecker ElementsAreSatisfyCondition(PositionElementChecker Condition, string? Message = null)
    {
        Service.CheckSeparator(ref Message);
        for (var i = 0; i < ActualValue.GetLength(0); i++)
            for (var j = 0; j < ActualValue.GetLength(1); j++)
            {
                var actual = ActualValue[i, j];

                if (!Condition(actual, i, j))
                {
                    FormattableString message = $"{Message}err[{i},{j}]:\r\n    {actual}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Condition(actual, i, j), "{0}err[{1},{2}]:{3}", Message, i, j, actual);
            }

        return this;
    }
}