﻿
// ReSharper disable UnusedType.Global

using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable IntroduceOptionalParameters.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace MathCore.Tests.Annotations;

/// <summary>
/// Indicates that the value of the marked element could be <c>null</c> sometimes,
/// so the check for <c>null</c> is necessary before its usage
/// </summary>
/// <example><code>
/// [CanBeNull] public object Test() { return null; }
/// public void UseTest() {
///   var p = Test();
///   var s = p.ToString(); // Warning: Possible 'System.NullReferenceException'
/// }
/// </code></example>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
internal sealed class CanBeNullAttribute : Attribute { }

/// <summary>
/// Can be applied to symbols of types derived from IEnumerable as well as to symbols of Task
/// and Lazy classes to indicate that the value of a collection item, of the Task.Result property
/// or of the Lazy.Value property can never be null.
/// </summary>
/// <example><code>
/// public void Foo([ItemNotNull]List&lt;string&gt; books)
/// {
///   foreach (var book in books) {
///     if (book != null) // Warning: Expression is always true
///      Console.WriteLine(book.ToUpper());
///   }
/// }
/// </code></example>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
internal sealed class ItemNotNullAttribute : Attribute { }

/// <summary>
/// Can be applied to symbols of types derived from IEnumerable as well as to symbols of Task
/// and Lazy classes to indicate that the value of a collection item, of the Task.Result property
/// or of the Lazy.Value property can be null.
/// </summary>
/// <example><code>
/// public void Foo([ItemCanBeNull]List&lt;string&gt; books)
/// {
///   foreach (var book in books)
///   {
///     // Warning: Possible 'System.NullReferenceException'
///     Console.WriteLine(book.ToUpper());
///   }
/// }
/// </code></example>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
internal sealed class ItemCanBeNullAttribute : Attribute { }

