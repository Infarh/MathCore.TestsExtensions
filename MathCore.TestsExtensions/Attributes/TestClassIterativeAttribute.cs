
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Итерационный тестовый класс</summary>
[AttributeUsage(AttributeTargets.Class)]
public class TestClassIterativeAttribute : TestClassAttribute
{
    /// <summary>Число итераций</summary>
    private readonly int _IterationsCount;

    /// <summary>Остановить процесс выполнения теста при первом сбое</summary>
    public bool StopAtFirstFail { get; set; }

    /// <summary>Инициализация итерационного теста</summary>
    /// <param name="IterationsCount">Число итераций</param>
    public TestClassIterativeAttribute(int IterationsCount) => _IterationsCount = IterationsCount;

    /// <inheritdoc />
    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute TestMethodAttribute)
    {
        var attribute = TestMethodAttribute as TestMethodIterativeAttribute ?? new TestMethodIterativeAttribute(_IterationsCount);
        attribute.StopAtFirstFail = StopAtFirstFail;
        return attribute;
    }
}