﻿using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Методы-расширения для объекта проверки строк</summary>
    public static class StringAssertExtensions
    {
        /// <summary>Проверка строки</summary>
        /// <param name="that">Объект проверки</param>
        /// <param name="ActualValue">Проверяемая строка</param>
        /// <returns>Объект проверки строк</returns>
        [NotNull]
        public static ValueChecker<string> Value([NotNull] StringAssert that, string ActualValue) => new ValueChecker<string>(ActualValue);
    }
}