using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests;

[TestClass]
public class TestMethodHandlerAttributeTests
{
    [TestMethodHandler(nameof(ResultHandler), HandlePassed = true)]
    public void ExecutionTest()
    {
        //Assert.Fail();
    }

    private static void ResultHandler(TestResult result)
    {
        
    }
}
