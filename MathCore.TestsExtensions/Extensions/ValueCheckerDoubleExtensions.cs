using System;
using System.Globalization;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable RedundantArgumentDefaultValue

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки вещественных значений</summary>
    public static class ValueCheckerDoubleExtensions
    {
        /// <summary>Проверка значения на равенство</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        [NotNull]
        public static DoubleEqualityCheckerWithAccuracy IsEqualTo([NotNull] this ValueChecker<double> Checker, double ExpectedValue) => 
            new DoubleEqualityCheckerWithAccuracy(Checker.ActualValue, ExpectedValue);

        /// <summary>Проверка значения на неравенство</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        [NotNull]
        public static DoubleEqualityCheckerWithAccuracy IsNotEqualTo([NotNull] this ValueChecker<double> Checker, double ExpectedValue) => 
            new DoubleEqualityCheckerWithAccuracy(Checker.ActualValue, ExpectedValue, true);

        /// <summary>Сравнение с ожидаемым значением с задаваемой точностью</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки вещественного значения</returns>
        [NotNull]
        public static ValueChecker<double> IsEqual(
            [NotNull] this ValueChecker<double> Checker, 
            double ExpectedValue, 
            double Accuracy, 
            string Message = null)
        {
            if(double.IsNaN(ExpectedValue)) throw new ArgumentException("ExpectedValue is NaN", nameof(ExpectedValue));
            var actual_value = Checker.ActualValue;
            if(double.IsNaN(actual_value)) throw new ArgumentException("Checker.ActualValue is NaN", nameof(Checker.ActualValue));
            if(double.IsNaN(Accuracy)) throw new ArgumentException("Accuracy is NaN", nameof(actual_value));

            var value_delta = ExpectedValue - actual_value;
            var value_delta_abs = Math.Abs(value_delta);
            if (value_delta_abs <= Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            var invariant_culture = CultureInfo.InvariantCulture;
            var delta_str = value_delta_abs.ToString("e2", invariant_culture);
            var rel_delta_str = (value_delta / actual_value).ToString(invariant_culture);
            var error_delta = value_delta_abs - Accuracy;
            var err_delta_str = error_delta.ToString("e2", invariant_culture);

            var error_delta_rel = error_delta / Accuracy;
            var expected_accuracy = error_delta_rel < 0.1 ? $" expected-eps:{(Accuracy + error_delta).ToString("e2", invariant_culture)}" : null;

            throw new AssertFailedException(
                $"{msg}err:{delta_str}(rel:{rel_delta_str}) eps:{Accuracy}(eps-delta:{err_delta_str}){expected_accuracy}");
        }

        /// <summary>Проверка на неравенство</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Не ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки вещественного значения</returns>
        [NotNull]
        public static ValueChecker<double> IsNotEqual(
            [NotNull] this ValueChecker<double> Checker, 
            double ExpectedValue, 
            double Accuracy,
            string Message = null)
        {
            Assert.AreNotEqual(
                ExpectedValue, Checker.ActualValue, Accuracy,
                "{0}err:{1}(rel:{2}) eps:{3}",
                Message.AddSeparator(), 
                Math.Abs(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture),
                ((ExpectedValue - Checker.ActualValue) / Checker.ActualValue).ToString(CultureInfo.InvariantCulture),
                Accuracy);
            return Checker;
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        [NotNull]
        public static ValueChecker<double> GreaterThan([NotNull] this ValueChecker<double> Checker, double ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue > ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение {Checker.ActualValue} должно быть больше {ExpectedValue}. delta:{(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture)}");
            return Checker;
        }

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        [NotNull]
        public static ValueChecker<double> GreaterOrEqualsThan([NotNull] this ValueChecker<double> Checker, double ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue >= ExpectedValue))
                throw new AssertFailedException(
                    $"{Message.AddSeparator()}Нарушено условие ({Checker.ActualValue} >= {ExpectedValue}). delta:{(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture)}");
            return Checker;
        }

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        [NotNull]
        public static ValueChecker<double> GreaterOrEqualsThan([NotNull] this ValueChecker<double> Checker, double ExpectedValue, double Accuracy, string Message = null)
        {
            if (!(Checker.ActualValue - ExpectedValue <= Accuracy))
                throw new AssertFailedException($"{Message.AddSeparator()}Нарушено условие ({Checker.ActualValue} >= {ExpectedValue}) при точности {Accuracy:e2}. delta:{(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture)}");
            return Checker;
        }

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        [NotNull]
        public static ValueChecker<double> LessThan([NotNull] this ValueChecker<double> Checker, double ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue < ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение {Checker.ActualValue} должно быть меньше {ExpectedValue}. delta:{(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture)}");
            return Checker;
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        [NotNull]
        public static ValueChecker<double> LessOrEqualsThan([NotNull] this ValueChecker<double> Checker, double ExpectedValue, string Message = null)
        {
            if (!(Checker.ActualValue <= ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение {Checker.ActualValue} должно быть меньше, либо равно {ExpectedValue}. delta:{(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture)}");
            return Checker;
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        [NotNull]
        public static ValueChecker<double> LessOrEqualsThan([NotNull] this ValueChecker<double> Checker, double ExpectedValue, double Accuracy, string Message = null)
        {
            if (!(ExpectedValue - Checker.ActualValue <= Accuracy))
                throw new AssertFailedException(
                    $"{Message.AddSeparator()}Нарушено условие ({Checker.ActualValue} >= {ExpectedValue}) при точности {Accuracy:e2}. delta:{(ExpectedValue - Checker.ActualValue).ToString("e2", CultureInfo.InvariantCulture)}");
            return Checker;
        }

        /// <summary>Значение больше (строго), чем указанное с задаваемой точностью</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        [NotNull]
        public static DoubleCompareCheckerWithAccuracy Greater([NotNull] this ValueChecker<double> Checker, double ExpectedValue) =>
            new DoubleCompareCheckerWithAccuracy(Checker.ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: false);

        /// <summary>Значение больше, чем указанное с задаваемой точностью</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        [NotNull]
        public static DoubleCompareCheckerWithAccuracy GreaterOrEqual([NotNull] this ValueChecker<double> Checker, double ExpectedValue) =>
            new DoubleCompareCheckerWithAccuracy(Checker.ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: false);

        /// <summary>Значение меньше (строго), чем указанное с задаваемой точностью</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        [NotNull]
        public static DoubleCompareCheckerWithAccuracy Less([NotNull] this ValueChecker<double> Checker, double ExpectedValue) =>
            new DoubleCompareCheckerWithAccuracy(Checker.ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: true);

        /// <summary>Значение меньше, чем указанное с задаваемой точностью</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        [NotNull]
        public static DoubleCompareCheckerWithAccuracy LessOrEqual([NotNull] this ValueChecker<double> Checker, double ExpectedValue) =>
            new DoubleCompareCheckerWithAccuracy(Checker.ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: true);

        /// <summary>Проверить, что значение не является не-числом</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
        [NotNull]
        public static ValueChecker<double> IsNotNaN([NotNull] this ValueChecker<double> Checker, string Message = null)
        {
            if(double.IsNaN(Checker.ActualValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение не является числом");
            return Checker;
        }

        /// <summary>Проверить, что значение является не-числом</summary>
        /// <param name="Checker">Объект проверки вещественного значения</param>
        /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
        [NotNull]
        public static ValueChecker<double> IsNaN([NotNull] this ValueChecker<double> Checker, string Message = null)
        {
            if (!double.IsNaN(Checker.ActualValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Значение не не является числом");
            return Checker;
        }
    }
}