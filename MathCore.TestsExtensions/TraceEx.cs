#nullable enable
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Расширения трассировки для удобного вывода значений и перечислений в <see cref="Trace"/> (категория "Tests")</summary>
public static class TraceEx
{
    /// <summary>Выводит значение в трассировку (<see cref="Trace"/>) с указанием имени аргумента и возвращает его обратно</summary>
    /// <typeparam name="T">Тип значения</typeparam>
    /// <param name="value">Значение для вывода</param>
    /// <param name="Prefix">Имя выражения, переданное компилятором (если доступно)</param>
    /// <returns>Исходное значение <paramref name="value"/> без изменений</returns>
    /// <remarks>
    /// Используется <see cref="CallerArgumentExpressionAttribute"/> чтобы автоматически получить текст выражения аргумента.
    /// Формат вывода: "Имя = Значение" если имя доступно, иначе просто "Значение".
    /// </remarks>
    public static T ToTrace<T>(this T value, [CallerArgumentExpression(nameof(value))] string? Prefix = null)
    {
        Trace.WriteLine(
            Prefix is { Length: > 0 }
                ? FormattableString.Invariant($"{Prefix} = {value}")
                : FormattableString.Invariant($"{value}"), 
            "Tests");
        return value;
    }

    /// <summary>Перечисляет элементы последовательности и выводит их значения в трассировку (<see cref="Trace"/>) c индексами</summary>
    /// <typeparam name="T">Тип элементов последовательности</typeparam>
    /// <param name="items">Последовательность элементов для вывода</param>
    /// <param name="Name">Имя выражения, переданное компилятором (если доступно)</param>
    /// <remarks>
    /// Если имя передано, выводит заголовок и заключает элементы в блок в виде массива.
    /// Каждый элемент сопровождается комментарием с индексом: /*[индекс]*/ value.
    /// Ширина поля индекса определяется количеством элементов (если доступно через <see cref="ICollection"/>).
    /// </remarks>
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

    /// <summary>Вычисляет целую часть десятичного логарифма положительного числа</summary>
    /// <param name="x">Положительное целое число</param>
    /// <returns>Целая часть log10(x)</returns>
    private static int Log10Int(int x)=> (int)Math.Log10(x);
}
