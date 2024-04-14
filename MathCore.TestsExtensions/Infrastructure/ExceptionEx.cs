using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Infrastructure;

internal static class ExceptionEx
{
    public static TException AddData<TException, TValue>(this TException exception, string Key, TValue value)
        where TException : Exception
    {
        if (value is null)
            exception.Data[Key] = null;
        else if (value.GetType().IsSerializable)
            exception.Data[Key] = value;
        return exception;
    }

    public static TException AddData<TException, TValue>(this TException exception, TValue value, [CallerArgumentExpression(nameof(value))] string? Key = null)
        where TException : Exception
    {
        if (Key is not { Length: > 0 }) return exception;

        if (value is null)
            exception.Data[Key] = null;
        else if (value.GetType().IsSerializable)
            exception.Data[Key] = value;
        return exception;
    }
}
