using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Extensions
{
    public static class StringAssertExtensions
    {
        public static ValueChecker<string> Value(StringAssert that, string ActualValue) => new ValueChecker<string>(ActualValue);
    }
}
