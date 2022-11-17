// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки вещественных значений</summary>
public static class ValueCheckerDoubleExtensions
{
    /// <summary>Проверка значения на равенство</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public static DoubleEqualityCheckerWithAccuracy IsEqualTo(
        this ValueChecker<double> Checker,
        double ExpectedValue) =>
        new(Checker.ActualValue, ExpectedValue);

    /// <summary>Проверка значения на неравенство</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public static DoubleEqualityCheckerWithAccuracy IsNotEqualTo(
        this ValueChecker<double> Checker,
        double ExpectedValue) =>
        new(Checker.ActualValue, ExpectedValue, true);

    /// <summary>Сравнение с ожидаемым значением с задаваемой точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Точность</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static ValueChecker<double> IsEqual(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (double.IsNaN(ExpectedValue)) throw new ArgumentException("ExpectedValue is NaN", nameof(ExpectedValue));
        var actual_value = Checker.ActualValue;
        if (double.IsNaN(actual_value)) throw new ArgumentException("Checker.ActualValue is NaN", nameof(Checker.ActualValue));
        if (double.IsNaN(Accuracy)) throw new ArgumentException("Accuracy is NaN", nameof(actual_value));

        var value_delta = ExpectedValue - actual_value;
        var value_delta_abs = Math.Abs(value_delta);
        if (value_delta_abs <= Accuracy)
            return Checker;

        var msg = Message.AddSeparator(Environment.NewLine);
        var delta_rel = value_delta / actual_value;
        var error_delta = value_delta_abs - Accuracy;

        var new_accuracy = value_delta_abs;
        var expected_accuracy = new_accuracy + Math.Pow(10, (int)Math.Log10(new_accuracy) - 3);

        FormattableString message = $"{msg}Ожидаемое значение\r\n    {ExpectedValue} не равно реальному\r\n    {actual_value}.\r\n    err:{value_delta_abs:e2}(err.rel:{delta_rel})\r\n    eps:{Accuracy}(eps-delta:{error_delta:e2})\r\n    Требуется точность :{expected_accuracy:e2}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка на неравенство</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Не ожидаемое значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static ValueChecker<double> IsNotEqual(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
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
    public static ValueChecker<double> GreaterThan(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue > ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть больше\r\n    {ExpectedValue}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше заданного</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker GreaterThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue > ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть больше\r\n    {ExpectedValue}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше заданного</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static ValueChecker<double> GreaterThan(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue + Math.Abs(Accuracy) > ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть больше\r\n    {ExpectedValue} при точности {Accuracy}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше заданного</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker GreaterThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue + Math.Abs(Accuracy) > ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть больше\r\n    {ExpectedValue} при точности {Accuracy}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static ValueChecker<double> GreaterOrEqualsThan(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue >= ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    delta:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker GreaterOrEqualsThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue >= ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    delta:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static ValueChecker<double> GreaterOrEqualsThan(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue >= (ExpectedValue - Accuracy))
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    точность:{Accuracy:e2}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker GreaterOrEqualsThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue >= (ExpectedValue - Accuracy))
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Нарушено условие\r\n    {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    точность:{Accuracy:e2}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше заданного</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker LessThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue < ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть меньше\r\n    {ExpectedValue}\r\n    err:{delta:e2}(rel.err:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение больше заданного</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker LessThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue - Math.Abs(Accuracy) < ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть меньше\r\n    {ExpectedValue} при точности {Accuracy}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static ValueChecker<double> LessOrEqualsThan(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue <= ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть меньше, либо равно\r\n    {ExpectedValue}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker LessOrEqualsThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        string? Message = null)
    {
        if (Checker.ActualValue <= ExpectedValue)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Значение\r\n    {Checker.ActualValue} должно быть меньше, либо равно\r\n    {ExpectedValue}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue:e2})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static ValueChecker<double> LessOrEqualsThan(
        this ValueChecker<double> Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue <= ExpectedValue + Accuracy)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Нарушено условие\r\n   {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    точность:{Accuracy:e2}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Опорное значение</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public static DoubleValueChecker LessOrEqualsThan(
        this DoubleValueChecker Checker,
        double ExpectedValue,
        double Accuracy,
        string? Message = null)
    {
        if (Checker.ActualValue <= ExpectedValue + Accuracy)
            return Checker;

        var msg = Message.AddSeparator();
        var delta = ExpectedValue - Checker.ActualValue;
        FormattableString message = $"{msg}Нарушено условие\r\n   {Checker.ActualValue}\r\n >= {ExpectedValue}\r\n    точность:{Accuracy:e2}\r\n    err:{delta:e2}(err.rel:{delta / ExpectedValue})";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Значение больше (строго), чем указанное с задаваемой точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public static DoubleCompareCheckerWithAccuracy Greater(
        this ValueChecker<double> Checker,
        double ExpectedValue) =>
        new(Checker.ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: false);

    /// <summary>Значение больше, чем указанное с задаваемой точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public static DoubleCompareCheckerWithAccuracy GreaterOrEqual(
        this ValueChecker<double> Checker,
        double ExpectedValue) =>
        new(Checker.ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: false);

    /// <summary>Значение меньше (строго), чем указанное с задаваемой точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public static DoubleCompareCheckerWithAccuracy Less(
        this ValueChecker<double> Checker,
        double ExpectedValue) =>
        new(Checker.ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: true);

    /// <summary>Значение меньше, чем указанное с задаваемой точностью</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public static DoubleCompareCheckerWithAccuracy LessOrEqual(
        this ValueChecker<double> Checker,
        double ExpectedValue) =>
        new(Checker.ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: true);

    /// <summary>Проверить, что значение не является не-числом</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
    public static ValueChecker<double> IsNotNaN(
        this ValueChecker<double> Checker,
        string? Message = null)
    {
        if (double.IsNaN(Checker.ActualValue))
            throw new AssertFailedException($"{Message.AddSeparator()}Значение не является числом");
        return Checker;
    }

    /// <summary>Проверить, что значение является не-числом</summary>
    /// <param name="Checker">Объект проверки вещественного значения</param>
    /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
    public static ValueChecker<double> IsNaN(
        this ValueChecker<double> Checker,
        string? Message = null)
    {
        if (double.IsNaN(Checker.ActualValue))
            return Checker;
        throw new AssertFailedException($"{Message.AddSeparator()}Значение не не является числом");
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