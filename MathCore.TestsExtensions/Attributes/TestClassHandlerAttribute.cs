namespace Microsoft.VisualStudio.TestTools.UnitTesting;

[AttributeUsage(AttributeTargets.Class)]
public class TestClassHandlerAttribute(string ExceptionHandlerMethod) : TestClassAttribute
{
    public string? ExceptionHandlerMethod { get; set; } = ExceptionHandlerMethod;

    public bool HandlePassed { get; set; }

    public TestClassHandlerAttribute() : this(null!) { }

    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute Attribute)
    {
        if(ExceptionHandlerMethod is { Length: > 0 } method_name)
            if (Attribute is TestMethodHandlerAttribute handler_attribute)
            {
                if (handler_attribute.ExceptionHandlerMethod is not { Length: > 0 })
                {
                    handler_attribute.ExceptionHandlerMethod = method_name;
                    handler_attribute.HandlePassed = HandlePassed;
                }
            }
            else
                Attribute = new TestMethodHandlerAttribute
                {
                    ExceptionHandlerMethod = method_name,
                    HandlePassed = HandlePassed,
                };

        return base.GetTestMethodAttribute(Attribute);
    }
}