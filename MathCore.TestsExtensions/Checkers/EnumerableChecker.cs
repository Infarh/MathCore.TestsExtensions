using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Checkers
{
    public sealed class EnumerableChecker<T> : ValueChecker<IEnumerable<T>>
    {
        internal EnumerableChecker(IEnumerable<T> ActualValue) : base(ActualValue) { }
    }
}
