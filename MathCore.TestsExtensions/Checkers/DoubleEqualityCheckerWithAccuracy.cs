// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Объект проверки равенства чисел с заданной точностью</summary>
public sealed class DoubleEqualityCheckerWithAccuracy : IDisposable
{
    /// <summary>Проверяемое значение</summary>
    private readonly double _ActualValue;
    /// <summary>Значение, с которым требуется провести сравнение</summary>
    private readonly double _ExpectedValue;
    /// <summary>Проверка на неравенство</summary>
    private readonly bool _Not;

    /// <summary>Проверка выполнена</summary>
    private bool _IsChecked;

    /// <summary>Инициализация нового объекта сравнения чисел с заданной точностью</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="ExpectedValue">Требуемое значение</param>
    /// <param name="Not">Проверять на неравенство</param>
    internal DoubleEqualityCheckerWithAccuracy(double ActualValue, double ExpectedValue, bool Not = false)
    {
        _ActualValue = ActualValue;
        _ExpectedValue = ExpectedValue;
        _Not = Not;
    }

    /// <summary>Проверка с задаваемой точностью</summary>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
    public DoubleEqualityCheckerWithAccuracy WithAccuracy(double Accuracy, string? Message = null)
    {
        if (Accuracy is double.NaN)
            throw new ArgumentException("Значение точности не может быть равно NaN");

        _IsChecked = true;
        if (double.IsNaN(_ExpectedValue) && double.IsNaN(_ActualValue))
            return this;

        if (double.IsNaN(_ActualValue))
            throw new AssertFailedException($"{Message.AddSeparator()}Полученное значение было равно NaN");

        var delta = Math.Abs(_ExpectedValue - _ActualValue);
        var delta_rel = delta / _ExpectedValue;
        if (_Not)
        {
            if (delta < Accuracy)
            {
                var msg = Message.AddSeparator();
                FormattableString message = $"{msg}  actual:{_ActualValue} ==\r\nexpected:{_ExpectedValue}\r\n      err:{delta:e2}(rel:{delta_rel:e2})\r\n      eps:{Accuracy}";
                throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                   .AddData("Expected", _ExpectedValue)
                   .AddData("Actual", _ActualValue)
                   .AddData(Accuracy);
            }

            //Assert.AreNotEqual(
            //    _ExpectedValue,
            //    _ActualValue,
            //    Accuracy,
            //    "{0}err:{1}(rel:{2})",
            //    Message.AddSeparator(),
            //    delta.ToString("e2", CultureInfo.InvariantCulture),
            //    delta_rel.ToString("e2", CultureInfo.InvariantCulture));
        }
        else
        {
            if (delta > Accuracy)
            {
                var msg = Message.AddSeparator();
                FormattableString message = $"{msg} actual:{_ActualValue} !=\r\nexpected:{_ExpectedValue}\r\n      err:{delta:e2}(rel:{delta_rel:e2})\r\n      eps:{Accuracy}";
                throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                   .AddData("Expected", _ExpectedValue)
                   .AddData("Actual", _ActualValue)
                   .AddData(Accuracy);
            }

            //Assert.AreEqual(
            //    _ExpectedValue,
            //    _ActualValue,
            //    Accuracy,
            //    "{0}err:{1}(rel:{2})",
            //    Message.AddSeparator(),
            //    delta.ToString("e2", CultureInfo.InvariantCulture),
            //    delta_rel.ToString("e2", CultureInfo.InvariantCulture));
        };
        return this;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Ожидание>")]
    void IDisposable.Dispose()
    {
        if (_IsChecked) return;
        Assert.Fail($"Проверка на {(_Not ? "не" : null)}равенство не выполнена");
    }
}