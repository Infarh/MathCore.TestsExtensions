using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void SuccessesTest()
        {
            var list = new List<int>();
            Assert.That.Value(list)
               .AssertThat(l => l.IsNotNull())
               .Where(l => l.Count).Check(Count => Count.IsEqual(0))
               .Where(l => l.Capacity).Check(Capacity => Capacity.IsEqual(2));
        }
    }
}
