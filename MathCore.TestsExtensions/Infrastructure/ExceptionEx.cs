using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Infrastructure;

internal static class ExceptionEx
{
    extension<TException>(TException exception) where TException : Exception
    {
        public TException AddData<TValue>(string Key, TValue value)
        {
            if (value is null)
                exception.Data[Key] = null;
            else if (value.GetType().IsSerializable)
                exception.Data[Key] = value;
            return exception;
        }

        public TException AddData<TValue>(TValue value, [CallerArgumentExpression(nameof(value))] string? Key = null)
        {
            if (Key is not { Length: > 0 }) return exception;

            if (value is null)
                exception.Data[Key] = null;
            else if (value.GetType().IsSerializable)
                exception.Data[Key] = value;
            return exception;
        }
    }
}
