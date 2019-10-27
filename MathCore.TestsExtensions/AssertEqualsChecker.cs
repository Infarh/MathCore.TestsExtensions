using System;
using System.Collections.Generic;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки значения</summary>
    /// <typeparam name="T">Тип проверяемого значения</typeparam>
    public sealed class AssertEqualsChecker<T>
    {
        /// <summary>Проверяемое значение</summary>
        private readonly T _ActualValue;

        /// <summary>Проверяемое значение</summary>
        public T Value => _ActualValue;

        /// <summary>Инициализация нового объекта проверки значения</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        internal AssertEqualsChecker(T ActualValue) => _ActualValue = ActualValue;

        /// <summary>Проверка значения на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(T ExpectedValue, string Message = null) => 
            Assert.AreEqual(ExpectedValue, _ActualValue,
                "{0}Актуальное значение {1} не соовтетствует ожидаемому {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка значение на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Comparer">Объект сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(T ExpectedValue, IEqualityComparer<T> Comparer, string Message = null) =>
            Assert.IsTrue(Comparer.Equals(_ActualValue, ExpectedValue),
                "{0}Актуальное значение {1} не соовтетствует ожидаемому {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Метод сравнения значений</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Истина, если проверяемое и ожидаемое значения равны</returns>
        public delegate bool EqualityComparer(T ExpectedValue, T ActualValue);

        /// <summary>Проверка значение на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Comparer">Метод сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(T ExpectedValue, EqualityComparer Comparer, string Message = null) =>
            Assert.IsTrue(Comparer(ExpectedValue, _ActualValue),
                "{0}Актуальное значение {1} не соовтетствует ожидаемому {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка значения на идентичность ожидаемому (при сравнении ссылок)</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsReferenceEquals(T ExpectedValue, string Message = null) =>
            Assert.IsTrue(ReferenceEquals(_ActualValue, ExpectedValue),
                "{0}Объект актуального значения {1} не является ожидаемым {2} при сравнении ссылок",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка значения на не эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Не ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNotEqual(T ExpectedValue, string Message = null) => 
            Assert.AreNotEqual(ExpectedValue, _ActualValue,
                "{0}Актуальное значение {1} соовтетствует ожидаемому {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка значения на не идентичность ожидаемому (при сравнении ссылок)</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNotReferenceEquals(T ExpectedValue, string Message = null) => 
            Assert.IsFalse(ReferenceEquals(_ActualValue, ExpectedValue),
                "{0}Объект актуального значения {1} является ожидаемым {2} при сравнении ссылок",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Ссылка на значение должна быть пустой</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNull(string Message = null) => 
            Assert.IsNull(_ActualValue, "{0}Ссылка на {1} не является пустой", Message.AddSeparator(), _ActualValue);

        /// <summary>Значение, гарантированно не являющееся пустой ссылкой</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Значение, гарантированно не являющееся пустой ссылкой</returns>
        [NotNull]
        public T IsNotNull(string Message = null)
        {
            Assert.IsNotNull(_ActualValue, "{0}Ссылка является пустой", Message.AddSeparator());
            return _ActualValue;
        }

        /// <summary>Значение является значением указанного типа</summary>
        /// <param name="ExpectedType">Ожидаемый тип значения</param>
        /// <returns>Текущий объект проверки</returns>
        [NotNull]
        public AssertEqualsChecker<T> Is(Type ExpectedType)
        {
            Assert.IsInstanceOfType(_ActualValue, ExpectedType);
            return this;
        }

        /// <summary>Значение является значением указанного типа</summary>
        /// <typeparam name="TExpectedType">Ожидаемый тип значения</typeparam>
        /// <returns>Текущий объект проверки</returns>
        [NotNull]
        public AssertEqualsChecker<T> Is<TExpectedType>()
        {
            Assert.IsInstanceOfType(_ActualValue, typeof(TExpectedType));
            return this;
        }

        /// <summary>ОБъект является объектом более специфичного типа</summary>
        /// <typeparam name="TExpectedType">Тип наследника</typeparam>
        /// <returns>Объект проверки типа наследника</returns>
        [NotNull]
        public AssertEqualsChecker<TExpectedType> As<TExpectedType>() where TExpectedType : class, T
        {
            Assert.IsInstanceOfType(_ActualValue, typeof(TExpectedType));
            return new AssertEqualsChecker<TExpectedType>((TExpectedType)_ActualValue);
        }

        /// <summary>Оператор неявного приведения типа объектапроверки к объекту проверяемого значения, разорачивающий значение</summary>
        /// <param name="Checker">Объект проверки</param>
        public static implicit operator T(AssertEqualsChecker<T> Checker) => Checker._ActualValue;
    }
}