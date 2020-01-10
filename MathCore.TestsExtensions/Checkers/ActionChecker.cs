using System;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки действия</summary>
    public class ActionChecker
    {
        /// <summary>Проверяемое действие</summary>
        public Action Action { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public Assert And => Assert.That;

        /// <summary>Инициализация нового объекта проверки действия</summary>
        /// <param name="action">Проверяемое действие</param>
        internal ActionChecker(Action action) => Action = action ?? throw new ArgumentNullException(nameof(action));

        /// <summary>Проверка, что метод вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        public ValueChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                Action();
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
    public class ActionChecker<TValue>
    {
        /// <summary>Проверяемое действие</summary>
        public Action<TValue> Action { get; }

        /// <summary>Параметр действия</summary>
        public TValue Value { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public Assert And => Assert.That;

        /// <summary>Инициализация нового объекта проверки действия</summary>
        /// <param name="action">Проверяемое действие</param>
        /// <param name="value">Параметр действия</param>
        internal ActionChecker(Action<TValue> action, TValue value)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Value = value;
        }

        /// <summary>Проверка, что метод вызывает исключение указанного типа</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
        /// <returns>Объект проверки исключения</returns>
        /// <exception cref="AssertFailedException">Возникает в случае, если тестируемый метод не сгенерировал никакого исключения</exception>
        public ValueChecker<TException> Throw<TException>(string Message = null) where TException : Exception
        {
            try
            {
                Action(Value);
            }
            catch (Exception exception)
            {
                return Assert.That.Value(exception).As<TException>($"{Message.AddSeparator()}Получено исключение, отличное от ожидаемого");
            }
            throw new AssertFailedException(Message.AddSeparator());
        }
    }
}