using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки функции</summary>
    /// <typeparam name="TResult">Тип значения функции</typeparam>
    public class AssertFunctionChecker<TResult>
    {
        /// <summary>Проверяемая функция</summary>
        private readonly Func<TResult> _Function;

        /// <summary>Инициализация нового объекта проверки функции</summary>
        /// <param name="function">Проверяемое действие</param>
        internal AssertFunctionChecker(Func<TResult> function) => _Function = function ?? throw new ArgumentNullException(nameof(function));

        /// <summary>Проверка, что функция вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        public AssertEqualsChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                _ = _Function();
            }
            catch (Exception exception)
            {
                return Assert.That.Value(exception).As<TException>("Получено исключение, отличное от ожидаемого");
            }
            throw new AssertFailedException(Message.AddSeparator());
        }
    }

    /// <summary>Объект проверки параметрической функции</summary>
    /// <typeparam name="TValue">Тип параметра функции</typeparam>
    /// <typeparam name="TResult">Тип значения функции</typeparam>
    public class AssertFunctionChecker<TValue, TResult>
    {
        /// <summary>Проверяемая функция</summary>
        private readonly Func<TValue, TResult> _Function;

        /// <summary>Параметр функции</summary>
        private readonly TValue _Value;

        /// <summary>Инициализация нового объекта проверки функции</summary>
        /// <param name="function">Проверяемая функция</param>
        /// <param name="value">Параметр функции</param>
        internal AssertFunctionChecker(Func<TValue, TResult> function, TValue value)
        {
            _Function = function ?? throw new ArgumentNullException(nameof(function));
            _Value = value;
        }

        /// <summary>Проверка, что функция вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        /// <exception cref="AssertFailedException"></exception>
        public AssertEqualsChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                _ = _Function(_Value);
            }
            catch (Exception exception)
            {
                return Assert.That.Value(exception).As<TException>("Получено исключение, отличное от ожидаемого");
            }
            throw new AssertFailedException(Message.AddSeparator());
        }
    }
}
