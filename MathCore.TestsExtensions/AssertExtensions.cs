namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Класс методов-расширений для объекта-помощника проверки <see cref="Assert.That"/></summary>
    public static class AssertExtensions
    {
        /// <summary>Проверка значения</summary>
        /// <typeparam name="T">ТИп проверяемого значения</typeparam>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки</returns>
        public static AssertEqualsChecker<T> Value<T>(this Assert that, T ActualValue) => new AssertEqualsChecker<T>(ActualValue);

        /// <summary>Проверка вещественного значения</summary>
        /// <param name="that">Объект-помощник проверки</param>
        /// <param name="ActualValue">Првоеряемое значение</param>
        /// <returns>Объект проверки вещественных значений</returns>
        public static AssertDoubleEqualsChecker Value(this Assert that, double ActualValue) => new AssertDoubleEqualsChecker(ActualValue);

        /// <summary>Проверка целочисленного значения</summary>
        /// <param name="that">Объект-помошник проверки</param>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <returns>Объект проверки целочисленных значений</returns>
        public static AssertIntEqualsChecker Value(this Assert that, int ActualValue) => new AssertIntEqualsChecker(ActualValue);
    }
}
