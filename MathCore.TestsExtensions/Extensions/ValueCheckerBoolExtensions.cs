// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки логических значений</summary>
public static class ValueCheckerBoolExtensions
{
    /// <param name="Checker">Объект проверки логического значения</param>
    extension(ValueChecker<bool> Checker)
    {
        /// <summary>Проверка значения на истинность</summary>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public ValueChecker<bool> IsTrue(string? Message = null)
        {
            Assert.IsTrue(Checker.ActualValue, $"{Message.AddSeparator()}Значение не истинно");
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public ValueChecker<bool> IsFalse(string? Message = null)
        {
            Assert.IsFalse(Checker.ActualValue, $"{Message.AddSeparator()}Значение не ложно");
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Value">Значение для проверки</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public ValueChecker<bool> Is(bool Value, string? Message = null)
        {
            if (Value)
                Assert.IsTrue(Checker.ActualValue, $"{Message.AddSeparator()}Значение не истинно");
            else
                Assert.IsFalse(Checker.ActualValue, $"{Message.AddSeparator()}Значение не ложно");

            return Checker;
        }
    }
}