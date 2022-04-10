using System.Globalization;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки целочисленных значений</summary>
public static class ValueCheckerIntExtensions
{
    /// <summary>Проверка, что проверяемое значение равно ожидаемому с заданной точностью</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> IsEqual(this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string? Message = null)
    {
        var delta = Math.Abs(ExpectedValue - Checker.ActualValue);
        if (delta <= Accuracy)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}actual:{Checker.ActualValue}\r\n    != {ExpectedValue}\r\n         err:{delta}(err.rel:{(ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue:e3})\r\n    accuracy:{Accuracy}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));

        //Assert.AreEqual(
        //    ExpectedValue, Checker.ActualValue, Accuracy,
        //    "{0}err:{1}(rel:{2}), eps:{3}",
        //    Message.AddSeparator(), 
        //    delta,
        //    ((ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue).ToString(CultureInfo.InvariantCulture),
        //    Accuracy);
    }

    /// <summary>Проверка, что проверяемое значение не равно ожидаемому с заданной точностью</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> IsNotEqual(this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string? Message = null)
    {
        var delta = Math.Abs(ExpectedValue - Checker.ActualValue);
        if (delta >= Accuracy)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}actual:{Checker.ActualValue}\r\n    == {ExpectedValue}\r\n         err:{delta}(err.rel:{(ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue:e3})\r\n    accuracy:{Accuracy}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));

        //Assert.AreNotEqual(
        //    ExpectedValue, Checker.ActualValue, Accuracy,
        //    "{0}err:{1}(rel:{2}), eps:{3}",
        //    Message.AddSeparator(),
        //    Math.Abs(ExpectedValue - Checker.ActualValue),
        //    ((ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue).ToString(CultureInfo.InvariantCulture),
        //    Accuracy);
    }

    /// <summary>Проверка, что значение больше заданного</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> GreaterThan(this ValueChecker<int> Checker, int ExpectedValue, string? Message = null)
    {
        if (Checker.ActualValue > ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть больше\r\n    {ExpectedValue}\r\n    err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> GreaterOrEqualsThan(this ValueChecker<int> Checker, int ExpectedValue, string? Message = null)
    {
        if (Checker.ActualValue >= ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> GreaterOrEqualsThan(this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string? Message = null)
    {
        if (Checker.ActualValue - ExpectedValue <= Accuracy)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})\r\n    accuracy:{Accuracy}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше заданного</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> LessThan(this ValueChecker<int> Checker, int ExpectedValue, string? Message = null)
    {
        if (Checker.ActualValue < ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть меньше\r\n    {ExpectedValue}\r\n    err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> LessOrEqualsThan(this ValueChecker<int> Checker, int ExpectedValue, string? Message = null)
    {
        if (Checker.ActualValue <= ExpectedValue)
            return Checker;

        FormattableString message = $"{Message.AddSeparator()}Значение\r\n    {Checker.ActualValue} должно быть меньше, либо равно\r\n    {ExpectedValue}\r\n    err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> LessOrEqualsThan(this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string? Message = null)
    {
        if (ExpectedValue - Checker.ActualValue <= Accuracy)
            return Checker;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})\r\n    accuracy:{Accuracy}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка - является ли число чётным?</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> IsEven(this ValueChecker<int> Checker, string? Message = null)
    {
        if (Checker.ActualValue % 2 == 0)
            return Checker;

        FormattableString message = $"{Message.AddSeparator()}Число {Checker.ActualValue} не является чётным";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка - является ли число нечётным?</summary>
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
    /// <returns>Объект проверки целочисленного значения</returns>
    public static ValueChecker<int> IsOdd(this ValueChecker<int> Checker, string? Message = null)
    {
        if (Checker.ActualValue % 2 != 0)
            return Checker;

        FormattableString message = $"{Message.AddSeparator()}Число {Checker.ActualValue} является чётным";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }
}