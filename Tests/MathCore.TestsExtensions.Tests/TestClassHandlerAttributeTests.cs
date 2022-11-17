namespace MathCore.TestsExtensions.Tests;

[TestClassHandler("ResultHandler", HandlePassed = true)]
public class TestClassHandlerAttributeTests
{
    [TestMethod]
    public void ExecutionTest()
    {
        //Assert.Fail();
    }

    private static void ResultHandler(TestResult result)
    {
        Debug.WriteLine("Handler executed");
        result.LogOutput += "123";
    }
}