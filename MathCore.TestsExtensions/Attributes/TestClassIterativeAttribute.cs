
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Итерационный тестовый класс</summary>
/// <remarks>Инициализация итерационного теста</remarks>
/// <param name="IterationsCount">Число итераций</param>
[AttributeUsage(AttributeTargets.Class)]
public class TestClassIterativeAttribute(int IterationsCount) : TestClassAttribute
{
    /// <summary>Число итераций</summary>
    private readonly int _IterationsCount = IterationsCount;

    /// <summary>Остановить процесс выполнения теста при первом сбое</summary>
    public bool StopAtFirstFail { get; set; }

    /// <inheritdoc />
    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute TestMethodAttribute)
    {
        var attribute = TestMethodAttribute as TestMethodIterativeAttribute ?? new TestMethodIterativeAttribute(_IterationsCount);
        attribute.StopAtFirstFail = StopAtFirstFail;
        return attribute;
    }
}