using System.Globalization;
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Объект сравнения чисел с заданной точностью</summary>
public sealed class DoubleCompareCheckerWithAccuracy : IDisposable
{
    /// <summary>Проверяемое значение</summary>
    private readonly double _ActualValue;
    /// <summary>Ожидаемое значение</summary>
    private readonly double _ExpectedValue;
    /// <summary>Проверка на равенство</summary>
    private readonly bool _IsEquals;
    /// <summary>Проверка - меньше</summary>
    private readonly bool _IsLessChecking;

    /// <summary>Проверка выполнена</summary>
    private bool _IsChecked;

    /// <summary>Инициализация объекта сравнения</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="IsEquals">Проверять на равенство</param>
    /// <param name="IsLessChecking">Проверка - на "меньше"</param>
    internal DoubleCompareCheckerWithAccuracy(double ActualValue, double ExpectedValue, bool IsEquals = true, bool IsLessChecking = false)
    {
        _ActualValue = ActualValue;
        _ExpectedValue = ExpectedValue;
        _IsEquals = IsEquals;
        _IsLessChecking = IsLessChecking;
    }

    /// <summary>Сравнение с задаваемой точностью</summary>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCompareCheckerWithAccuracy WithAccuracy(double Accuracy, string? Message = null)
    {
        _IsChecked = true;
        if (_IsLessChecking)
        {
            if (_IsEquals)
                if (!(_ActualValue - Math.Abs(Accuracy) <= _ExpectedValue))
                {
                    var msg = Message.AddSeparator();
                    FormattableString message = $"{msg}Значени\r\n    {_ActualValue} должно быть меньше, либо равно\r\n    {_ExpectedValue}\r\n    при точности {Accuracy}.\r\n    delta:{_ExpectedValue - _ActualValue:e2}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }
                else
                if (!(_ActualValue - Math.Abs(Accuracy) < _ExpectedValue))
                {
                    var msg = Message.AddSeparator();
                    FormattableString message = $"{msg}Значение\r\n    {_ActualValue} должно быть меньше\r\n    {_ExpectedValue}\r\n    при точности {Accuracy}.\r\n    delta:{_ExpectedValue - _ActualValue:e2}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }
        }
        else
        {
            if (_IsEquals)
                if (!(_ActualValue + Math.Abs(Accuracy) >= _ExpectedValue))
                {
                    var msg = Message.AddSeparator();
                    FormattableString message = $"{msg}Значение\r\n    {_ActualValue} должно быть больше, либо равно\r\n    {_ExpectedValue}\r\n    при точности {Accuracy}.\r\n    delta:{_ExpectedValue - _ActualValue:e2}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }
                else 
                if (!(_ActualValue + Math.Abs(Accuracy) > _ExpectedValue))
                {
                    var msg = Message.AddSeparator();
                    FormattableString message = $"{msg}Значение\r\n    {_ActualValue} должно быть больше\r\n    {_ExpectedValue}\r\n    при точности {Accuracy}.\r\n    delta:{_ExpectedValue - _ActualValue:e2}";
                    throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }
        }
        return this;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Ожидание>")]
    void IDisposable.Dispose()
    {
        if (_IsChecked) return;
        Assert.Fail("Проверка значения не выполнена");
    }
}