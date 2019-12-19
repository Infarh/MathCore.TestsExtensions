using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Итерационное выполнение теста с заданием числа итераций для набора статистики</summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodIterativeAttribute : TestMethodAttribute
    {
        /// <summary>Число итераций повторения теста</summary>
        private readonly int _IterationsCount;

        /// <summary>Инициализация итерационного теста</summary>
        /// <param name="IterationsCount">Число итераций</param>
        public TestMethodIterativeAttribute(int IterationsCount) => _IterationsCount = IterationsCount;

        /// <inheritdoc />
        public override TestResult[] Execute(ITestMethod TestMethod)
        {
            var results = new List<TestResult>();
            for (var count = 0; count < _IterationsCount; count++)
            {
                var test_results = base.Execute(TestMethod);
                results.AddRange(test_results);
                if (test_results.Any(r => r.TestFailureException != null)) break;
            }

            return results.ToArray();
        }
    }
}
