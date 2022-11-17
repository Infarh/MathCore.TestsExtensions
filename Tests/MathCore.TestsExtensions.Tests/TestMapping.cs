namespace MathCore.TestsExtensions.Tests;

internal static class TestMapping
{
    public static TResult MapTo<T, TItem, TResult>(this T item, Func<T, TResult> Mapper)
        where T : ICollection<TItem> =>
        Mapper(item);
}