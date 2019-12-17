using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки значения</summary>
    public static class ValueCheckerExtensions
    {
        /// <summary>Проверка значения на истинность</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public static ValueChecker<bool> IsTrue(this ValueChecker<bool> Checker, string Message = null)
        {
            Assert.IsTrue(Checker.ActualValue, "{0}Значение не истинно", Message.AddSeparator());
            return Checker;
        }

        /// <summary>Проверка значения на ложно</summary>
        /// <param name="Checker">Объект проверки логического значения</param>
        /// <param name="Message">Сообщение, выводимое в случае нарушения проверки</param>
        /// <returns>Исходный объект проверки логического значения</returns>
        public static ValueChecker<bool> IsFalse(this ValueChecker<bool> Checker, string Message = null)
        {
            Assert.IsFalse(Checker.ActualValue, "{0}Значение не истинно", Message.AddSeparator());
            return Checker;
        }
    }
}
