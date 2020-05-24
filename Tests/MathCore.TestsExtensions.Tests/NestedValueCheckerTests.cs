using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    [TestClass]
    public class NestedValueCheckerTests
    {
        [TestMethod]
        public void CheckEquals_Test()
        {
            (double x, string msg) test_value = (3.14, "Hello World!");

            Assert.That.Value(test_value)
               .Where(v => v.x).CheckEquals(3.14)
               .Where(v => v.msg).CheckEquals("Hello World!");
        }
    }
}
