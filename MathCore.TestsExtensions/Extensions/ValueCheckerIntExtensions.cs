using System;
using System.Globalization;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки целочисленных значений</summary>
    public static class ValueCheckerIntExtensions
    {
        /// <summary>Проверка, что проверяемое значение равно ожидаемому с заданной точностью</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> IsEqual([NotNull] this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string Message = null)
        {
            Assert.AreEqual(
                ExpectedValue, Checker.ActualValue, Accuracy,
                "{0}err:{1}(rel:{2}), eps:{3}",
                Message.AddSeparator(), 
                Math.Abs(ExpectedValue - Checker.ActualValue),
                ((ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue).ToString(CultureInfo.InvariantCulture),
                Accuracy);
            return Checker;
        }

        /// <summary>Проверка, что проверяемое значение не равно ожидаемому с заданной точностью</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> IsNotEqual([NotNull] this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string Message = null)
        {
            Assert.AreNotEqual(
                ExpectedValue, Checker.ActualValue, Accuracy,
                "{0}err:{1}(rel:{2}), eps:{3}",
                Message.AddSeparator(),
                 Math.Abs(ExpectedValue - Checker.ActualValue),
                ((ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue).ToString(CultureInfo.InvariantCulture),
                Accuracy);
            return Checker;
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> GreaterThan([NotNull] this ValueChecker<int> Checker, int ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue > ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение {Checker.ActualValue} должно быть больше {ExpectedValue}. delta:{ExpectedValue - Checker.ActualValue}");
            return Checker;
        }

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> GreaterOrEqualsThan([NotNull] this ValueChecker<int> Checker, int ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue >= ExpectedValue))
                throw new AssertFailedException(
                    $"{Message.AddSeparator()}Нарушено условие ({Checker.ActualValue} >= {ExpectedValue}). delta:{ExpectedValue - Checker.ActualValue}");
            return Checker;
        }

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> GreaterOrEqualsThan([NotNull] this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string Message = null)
        {
            if (!(Checker.ActualValue - ExpectedValue <= Accuracy))
                throw new AssertFailedException($"{Message.AddSeparator()}Нарушено условие ({Checker.ActualValue} >= {ExpectedValue}) при точности {Accuracy}. delta:{ExpectedValue - Checker.ActualValue}");
            return Checker;
        }

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> LessThan([NotNull] this ValueChecker<int> Checker, int ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue < ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение {Checker.ActualValue} должно быть меньше {ExpectedValue}. delta:{ExpectedValue - Checker.ActualValue}");
            return Checker;
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> LessOrEqualsThan([NotNull] this ValueChecker<int> Checker, int ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue <= ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение {Checker.ActualValue} должно быть меньше, либо равно {ExpectedValue}. delta:{ExpectedValue - Checker.ActualValue}");
            return Checker;
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> LessOrEqualsThan([NotNull] this ValueChecker<int> Checker, int ExpectedValue, int Accuracy, string Message = null)
        {
            if (!(ExpectedValue - Checker.ActualValue <= Accuracy))
                throw new AssertFailedException(
                    $"{Message.AddSeparator()}Нарушено условие ({Checker.ActualValue} >= {ExpectedValue}) при точности {Accuracy}. delta:{ExpectedValue - Checker.ActualValue}");
            return Checker;
        }

        /// <summary>Проверка - является ли число чётным?</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> IsEven([NotNull] this ValueChecker<int> Checker, string Message = null)
        {
            if (Checker.ActualValue % 2 != 0)
                throw new AssertFailedException($"{Message.AddSeparator()}Число {Checker.ActualValue} не является чётным");
            return Checker;
        }

        /// <summary>Проверка - является ли число нечётным?</summary>
        /// <param name="Checker">Объект проверки целочисленного значения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        [NotNull]
        public static ValueChecker<int> IsOdd([NotNull] this ValueChecker<int> Checker, string Message = null)
        {
            if (Checker.ActualValue % 2 == 0)
                throw new AssertFailedException($"{Message.AddSeparator()}Число {Checker.ActualValue} является чётным");
            return Checker;
        }
    }
}