using System;
using System.Collections.Generic;
using System.Reflection;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки значения</summary>
    /// <typeparam name="T">Тип проверяемого значения</typeparam>
    public class ValueChecker<T>
    {
        /// <summary>Проверяемое значение</summary>
        public T ActualValue { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public Assert And => Assert.That;

        /// <summary>Инициализация нового объекта проверки значения</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        internal ValueChecker(T ActualValue) => this.ActualValue = ActualValue;

        /// <summary>Проверка значения на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        [NotNull]
        public virtual ValueChecker<T> IsEqual(T ExpectedValue, string Message = null)
        {
            Assert.AreEqual(
                ExpectedValue, ActualValue,
                "{0}Актуальное значение {1} не соответствует ожидаемому {2}",
                Message.AddSeparator(), ActualValue, ExpectedValue);
            return this;
        }

        /// <summary>Проверка значение на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Comparer">Объект сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        [NotNull]
        public ValueChecker<T> IsEqual(T ExpectedValue, IEqualityComparer<T> Comparer, string Message = null)
        {
            if(!Comparer.Equals(ActualValue, ExpectedValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Актуальное значение {ActualValue} не соответствует ожидаемому {ExpectedValue}");
            return this;
        }

        /// <summary>Метод сравнения значений</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Истина, если проверяемое и ожидаемое значения равны</returns>
        public delegate bool EqualityComparer(T ExpectedValue, T ActualValue);

        /// <summary>Проверка значение на эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Comparer">Метод сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        [NotNull]
        public ValueChecker<T> IsEqual(T ExpectedValue, EqualityComparer Comparer, string Message = null)
        {
            if(!Comparer(ExpectedValue, ActualValue))
                throw new AssertFailedException($"{Message.AddSeparator()}Актуальное значение {ActualValue} не соответствует ожидаемому {ExpectedValue}");
            return this;
        }

        /// <summary>Проверка значения на идентичность ожидаемому (при сравнении ссылок)</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        [NotNull]
        public ValueChecker<T> IsReferenceEquals(T ExpectedValue, string Message = null)
        {
            if(!ReferenceEquals(ActualValue, ExpectedValue))
                throw new AssertFailedException(
                    $"{Message.AddSeparator()}Объект актуального значения {ActualValue} не является ожидаемым {ExpectedValue} при сравнении ссылок");
            return this;
        }

        /// <summary>Проверка значения на не эквивалентность ожидаемому</summary>
        /// <param name="ExpectedValue">Не ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        [NotNull]
        public virtual ValueChecker<T> IsNotEqual(T ExpectedValue, string Message = null)
        {
            Assert.AreNotEqual(
                ExpectedValue, ActualValue,
                "{0}Актуальное значение {1} соответствует ожидаемому {2}",
                Message.AddSeparator(), ActualValue, ExpectedValue);
            return this;
        }

        /// <summary>Проверка значения на не идентичность ожидаемому (при сравнении ссылок)</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        [NotNull]
        public ValueChecker<T> IsNotReferenceEquals(T ExpectedValue, string Message = null)
        {
            if(ReferenceEquals(ActualValue, ExpectedValue))
                throw new AssertFailedException(
                    $"{Message.AddSeparator()}Объект актуального значения {ActualValue} является ожидаемым {ExpectedValue} при сравнении ссылок");
            return this;
        }

        /// <summary>Ссылка на значение должна быть пустой</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNull(string Message = null) => 
            Assert.IsNull(ActualValue, "{0}Ссылка на {1} не является пустой", Message.AddSeparator(), ActualValue);

        /// <summary>Значение, гарантированно не являющееся пустой ссылкой</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Значение, гарантированно не являющееся пустой ссылкой</returns>
        [NotNull]
        public T IsNotNull(string Message = null)
        {
            Assert.IsNotNull(ActualValue, "{0}Ссылка является пустой", Message.AddSeparator());
            return ActualValue;
        }

        /// <summary>Значение, гарантированно не являющееся пустой ссылкой</summary>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Исходный объект проверки значений</returns>
        [NotNull]
        public ValueChecker<T> AsNotNull(string Message = null)
        {
            IsNotNull(Message);
            return this;
        }

        /// <summary>Значение является значением указанного типа</summary>
        /// <param name="ExpectedType">Ожидаемый тип значения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Текущий объект проверки</returns>
        [NotNull]
        public ValueChecker<T> Is(Type ExpectedType, string Message = null)
        {
            IsNotNull(Message);
            Assert.IsInstanceOfType(
                ActualValue,
                ExpectedType,
                "{0}Значение {1} не является значением типа {2}",
                Message.AddSeparator(),
                ActualValue?.GetType(),
                ExpectedType);
            return this;
        }

        /// <summary>Значение является значением указанного типа</summary>
        /// <typeparam name="TExpectedType">Ожидаемый тип значения</typeparam>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Текущий объект проверки</returns>
        [NotNull]
        public ValueChecker<T> Is<TExpectedType>(string Message = null)
        {
            var expected_type = typeof(TExpectedType);
            IsNotNull(Message);
            Assert.IsInstanceOfType(
                ActualValue, 
                expected_type, 
                "{0}Значение {1} не является значением типа {2}",
                Message.AddSeparator(),
                ActualValue?.GetType(),
                expected_type);
            return this;
        }

        /// <summary>Объект является объектом более специфичного типа</summary>
        /// <typeparam name="TExpectedType">Тип наследника</typeparam>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Объект проверки типа наследника</returns>
        [NotNull]
        public ValueChecker<TExpectedType> As<TExpectedType>(string Message = null) where TExpectedType : T
        {
            var expected_type = typeof(TExpectedType);
            IsNotNull(Message);
            if (expected_type.GetTypeInfo().IsAssignableFrom(ActualValue.GetType().GetTypeInfo()))
                return new ValueChecker<TExpectedType>((TExpectedType) ActualValue);
            throw new AssertFailedException($"{Message.AddSeparator()}Значение {ActualValue?.GetType()} не является значением типа {expected_type}");
        }

        /// <summary>Объект является объектом более специфичного типа и можно определить производное значение указанным методом</summary>
        /// <typeparam name="TExpectedType">Тип наследника</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        /// <param name="Selector">Метод определения значения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Объект проверки производного значения</returns>
        [NotNull]
        public ValueChecker<TValue> As<TExpectedType, TValue>([NotNull] Func<TExpectedType, TValue> Selector, string Message = null) where TExpectedType : class, T
        {
            var expected_type = typeof(TExpectedType);
            IsNotNull(Message);
            Assert.IsInstanceOfType(
                ActualValue,
                expected_type,
                "{0}Значение {1} не является значением типа {2}",
                Message.AddSeparator(),
                ActualValue?.GetType(),
                expected_type);
            return new ValueChecker<TValue>(Selector((TExpectedType)ActualValue));
        }

        /// <summary>Проверка вложенного значения</summary>
        /// <typeparam name="TValue">Тип вложенного значения</typeparam>
        /// <param name="Selector">Метод определения вложенного значения</param>
        /// <returns>Объект проверки вложенного значения</returns>
        [NotNull]
        public NestedValueChecker<TValue, T> Where<TValue>([NotNull] Func<T, TValue> Selector) => new NestedValueChecker<TValue, T>(Selector(ActualValue), this);

        /// <summary>Проверка вложенного значения</summary>
        /// <typeparam name="TValue">Тип вложенного значения</typeparam>
        /// <param name="Selector">Метод определения вложенного значения</param>
        /// <param name="Checker">Метод проверки вложенного значения</param>
        /// <returns>Объект проверки текущего значения</returns>
        [NotNull]
        public ValueChecker<T> Where<TValue>([NotNull] Func<T, TValue> Selector, [NotNull] Action<ValueChecker<TValue>> Checker)
        {
            var value_checker = new ValueChecker<TValue>(Selector(ActualValue));
            Checker(value_checker);
            return this;
        }

        /// <summary>Набор проверок</summary>
        /// <param name="Checker">Метод проведения проверок</param>
        /// <returns>Исходный объект проверки</returns>
        [NotNull]
        public ValueChecker<T> AssertThat([NotNull] Action<ValueChecker<T>> Checker)
        {
            Checker(this);
            return this;
        }

        /// <summary>Проверка вложенной коллекции</summary>
        /// <param name="Selector">Метод определения вложенной коллекции</param>
        /// <param name="Checker">Метод проверки вложенной коллекции</param>
        /// <typeparam name="TItem">Тип элементов вложенной коллекции</typeparam>
        /// <returns>Исходный объект проверки</returns>
        [NotNull]
        public ValueChecker<T> WhereAll<TItem>([NotNull] Func<T, IEnumerable<TItem>> Selector, Action<ValueChecker<TItem>> Checker)
        {
            foreach (var item in Selector(ActualValue))
            {
                var item_checker = new ValueChecker<TItem>(item);
                Checker(item_checker);
            }
            return this;
        }

        /// <summary>Проверка вложенной коллекции</summary>
        /// <param name="Selector">Позиционный метод определения вложенной коллекции</param>
        /// <param name="Checker">Метод проверки вложенной коллекции</param>
        /// <typeparam name="TItem">Тип элементов вложенной коллекции</typeparam>
        /// <returns>Исходный объект проверки</returns>
        [NotNull]
        public ValueChecker<T> WhereAll<TItem>([NotNull] Func<T, IEnumerable<TItem>> Selector, Action<ValueChecker<TItem>, int> Checker)
        {
            var item_index = 0;
            foreach (var item in Selector(ActualValue))
            {
                var item_checker = new ValueChecker<TItem>(item);
                Checker(item_checker, item_index);
                item_index++;
            }
            return this;
        }

        /// <summary>Оператор неявного приведения типа объекта проверки к объекту проверяемого значения, разворачивающий значение</summary>
        /// <param name="Checker">Объект проверки</param>
        public static implicit operator T(ValueChecker<T> Checker) => Checker.ActualValue;
    }
}