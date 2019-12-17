namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки логических значений</summary>
    public sealed class BoolValueChecker
    {
        /// <summary>Проверяемое значение</summary>
        private readonly bool _ActualValue;

        /// <summary>Инициализация нового объекта проверки логического значения</summary>
        /// <param name="ExpectedValue">Проверяемое значение</param>
        internal BoolValueChecker(bool ExpectedValue) => _ActualValue = ExpectedValue;

        /// <summary>Проверка значения на истину</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsTrue(string Message = null) => Assert.IsTrue(_ActualValue, "{0}Значение не истинно", Message.AddSeparator());

        /// <summary>Проверка значения на ложь</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsFalse(string Message = null) => Assert.IsFalse(_ActualValue, "{0}Значение не ложно", Message.AddSeparator());

        /// <summary>Сравнение с ожидаемым значением</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsEqual(bool ExpectedValue, string Message = null) => Assert.That.Value(_ActualValue).IsEqual(ExpectedValue, Message);
    }
}