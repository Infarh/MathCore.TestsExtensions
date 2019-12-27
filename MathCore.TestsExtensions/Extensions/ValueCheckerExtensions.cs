using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MathCore.Tests.Annotations;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки значения</summary>
    public static class ValueCheckerExtensions
    {
        /// <summary>Проверка значения на истинность</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public static ValueChecker<bool> IsTrue(this ValueChecker<bool> Checker, string Message = null)
        {
            Assert.IsTrue(Checker.ActualValue, "{0}Значение не истинно", Message.AddSeparator());
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public static ValueChecker<bool> IsFalse(this ValueChecker<bool> Checker, string Message = null)
        {
            Assert.IsFalse(Checker.ActualValue, "{0}Значение не ложно", Message.AddSeparator());
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Value">Значение для проверки</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public static ValueChecker<bool> Is(this ValueChecker<bool> Checker, bool Value, string Message = null)
        {
            if (Value)
                Assert.IsTrue(Checker.ActualValue, "{0}Значение не истинно", Message.AddSeparator());
            else
                Assert.IsFalse(Checker.ActualValue, "{0}Значение не ложно", Message.AddSeparator());
            return Checker;
        }

        /// <summary>Проверка значения как коллекции элементов</summary>
        /// <param name="Checker">Объект проверки одиночного значения</param>
        /// <typeparam name="T">Тип проверяемого значения, которое является коллекцией объектов типа <typeparamref name="TItem"/></typeparam>
        /// <typeparam name="TItem">Тип элементов проверяемой коллекции</typeparam>
        /// <returns>Объект проверки коллекции</returns>
        public static CollectionChecker<TItem> Are<T, TItem>(this ValueChecker<T> Checker) where T : ICollection<TItem> => new CollectionChecker<TItem>(Checker.ActualValue);

        /// <summary>Проверка, что строка начинается с указанного префикса</summary>
        /// <param name="Checker">Объект проверки строкового значения</param>
        /// <param name="ExpectedPrefix">Ожидаемый префикс</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        [NotNull]
        public static ValueChecker<string> StartWith([NotNull] this ValueChecker<string> Checker, [NotNull] string ExpectedPrefix, string Message = null)
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
        [NotNull]
        public static ValueChecker<string> EndWith([NotNull] this ValueChecker<string> Checker, [NotNull] string ExpectedSuffix, string Message = null)
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
        [NotNull]
        public static ValueChecker<string> Contains([NotNull] this ValueChecker<string> Checker, [NotNull] string ExpectedSubstring, string Message = null)
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
        [NotNull] public static ValueChecker<string> Matches([NotNull] this ValueChecker<string> Checker, [NotNull, RegexPattern] string ExpectedRegEx, string Message = null) => Checker.Matches(new Regex(ExpectedRegEx), Message);

        /// <summary>Проверка, что строка соответствует указанному регулярному выражению</summary>
        /// <param name="Checker">Объект проверки строкового значения</param>
        /// <param name="ExpectedRegEx">Ожидаемое регулярное выражение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        [NotNull]
        public static ValueChecker<string> Matches([NotNull] this ValueChecker<string> Checker, [NotNull] Regex ExpectedRegEx, string Message = null)
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
        [NotNull] public static ValueChecker<string> DoesNotMatch([NotNull] this ValueChecker<string> Checker, [NotNull, RegexPattern] string ExpectedRegEx, string Message = null) => Checker.DoesNotMatch(new Regex(ExpectedRegEx), Message);

        /// <summary>Проверка, что строка НЕ соответствует указанному регулярному выражению</summary>
        /// <param name="Checker">Объект проверки строкового значения</param>
        /// <param name="ExpectedRegEx">Регулярное выражение, которому не должна соответствовать строка</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки при проверке</param>
        /// <returns>Исходный объект проверки строки</returns>
        [NotNull]
        public static ValueChecker<string> DoesNotMatch([NotNull] this ValueChecker<string> Checker, [NotNull] Regex ExpectedRegEx, string Message = null)
        {
            StringAssert.DoesNotMatch(Checker.ActualValue, ExpectedRegEx,
                "{0}Указанная строка {1} ошибочно соответствует ожидаемому регулярному выражению {2}",
                Message.AddSeparator(),
                Checker.ActualValue,
                ExpectedRegEx);
            return Checker;
        }
    }
}
