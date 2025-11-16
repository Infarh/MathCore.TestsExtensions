using System.Text.RegularExpressions;

using MathCore.Tests.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки строковых значений</summary>
public static class ValueCheckerStringExtensions
{
    /// <param name="Checker">Объект проверки строкового значения</param>
    extension(ValueChecker<string> Checker)
    {
        /// <summary>Проверка, что строка начинается с указанного префикса</summary>
        /// <param name="ExpectedPrefix">Ожидаемый префикс</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> StartWith(string ExpectedPrefix, string? Message = null)
        {
            FormattableString message = $"{Message.AddSeparator()}Указанная строка {Checker.ActualValue} не начинается с ожидаемого префикса {ExpectedPrefix}";
            Assert.StartsWith(Checker.ActualValue, ExpectedPrefix, message.ToStringInvariant());
            return Checker;
        }

        /// <summary>Проверка, что строка заканчивается указанной подстрокой</summary>
        /// <param name="ExpectedSuffix">Ожидаемое окончание</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> EndWith(string ExpectedSuffix, string? Message = null)
        {
            FormattableString msg = $"{Message.AddSeparator()}Указанная строка {Checker.ActualValue} не заканчивается ожидаемым окончанием {ExpectedSuffix}";
            Assert.EndsWith(Checker.ActualValue, ExpectedSuffix, msg.ToStringInvariant());
            return Checker;
        }

        /// <summary>Проверка, что строка содержит ожидаемую подстроку</summary>
        /// <param name="ExpectedSubstring">Ожидаемая подстрока</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> Contains(string ExpectedSubstring, string? Message = null)
        {
            FormattableString msg = $"{Message.AddSeparator()}Указанная строка {Checker.ActualValue} не содержит ожидаемой подстроки {ExpectedSubstring}";
            Assert.Contains(Checker.ActualValue!, ExpectedSubstring, msg.ToStringInvariant());
            return Checker;
        }

        /// <summary>Проверка, что строка соответствует указанному регулярному выражению</summary>
        /// <param name="ExpectedRegEx">Ожидаемое регулярное выражение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> Matches([RegexPattern] string ExpectedRegEx, string? Message = null) => Checker.Matches(new Regex(ExpectedRegEx), Message);

        /// <summary>Проверка, что строка соответствует указанному регулярному выражению</summary>
        /// <param name="ExpectedRegEx">Ожидаемое регулярное выражение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> Matches(Regex ExpectedRegEx, string? Message = null)
        {
            FormattableString msg = $"{Message.AddSeparator()}Указанная строка {Checker.ActualValue} не соответствует ожидаемому регулярному выражению {ExpectedRegEx}";
            StringAssert.Matches(Checker.ActualValue, ExpectedRegEx, msg.ToStringInvariant());
            return Checker;
        }

        /// <summary>Проверка, что строка НЕ соответствует указанному регулярному выражению</summary>
        /// <param name="ExpectedRegEx">Регулярное выражение, которому не должна соответствовать строка</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> DoesNotMatch([RegexPattern] string ExpectedRegEx, string? Message = null) => Checker.DoesNotMatch(new Regex(ExpectedRegEx), Message);

        /// <summary>Проверка, что строка НЕ соответствует указанному регулярному выражению</summary>
        /// <param name="ExpectedRegEx">Регулярное выражение, которому не должна соответствовать строка</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> DoesNotMatch(Regex ExpectedRegEx, string? Message = null)
        {
            FormattableString msg = $"{Message.AddSeparator()}Указанная строка {Checker.ActualValue} ошибочно соответствует ожидаемому регулярному выражению {ExpectedRegEx}";
            StringAssert.DoesNotMatch(Checker.ActualValue, ExpectedRegEx, msg.ToStringInvariant());
            return Checker;
        }

        /// <summary>Проверка, что ссылка на строку не пуста и строка не является пустой</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> IsNotNullOrEmpty(string? Message = null)
        {
            var str = Checker.ActualValue;
            if (!string.IsNullOrEmpty(str)) return Checker;

            throw new AssertFailedException($"{Message.AddSeparator()}Строка является пустой {(str is null ? "ссылкой" : "строкой")}")
            {
                Data = { { "Actual", Checker.ActualValue } }
            };
        }

        /// <summary>Проверка, что ссылка на строку пуста, либо строка пуста</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> IsNullOrEmpty(string? Message = null)
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
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> IsNotNullOrWhiteSpace(string? Message = null)
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
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        public ValueChecker<string> IsNullOrWhiteSpace(string? Message = null)
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
}