using MathCore.Tests.Annotations;
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки логических значений</summary>
    public static class ValueCheckerBoolExtensions
    {
        /// <summary>Проверка значения на истинность</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        [NotNull]
        public static ValueChecker<bool> IsTrue([NotNull] this ValueChecker<bool> Checker, string Message = null)
        {
            Assert.IsTrue(Checker.ActualValue, "{0}Значение не истинно", Message.AddSeparator());
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        [NotNull]
        public static ValueChecker<bool> IsFalse([NotNull] this ValueChecker<bool> Checker, string Message = null)
        {
            Assert.IsFalse(Checker.ActualValue, "{0}Значение не ложно", Message.AddSeparator());
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Value">Значение для проверки</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        [NotNull]
        public static ValueChecker<bool> Is([NotNull] this ValueChecker<bool> Checker, bool Value, string Message = null)
        {
            if (Value)
                Assert.IsTrue(Checker.ActualValue, "{0}Значение не истинно", Message.AddSeparator());
            else
                Assert.IsFalse(Checker.ActualValue, "{0}Значение не ложно", Message.AddSeparator());
            return Checker;
        }
    }
}