using System;
using System.Globalization;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки равенства/неравенства чисел с плавающей запятой двойной точности</summary>
    public sealed class DoubleValueChecker : ValueChecker<double>
    {
        /// <summary>Инициализация нового объекта сравнения чисел с плавающей запятой</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        internal DoubleValueChecker(double ActualValue) : base(ActualValue) { }

        /// <summary>Сравнение с ожидаемым значением</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки вещественного значения</returns>
        [NotNull]
        public override ValueChecker<double> IsEqual(double ExpectedValue, string Message = null)
        {
            Assert.AreEqual(
                ExpectedValue, ActualValue,
                "{0}error:{1}",
                Message.AddSeparator(), 
                Math.Abs(ExpectedValue - ActualValue).ToString("e2", CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>Проверка на неравенство</summary>
        /// <param name="ExpectedValue">Не ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки вещественного значения</returns>
        [NotNull]
        public override ValueChecker<double> IsNotEqual(double ExpectedValue, string Message = null)
        {
            Assert.AreNotEqual(
                ExpectedValue, ActualValue,
                "{0}error:{1}",
                Message.AddSeparator(), 
                Math.Abs(ExpectedValue - ActualValue).ToString("e2", CultureInfo.InvariantCulture));
            return this;
        }
    }
}