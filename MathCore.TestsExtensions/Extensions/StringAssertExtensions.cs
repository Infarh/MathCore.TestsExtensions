namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки строк</summary>
    public static class StringAssertExtensions
    {
        /// <summary>Проверка строки</summary>
        /// <param name="that">Объект проверки</param>
        /// <param name="ActualValue">Проверяемая строка</param>
        /// <returns>Объект проверки строк</returns>
        public static ValueChecker<string> Value(StringAssert that, string ActualValue) => new ValueChecker<string>(ActualValue);
    }
}
