namespace MathCore.TestsExtensions.Tests;

[TestClass]
public class TestingExtensionsTests
{
    [TestMethod]
    public void AssertGreaterOrEqualsThan_Success()
    {
        const double actual = -3.182280639625859E-14;
        const double expected = -1;

        actual.AssertGreaterOrEqualsThan(expected, 1e-14);
    }

    [TestMethod]
    public void AssertGreaterOrEqualsThan_Fail()
    {

    }
}
