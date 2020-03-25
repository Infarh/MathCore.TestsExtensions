using System;
using System.Collections.Generic;
using System.Text;

namespace MathCore.TestsExtensions.Tests
{
    internal static class TestMapping
    {
        public static TResult MapTo<T, TItem, TResult>(this T item, Func<T, TResult> Mapper)
            where T : ICollection<TItem>
        {
            return Mapper(item);
        }
    }
}
