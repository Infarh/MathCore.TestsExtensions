// ReSharper disable once CheckNamespace
namespace Microsoft.VisualStudio.TestTools.UnitTesting.Attributes;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
#if SYSTEM_PRIVATE_CORELIB
public
#else
public
#endif
    sealed class NotNullIfNotNullAttribute : Attribute
{
    /// <summary>Initializes the attribute with the associated parameter name.</summary>
    /// <param name="ParameterName">
    /// The associated parameter name.  The output will be non-null if the argument to the parameter specified is non-null.
    /// </param>
    public NotNullIfNotNullAttribute(string ParameterName) => this.ParameterName = ParameterName;

    /// <summary>Gets the associated parameter name.</summary>
    public string ParameterName { get; }
}
