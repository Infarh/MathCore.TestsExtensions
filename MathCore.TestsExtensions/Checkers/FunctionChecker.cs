using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки функции</summary>
    /// <typeparam name="TResult">Тип значения функции</typeparam>
    public class FunctionChecker<TResult>
    {
        /// <summary>Проверяемая функция</summary>
        public Func<TResult> Function { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public Assert And => Assert.That;

        /// <summary>Инициализация нового объекта проверки функции</summary>
        /// <param name="function">Проверяемое действие</param>
        internal FunctionChecker(Func<TResult> function) => Function = function ?? throw new ArgumentNullException(nameof(function));

        /// <summary>Проверка, что функция вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        public ValueChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                _ = Function();
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
    public class FunctionChecker<TValue, TResult>
    {
        /// <summary>Проверяемая функция</summary>
        public Func<TValue, TResult> Function { get; }

        /// <summary>Параметр функции</summary>
        public TValue Value { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public Assert And => Assert.That;

        /// <summary>Инициализация нового объекта проверки функции</summary>
        /// <param name="function">Проверяемая функция</param>
        /// <param name="value">Параметр функции</param>
        internal FunctionChecker(Func<TValue, TResult> function, TValue value)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Value = value;
        }

        /// <summary>Проверка, что функция вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        /// <exception cref="AssertFailedException">Возникает в случае, если тестируемая функция не сгенерировала исключения</exception>
        public ValueChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                _ = Function(Value);
            }
            catch (Exception exception)
            {
                return Assert.That.Value(exception).As<TException>($"{Message.AddSeparator()}Получено исключение, отличное от ожидаемого");
            }
            throw new AssertFailedException(Message.AddSeparator());
        }
    }
}