/// <summary>
/// Indicates that the marked method builds string by format pattern and (optional) arguments.
/// Parameter, which contains format string, should be given in constructor. The format string
/// should be in <see cref="string.Format(IFormatProvider,string,object[])"/>-like form
/// </summary>
/// <example><code>
/// [StringFormatMethod("message")]
/// public void ShowError(string message, params object[] args) { /* do something */ }
/// public void Foo() {
///   ShowError("Failed: {0}"); // Warning: Non-existing argument in format string
/// }
/// </code></example>
/// <param name="formatParameterName">
/// Specifies which parameter of an annotated method should be treated as format-string
/// </param>
[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
internal sealed class StringFormatMethodAttribute(string formatParameterName) : Attribute
{
    public string FormatParameterName { get; } = formatParameterName;
}

/// <summary>
/// Indicates that the function argument should be string literal and match one
/// of the parameters of the caller function. For example, ReSharper annotates
/// the parameter of <see cref="System.ArgumentNullException"/>
/// </summary>
/// <example><code>
/// public void Foo(string param) {
///   if (param == null)
///     throw new ArgumentNullException("par"); // Warning: Cannot resolve symbol
/// }
/// </code></example>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class InvokerParameterNameAttribute : Attribute { }

/// <summary>
/// Indicates that the method is contained in a type that implements
/// <see cref="System.ComponentModel.INotifyPropertyChanged"/> interface
/// and this method is used to notify that some property value changed
/// </summary>
/// <remarks>
/// The method should be non-static and conform to one of the supported signatures:
/// <list>
/// <item><c>NotifyChanged(string)</c></item>
/// <item><c>NotifyChanged(params string[])</c></item>
/// <item><c>NotifyChanged{T}(Expression{Func{T}})</c></item>
/// <item><c>NotifyChanged{T,U}(Expression{Func{T,U}})</c></item>
/// <item><c>SetProperty{T}(ref T, T, string)</c></item>
/// </list>
/// </remarks>
/// <example><code>
/// public class Foo : INotifyPropertyChanged {
///   public event PropertyChangedEventHandler PropertyChanged;
///   [NotifyPropertyChangedInvocator]
///   protected virtual void NotifyChanged(string propertyName) { ... }
///
///   private string _name;
///   public string Name {
///     get { return _name; }
///     set { _name = value; NotifyChanged("LastName"); /* Warning */ }
///   }
/// }
/// </code>
/// Examples of generated notifications:
/// <list>
/// <item><c>NotifyChanged("Property")</c></item>
/// <item><c>NotifyChanged(() =&gt; Property)</c></item>
/// <item><c>NotifyChanged((VM x) =&gt; x.Property)</c></item>
/// <item><c>SetProperty(ref myField, value, "Property")</c></item>
/// </list>
/// </example>
[AttributeUsage(AttributeTargets.Method)]
internal sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
{
    public NotifyPropertyChangedInvocatorAttribute() { }
    public NotifyPropertyChangedInvocatorAttribute(string parameterName) => ParameterName = parameterName;

    public string ParameterName { get; }
}

/// <summary>
/// Describes dependency between method input and output
/// </summary>
/// <syntax>
/// <p>Function Definition Table syntax:</p>
/// <list>
/// <item>FDT      ::= FDTRow [;FDTRow]*</item>
/// <item>FDTRow   ::= Input =&gt; Output | Output &lt;= Input</item>
/// <item>Input    ::= ParameterName: Value [, Input]*</item>
/// <item>Output   ::= [ParameterName: Value]* {halt|stop|void|nothing|Value}</item>
/// <item>Value    ::= true | false | null | notnull | canbenull</item>
/// </list>
/// If method has single input parameter, it's name could be omitted.<br/>
/// Using <c>halt</c> (or <c>void</c>/<c>nothing</c>, which is the same)
/// for method output means that the methos doesn't return normally.<br/>
/// <c>canbenull</c> annotation is only applicable for output parameters.<br/>
/// You can use multiple <c>[ContractAnnotation]</c> for each FDT row,
/// or use single attribute with rows separated by semicolon.<br/>
/// </syntax>
/// <examples><list>
/// <item><code>
/// [ContractAnnotation("=> halt")]
/// public void TerminationMethod()
/// </code></item>
/// <item><code>
/// [ContractAnnotation("halt &lt;= condition: false")]
/// public void Assert(bool condition, string text) // regular assertion method
/// </code></item>
/// <item><code>
/// [ContractAnnotation("s:null => true")]
/// public bool IsNullOrEmpty(string s) // string.IsNullOrEmpty()
/// </code></item>
/// <item><code>
/// // A method that returns null if the parameter is null, and not null if the parameter is not null
/// [ContractAnnotation("null => null; notnull => notnull")]
/// public object Transform(object data) 
/// </code></item>
/// <item><code>
/// [ContractAnnotation("s:null=>false; =>true,result:notnull; =>false, result:null")]
/// public bool TryParse(string s, out Person result)
/// </code></item>
/// </list></examples>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class ContractAnnotationAttribute(string contract, bool forceFullStates) : Attribute
{
    public ContractAnnotationAttribute(string contract) : this(contract, false) { }

    public string Contract { get; } = contract;
    public bool ForceFullStates { get; } = forceFullStates;
}

/// <summary>
/// Indicates that marked element should be localized or not
/// </summary>
/// <example><code>
/// [LocalizationRequiredAttribute(true)]
/// public class Foo {
///   private string str = "my string"; // Warning: Localizable string
/// }
/// </code></example>
[AttributeUsage(AttributeTargets.All)]
internal sealed class LocalizationRequiredAttribute(bool required) : Attribute
{
    public LocalizationRequiredAttribute() : this(true) { }

    public bool Required { get; } = required;
}

/// <summary>
/// Indicates that the value of the marked type (or its derivatives)
/// cannot be compared using '==' or '!=' operators and <c>Equals()</c>
/// should be used instead. However, using '==' or '!=' for comparison
/// with <c>null</c> is always permitted.
/// </summary>
/// <example><code>
/// [CannotApplyEqualityOperator]
/// class NoEquality { }
/// class UsesNoEquality {
///   public void Test() {
///     var ca1 = new NoEquality();
///     var ca2 = new NoEquality();
///     if (ca1 != null) { // OK
///       bool condition = ca1 == ca2; // Warning
///     }
///   }
/// }
/// </code></example>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
internal sealed class CannotApplyEqualityOperatorAttribute : Attribute { }

/// <summary>
/// When applied to a target attribute, specifies a requirement for any type marked
/// with the target attribute to implement or inherit specific type or types.
/// </summary>
/// <example><code>
/// [BaseTypeRequired(typeof(IComponent)] // Specify requirement
/// public class ComponentAttribute : Attribute { }
/// [Component] // ComponentAttribute requires implementing IComponent interface
/// public class MyComponent : IComponent { }
/// </code></example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[BaseTypeRequired(typeof(Attribute))]
internal sealed class BaseTypeRequiredAttribute(Type baseType) : Attribute
{
    public Type BaseType { get; } = baseType;
}

/// <summary>
/// Indicates that the marked symbol is used implicitly
/// (e.g. via reflection, in external library), so this symbol
/// will not be marked as unused (as well as by other usage inspections)
/// </summary>
[AttributeUsage(AttributeTargets.All)]
internal sealed class UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
    : Attribute
{
    public UsedImplicitlyAttribute()
        : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

    public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
        : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

    public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
        : this(ImplicitUseKindFlags.Default, targetFlags) { }

    public ImplicitUseKindFlags UseKindFlags { get; } = useKindFlags;
    public ImplicitUseTargetFlags TargetFlags { get; } = targetFlags;
}

/// <summary>
/// Should be used on attributes and causes ReSharper
/// to not mark symbols marked with such attributes as unused
/// (as well as by other usage inspections)
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal sealed class MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
    : Attribute
{
    public MeansImplicitUseAttribute()
        : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

    public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
        : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

    public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
        : this(ImplicitUseKindFlags.Default, targetFlags) { }

    [UsedImplicitly] public ImplicitUseKindFlags UseKindFlags { get; } = useKindFlags;
    [UsedImplicitly] public ImplicitUseTargetFlags TargetFlags { get; } = targetFlags;
}

[Flags]
public enum ImplicitUseKindFlags
{
    Default = Access | Assign | InstantiatedWithFixedConstructorSignature,
    /// <summary>Only entity marked with attribute considered used</summary>
    Access = 1,
    /// <summary>Indicates implicit assignment to a member</summary>
    Assign = 2,
    /// <summary>
    /// Indicates implicit instantiation of a type with fixed constructor signature.
    /// That means any unused constructor parameters won't be reported as such.
    /// </summary>
    InstantiatedWithFixedConstructorSignature = 4,
    /// <summary>Indicates implicit instantiation of a type</summary>
    InstantiatedNoFixedConstructorSignature = 8,
}

/// <summary>
/// Specify what is considered used implicitly
/// when marked with <see cref="MeansImplicitUseAttribute"/>
/// or <see cref="UsedImplicitlyAttribute"/>
/// </summary>
[Flags]
public enum ImplicitUseTargetFlags
{
    Default = Itself,
    Itself = 1,
    /// <summary>Members of entity marked with attribute are considered used</summary>
    Members = 2,
    /// <summary>Entity marked with attribute and all its members considered used</summary>
    WithMembers = Itself | Members
}

/// <summary>
/// This attribute is intended to mark publicly available API
/// which should not be removed and so is treated as used
/// </summary>
[MeansImplicitUse]
internal sealed class PublicAPIAttribute : Attribute
{
    public PublicAPIAttribute() { }
    public PublicAPIAttribute(string comment) => Comment = comment;

    public string Comment { get; }
}

/// <summary>
/// Tells code analysis engine if the parameter is completely handled
/// when the invoked method is on stack. If the parameter is a delegate,
/// indicates that delegate is executed while the method is executed.
/// If the parameter is an enumerable, indicates that it is enumerated
/// while the method is executed
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class InstantHandleAttribute : Attribute { }

/// <summary>
/// Indicates that a parameter is a path to a file or a folder
/// within a web project. Path can be relative or absolute,
/// starting from web root (~)
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class PathReferenceAttribute : Attribute
{
    public PathReferenceAttribute() { }
    public PathReferenceAttribute([PathReference] string basePath) => BasePath = basePath;

    public string BasePath { get; }
}

/// <summary>Является регулярным выражением</summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class RegexPatternAttribute : Attribute { }