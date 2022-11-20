#nullable enable
namespace MathCore.TestsExtensions.Tests;

[TestClass]
public class TestingExtensionsTests
{
    [TestMethod]
    public void AssertGreaterOrEqualsThan_Success()
    {
        const double actual   = -3.182280639625859E-14;
        const double expected = -1;

        actual.AssertGreaterOrEqualsThan(expected, 1e-14);
    }

    [TestMethod]
    public void AssertNotNull_Success()
    {
        var str          = (string?)"Hello World!";
        var not_null_str = str.AssertNotNull();
        _ = not_null_str.Length;
    }

    [TestMethod]
    public void AssertNotNull_Fail()
    {
        var str = (string?)null;

        try
        {
            var not_null_str = str.AssertNotNull();
            _ = not_null_str.Length;

            Assert.Fail();
        }
        catch (AssertFailedException) { }
        catch
        {
            Assert.Fail();
        }
    }
}