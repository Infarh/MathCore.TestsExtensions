using System.Text.RegularExpressions;

using MathCore.Tests.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки строковых значений</summary>
public static class ValueCheckerStringExtensions
{
    /// <summary>Проверка, что строка начинается с указанного префикса</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedPrefix">Ожидаемый префикс</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> StartWith(this ValueChecker<string> Checker, string ExpectedPrefix, string? Message = null)
    {
        StringAssert.StartsWith(Checker.ActualValue, ExpectedPrefix,
            "{0}Указанная строка {1} не начинается с ожидаемого префикса {2}",
            Message.AddSeparator(),
            Checker.ActualValue,
            ExpectedPrefix);
        return Checker;
    }

    /// <summary>Проверка, что строка заканчивается указанной подстрокой</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedSuffix">Ожидаемое окончание</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> EndWith(this ValueChecker<string> Checker, string ExpectedSuffix, string? Message = null)
    {
        StringAssert.EndsWith(Checker.ActualValue, ExpectedSuffix,
            "{0}Указанная строка {1} не заканчивается ожидаемым окончанием {2}",
            Message.AddSeparator(),
            Checker.ActualValue,
            ExpectedSuffix);
        return Checker;
    }

    /// <summary>Проверка, что строка содержит ожидаемую подстроку</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedSubstring">Ожидаемая подстрока</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> Contains(this ValueChecker<string> Checker, string ExpectedSubstring, string? Message = null)
    {
        StringAssert.Contains(Checker.ActualValue, ExpectedSubstring,
            "{0}Указанная строка {1} не содержит ожидаемой подстроки {2}",
            Message.AddSeparator(),
            Checker.ActualValue,
            ExpectedSubstring);
        return Checker;
    }

    /// <summary>Проверка, что строка соответствует указанному регулярному выражению</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedRegEx">Ожидаемое регулярное выражение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> Matches(this ValueChecker<string> Checker, [RegexPattern] string ExpectedRegEx, string? Message = null) => Checker.Matches(new Regex(ExpectedRegEx), Message);

    /// <summary>Проверка, что строка соответствует указанному регулярному выражению</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedRegEx">Ожидаемое регулярное выражение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> Matches(this ValueChecker<string> Checker, Regex ExpectedRegEx, string? Message = null)
    {
        StringAssert.Matches(Checker.ActualValue, ExpectedRegEx,
            "{0}Указанная строка {1} не соответствует ожидаемому регулярному выражению {2}",
            Message.AddSeparator(),
            Checker.ActualValue,
            ExpectedRegEx);
        return Checker;
    }

    /// <summary>Проверка, что строка НЕ соответствует указанному регулярному выражению</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedRegEx">Регулярное выражение, которому не должна соответствовать строка</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> DoesNotMatch(this ValueChecker<string> Checker, [RegexPattern] string ExpectedRegEx, string? Message = null) => Checker.DoesNotMatch(new Regex(ExpectedRegEx), Message);

    /// <summary>Проверка, что строка НЕ соответствует указанному регулярному выражению</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="ExpectedRegEx">Регулярное выражение, которому не должна соответствовать строка</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> DoesNotMatch(this ValueChecker<string> Checker, Regex ExpectedRegEx, string? Message = null)
    {
        StringAssert.DoesNotMatch(Checker.ActualValue, ExpectedRegEx,
            "{0}Указанная строка {1} ошибочно соответствует ожидаемому регулярному выражению {2}",
            Message.AddSeparator(),
            Checker.ActualValue,
            ExpectedRegEx);
        return Checker;
    }

    /// <summary>Проверка, что ссылка на строку не пуста и строка не является пустой</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> IsNotNullOrEmpty(this ValueChecker<string> Checker, string? Message = null)
    {
        var str = Checker.ActualValue;
        if (!string.IsNullOrEmpty(str)) return Checker;

        throw new AssertFailedException($"{Message.AddSeparator()}Строка является пустой {(str is null ? "ссылкой" : "строкой")}")
        {
            Data = { { "Actual", Checker.ActualValue } }
        };
    }

    /// <summary>Проверка, что ссылка на строку пуста, либо строка пуста</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> IsNullOrEmpty(this ValueChecker<string> Checker, string? Message = null)
    {
        var str = Checker.ActualValue;
        if (!string.IsNullOrEmpty(str))
            throw new AssertFailedException($"{Message.AddSeparator()}Строка не пуста")
            {
                Data = { { "Actual", Checker.ActualValue } }
            };
        return Checker;
    }

    /// <summary>Проверка, что ссылка на строку не пуста и строка не является пустой, либо состоящей из пробелов</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> IsNotNullOrWhiteSpace(this ValueChecker<string> Checker, string? Message = null)
    {
        var str = Checker.ActualValue;
        if (string.IsNullOrWhiteSpace(str))
            throw new AssertFailedException($"{Message.AddSeparator()}Строка является {(str is null ? "пустой ссылкой" : string.IsNullOrWhiteSpace(str) ? "строкой из пробелов" : "пустой строкой")}")
            {
                Data = { { "Actual", Checker.ActualValue } }
            };
        return Checker;
    }

    /// <summary>Проверка, что ссылка на строку пуста, либо строка пуста, либо строка состоит из пробелов</summary>
    /// <param name="Checker">Объект проверки строкового значения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
    /// <returns>Исходный объект проверки строки</returns>
    public static ValueChecker<string> IsNullOrWhiteSpace(this ValueChecker<string> Checker, string? Message = null)
    {
        var str = Checker.ActualValue;
        if (!string.IsNullOrWhiteSpace(str))
            throw new AssertFailedException($"{Message.AddSeparator()}Строка не пуста")
            {
                Data = { { "Actual", Checker.ActualValue } }
            };
        return Checker;
    }
}