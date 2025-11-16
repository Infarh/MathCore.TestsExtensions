using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

[AttributeUsage(AttributeTargets.Method)]
public class TestMethodHandlerAttribute(string? ExceptionHandlerMethod, bool HandlePassed = false, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1) : TestMethodAttribute(callerFilePath, callerLineNumber)
{
    public TestMethodHandlerAttribute(bool HandlePassed = false, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1) : this(null, HandlePassed, callerFilePath, callerLineNumber) { }

    public string? ExceptionHandlerMethod { get; set; } = ExceptionHandlerMethod;

    public bool HandlePassed { get; set; } = HandlePassed;

    public override async Task<TestResult[]> ExecuteAsync(ITestMethod Method)
    {

        if (ExceptionHandlerMethod is not { Length: > 0 } handler_method_name)
            return await base.ExecuteAsync(Method);

        var test_class = Method.MethodInfo.DeclaringType ?? throw new InvalidOperationException("Невозможно определить класс модульного теста");

        //const BindingFlags public_instance = BindingFlags.Public | BindingFlags.Instance;
        //const BindingFlags private_instance = BindingFlags.NonPublic | BindingFlags.Instance;
        const BindingFlags public_static = BindingFlags.Public | BindingFlags.Static;
        const BindingFlags private_static = BindingFlags.NonPublic | BindingFlags.Static;

        var test_result_type = typeof(TestResult);
        var handler_method_info =
            //test_class.GetMethod(handler_method_name, public_instance, null, new[] { test_result_type }, null) ??
            //test_class.GetMethod(handler_method_name, private_instance, null, new[] { test_result_type }, null) ??
            test_class.GetMethod(handler_method_name, public_static, null, [test_result_type], null) ??
            test_class.GetMethod(handler_method_name, private_static, null, [test_result_type], null);

        if (handler_method_info is null)
            return await base.ExecuteAsync(Method);

        var result = await base.ExecuteAsync(Method);

        var results_to_process = HandlePassed
            ? result
            : result.Where(r => r.Outcome != UnitTestOutcome.Passed);

        foreach (var test_result in results_to_process)
            handler_method_info.Invoke(null, [test_result]);

        return result;
    }
}