using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Итерационный тестовый класс</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassIterativeAttribute : TestClassAttribute
    {
        /// <summary>Число итераций</summary>
        private readonly int _IterationsCount;

        /// <summary>Инициализация итерационного теста</summary>
        /// <param name="IterationsCount">Число итераций</param>
        public TestClassIterativeAttribute(int IterationsCount) => _IterationsCount = IterationsCount;

        /// <inheritdoc />
        public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute TestMethodAttribute) => 
            TestMethodAttribute is TestMethodIterativeAttribute 
                ? TestMethodAttribute 
                : new TestMethodIterativeAttribute(_IterationsCount);
    }
}