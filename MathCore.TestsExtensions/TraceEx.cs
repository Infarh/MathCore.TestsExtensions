#nullable enable
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

public static class TraceEx
{
    public static T ToTrace<T>(this T value, [CallerArgumentExpression(nameof(value))] string? Prefix = null)
    {
        Trace.WriteLine(
            Prefix is { Length: > 0 }
                ? FormattableString.Invariant($"{Prefix} = {value}")
                : FormattableString.Invariant($"{value}"), 
            "Tests");
        return value;
    }

    public static void ToTraceEnum<T>(this IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? Name = null)
    {
        string? pad_str = null;
        if (Name is { Length: > 0 })
        {
            Trace.WriteLine($"{typeof(T).Name}[] {Name} =", "Tests");
            Trace.WriteLine("[", "Tests");
            pad_str = "    ";
        }
        var i = 0;
        var m = items is ICollection { Count: var items_count }
            ? Log10Int(items_count) + 1
            : 2;
        foreach (var item in items)
        {
            if (i > 0)
                Debug.WriteLine(",");

            Trace.WriteLine(FormattableString.Invariant($"{pad_str}/*[{i.ToString().PadLeft(m)}]*/ {item}"), "Tests");
            i++;
        }

        if (pad_str is not null)
            Trace.WriteLine("]", "Tests");
    }

    private static int Log10Int(int x)=> (int)Math.Log10(x);
}
