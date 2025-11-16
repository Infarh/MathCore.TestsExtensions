// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global

using static System.Math;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки вещественных значений</summary>
public static class ValueCheckerDoubleExtensions
{
    /// <param name="Checker">Объект проверки вещественного значения</param>
    extension(ValueChecker<double> Checker)
    {
        /// <summary>Проверка значения на равенство</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public DoubleEqualityCheckerWithAccuracy IsEqualTo(double ExpectedValue) => new(Checker.ActualValue, ExpectedValue);

        /// <summary>Проверка значения на неравенство</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public DoubleEqualityCheckerWithAccuracy IsNotEqualTo(double ExpectedValue) => new(Checker.ActualValue, ExpectedValue, true);

        /// <summary>Сравнение с ожидаемым значением с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public ValueChecker<double> IsEqual(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (double.IsNaN(ExpectedValue)) throw new ArgumentException("ExpectedValue is NaN", nameof(ExpectedValue));
            var actual_value = Checker.ActualValue;
            if (double.IsNaN(actual_value)) throw new ArgumentException("Checker.ActualValue is NaN", nameof(Checker.ActualValue));
            if (double.IsNaN(Accuracy)) throw new ArgumentException("Accuracy is NaN", nameof(actual_value));

            var value_delta = ExpectedValue - actual_value;
            var value_delta_abs = Abs(value_delta);
            if (value_delta_abs <= Accuracy)
                return Checker;

            var msg = Message.AddSeparator(Environment.NewLine);
            var delta_rel = value_delta / actual_value;
            var error_delta = value_delta_abs - Accuracy;

            var new_accuracy = value_delta_abs;
            var expected_accuracy = new_accuracy + Pow(10, (int)Log10(new_accuracy) - 3);

            FormattableString message = $"""
             {msg}Ожидаемое значение
                 {ExpectedValue} не равно реальному
                 {actual_value}.
                 err:{value_delta_abs:e2}(err.rel:{delta_rel})
                 eps:{Accuracy}(eps-delta:{error_delta:e2})
                 Требуется точность :{expected_accuracy:e2}
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка на неравенство</summary>
        /// <param name="ExpectedValue">Не ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки вещественного значения</returns>
        public ValueChecker<double> IsNotEqual(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            FormattableString msg = $"{Message.AddSeparator()}err:{Abs(ExpectedValue - Checker.ActualValue):e2}(rel:{(ExpectedValue - Checker.ActualValue) / Checker.ActualValue}) eps:{Accuracy}";
            Assert.AreNotEqual(
                ExpectedValue, Checker.ActualValue, Accuracy,
                msg.ToStringInvariant());
            return Checker;
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public ValueChecker<double> GreaterThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue > ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть больше
                 {ExpectedValue}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }
    }

    /// <param name="Checker">Объект проверки вещественного значения</param>
    extension(ValueChecker<double> Checker)
    {
        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public ValueChecker<double> GreaterThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue + Abs(Accuracy) > ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть больше
                 {ExpectedValue} при точности {Accuracy}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public ValueChecker<double> GreaterOrEqualsThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue >= ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Нарушено условие
                 {Checker.ActualValue}
              >= {ExpectedValue}
                 delta:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public ValueChecker<double> GreaterOrEqualsThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue >= (ExpectedValue - Accuracy))
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Нарушено условие
                 {Checker.ActualValue}
              >= {ExpectedValue}
                 точность:{Accuracy:e2}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }
    }

    /// <param name="Checker">Объект проверки вещественного значения</param>
    extension(DoubleValueChecker Checker)
    {
        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker GreaterOrEqualsThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue >= (ExpectedValue - Accuracy))
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Нарушено условие
                 {Checker.ActualValue}
              >= {ExpectedValue}
                 точность:{Accuracy:e2}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker LessThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue < ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть меньше
                 {ExpectedValue}
                 err:{delta:e2}(rel.err:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker LessThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue - Abs(Accuracy) < ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть меньше
                 {ExpectedValue} при точности {Accuracy}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker GreaterThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue > ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть больше
                 {ExpectedValue}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker GreaterThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue + Abs(Accuracy) > ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть больше
                 {ExpectedValue} при точности {Accuracy}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker GreaterOrEqualsThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue >= ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Нарушено условие
                 {Checker.ActualValue}
              >= {ExpectedValue}
                 delta:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker LessOrEqualsThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue <= ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть меньше, либо равно
                 {ExpectedValue}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public DoubleValueChecker LessOrEqualsThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue <= ExpectedValue + Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Нарушено условие
                {Checker.ActualValue}
              >= {ExpectedValue}
                 точность:{Accuracy:e2}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }
    }

    /// <param name="Checker">Объект проверки вещественного значения</param>
    extension(ValueChecker<double> Checker)
    {
        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public ValueChecker<double> LessOrEqualsThan(double ExpectedValue,
            string? Message = null)
        {
            if (Checker.ActualValue <= ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Значение
                 {Checker.ActualValue} должно быть меньше, либо равно
                 {ExpectedValue}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public ValueChecker<double> LessOrEqualsThan(double ExpectedValue,
            double Accuracy,
            string? Message = null)
        {
            if (Checker.ActualValue <= ExpectedValue + Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            var delta = ExpectedValue - Checker.ActualValue;
            FormattableString message = $"""
             {msg}Нарушено условие
                {Checker.ActualValue}
              >= {ExpectedValue}
                 точность:{Accuracy:e2}
                 err:{delta:e2}(err.rel:{delta / ExpectedValue})
             """;
            throw new AssertFailedException(message.ToStringInvariant());
        }
    }

    /// <param name="Checker">Объект проверки вещественного значения</param>
    extension(ValueChecker<double> Checker)
    {
        /// <summary>Значение больше (строго), чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public DoubleCompareCheckerWithAccuracy Greater(double ExpectedValue) =>
            new(Checker.ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: false);

        /// <summary>Значение больше, чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public DoubleCompareCheckerWithAccuracy GreaterOrEqual(double ExpectedValue) =>
            new(Checker.ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: false);

        /// <summary>Значение меньше (строго), чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public DoubleCompareCheckerWithAccuracy Less(double ExpectedValue) =>
            new(Checker.ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: true);

        /// <summary>Значение меньше, чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public DoubleCompareCheckerWithAccuracy LessOrEqual(double ExpectedValue) =>
            new(Checker.ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: true);

        /// <summary>Проверить, что значение не является не-числом</summary>
        /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
        public ValueChecker<double> IsNotNaN(string? Message = null) => double.IsNaN(Checker.ActualValue) 
            ? throw new AssertFailedException($"{Message.AddSeparator()}Значение не является числом") 
            : Checker;

        /// <summary>Проверить, что значение является не-числом</summary>
        /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
        public ValueChecker<double> IsNaN(string? Message = null) => double.IsNaN(Checker.ActualValue) 
            ? Checker 
            : throw new AssertFailedException($"{Message.AddSeparator()}Значение не не является числом");
    }

    /// <summary>Сравнение с ожидаемым значением с задаваемой точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Точность</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки родительского объекта-значения</returns>
    public static ValueChecker<TBaseValue> CheckEquals<TBaseValue>(
        this NestedValueChecker<double, TBaseValue> Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        Assert.That.Value(Checker.ActualValue).IsEqual(ExpectedValue, Accuracy, Message);
        return Checker.BaseValue;
    }
}