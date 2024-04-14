// ReSharper disable once CheckNamespace

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Attributes;

/// <summary>Specifies that the output will be non-null if the named parameter is non-null.</summary>
/// <remarks>Initializes the attribute with the associated parameter name.</remarks>
/// <param name="parameterName">
/// The associated parameter name.  The output will be non-null if the argument to the parameter specified is non-null.
/// </param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
internal sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute
{

    /// <summary>Gets the associated parameter name.</summary>
    public string ParameterName { get; } = parameterName;
}