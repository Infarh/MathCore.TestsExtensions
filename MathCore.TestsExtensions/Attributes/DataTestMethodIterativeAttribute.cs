// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Итерационное выполнение теста на основе данных с заданием числа итераций для набора статистики</summary>
/// <remarks>Инициализация итерационного теста на основе данных</remarks>
/// <param name="IterationsCount">Число итераций</param>
[AttributeUsage(AttributeTargets.Method)]
public class DataTestMethodIterativeAttribute(int IterationsCount) : TestMethodAttribute
{
    /// <summary>Число итераций повторения теста</summary>
    private readonly int _IterationsCount = IterationsCount;

    /// <summary>Остановить процесс выполнения теста при первом сбое</summary>
    public bool StopAtFirstFail { get; set; }

    /// <inheritdoc />
    public override TestResult[] Execute(ITestMethod TestMethod)
    {
        var results = new List<TestResult>();
        var stop_at_first_fail = this.StopAtFirstFail;
        for (var count = 0; count < _IterationsCount; count++)
        {
            var test_results = base.Execute(TestMethod);
            results.AddRange(test_results);
            if (stop_at_first_fail && test_results.Any(r => r.TestFailureException != null)) break;
        }

        return results.ToArray();
    }
}