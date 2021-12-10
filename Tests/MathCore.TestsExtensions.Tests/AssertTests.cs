using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    public abstract class AssertTests
    {
        protected static AssertFailedException IsAssertFail(Action AssertAction) => ExpectedException<AssertFailedException>(AssertAction);

        protected static TException ExpectedException<TException>(Action AssertAction) where TException : Exception
        {
            TException expected_exception = null;
            try
            {
                AssertAction();
            }
            catch (TException exception)
            {
                expected_exception = exception;
            }
            if (expected_exception is null)
                throw new AssertFailedException($"Требуемое исключение типа {typeof(TException).Name} выброшено не было");

            return expected_exception;
        }
    }
}