using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки действия</summary>
    public class AssertActionChecker
    {
        /// <summary>Проверяемое действие</summary>
        private readonly Action _Action;

        /// <summary>Инициализация нового объекта проверки действия</summary>
        /// <param name="action">Проверяемое действие</param>
        internal AssertActionChecker(Action action) => _Action = action ?? throw new ArgumentNullException(nameof(action));

        /// <summary>Проверка, что метод вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        /// <exception cref="AssertFailedException"></exception>
        public AssertEqualsChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                _Action();
            }
            catch (Exception exception)
            {
                return Assert.That.Value(exception).As<TException>("Получено исключение, отличное от ожидаемого");
            }
            throw new AssertFailedException(Message.AddSeparator());
        } 
    }

    /// <summary>Объект проверки параметрического действия</summary>
    /// <typeparam name="TValue">Тип параметра действия</typeparam>
    public class AssertActionChecker<TValue>
    {
        /// <summary>Проверяемое действие</summary>
        private readonly Action<TValue> _Action;

        /// <summary>Параметр действия</summary>
        private readonly TValue _Value;

        /// <summary>Инициализация нового объекта проверки действия</summary>
        /// <param name="action">Проверяемое действие</param>
        /// <param name="value">Параметр действия</param>
        internal AssertActionChecker(Action<TValue> action, TValue value)
        {
            _Action = action ?? throw new ArgumentNullException(nameof(action));
            _Value = value;
        }

        /// <summary>Проверка, что метод вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        /// <exception cref="AssertFailedException"></exception>
        public AssertEqualsChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                _Action(_Value);
            }
            catch (Exception exception)
            {
                return Assert.That.Value(exception).As<TException>("Получено исключение, отличное от ожидаемого");
            }
            throw new AssertFailedException(Message.AddSeparator());
        }
    }
}