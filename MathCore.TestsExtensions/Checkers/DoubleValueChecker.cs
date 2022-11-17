// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Объект проверки равенства/неравенства чисел с плавающей запятой двойной точности</summary>
public sealed class DoubleValueChecker : ValueChecker<double>
{
    /// <summary>Инициализация нового объекта сравнения чисел с плавающей запятой</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    internal DoubleValueChecker(double ActualValue) : base(ActualValue) { }

    /// <summary>Сравнение с ожидаемым значением</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public override ValueChecker<double> IsEqual(double ExpectedValue, string? Message = null)
    {
        if (Equals(ExpectedValue, ActualValue)) return this;

        FormattableString msg = $"{Message.AddSeparator()} получено значение\r\n    {ActualValue:F18} не равно ожидаемому\r\n    {ExpectedValue:F18}\r\n    err:{ExpectedValue - ActualValue:e3}(rel.err:{(ExpectedValue - ActualValue) / ExpectedValue:e3})";
        throw new AssertFailedException(msg.ToString(CultureInfo.InvariantCulture))
           .AddData("Expected", ExpectedValue)
           .AddData("Actual", ActualValue);
    }

    /// <summary>Проверка на неравенство</summary>
    /// <param name="ExpectedValue">Не ожидаемое значение</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public override ValueChecker<double> IsNotEqual(double ExpectedValue, string? Message = null)
    {
        if (!Equals(ExpectedValue, ActualValue)) return this;

        FormattableString msg = $"{Message.AddSeparator()} полученное значение\r\n    {ActualValue:F18} равно ожидаемому\r\n    {ExpectedValue:F18}\r\n    err:{ExpectedValue - ActualValue:e3}(rel.err:{(ExpectedValue - ActualValue) / ExpectedValue:e3})";
        throw new AssertFailedException(msg.ToString(CultureInfo.InvariantCulture))
           .AddData("Expected", ExpectedValue)
           .AddData("Actual", ActualValue);
    }
}