using Microsoft.VisualStudio.TestTools.UnitTesting.Attributes;
using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Infrastructure;

internal static class ObjectEx
{
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    [return: NotNullIfNotNull("obj")]
    public static T NotNull<T>(this T? obj, string? Message = null, [CallerArgumentExpression("obj")] string? ParameterName = null) where T : class
    {
        if (obj is not null)
            return obj;

        if (ParameterName is null)
            throw new InvalidOperationException(Message ?? "Пустая ссылка на объект");

        throw new ArgumentNullException(ParameterName, Message ?? "Пустая ссылка на в значении параметра");
    }
}
