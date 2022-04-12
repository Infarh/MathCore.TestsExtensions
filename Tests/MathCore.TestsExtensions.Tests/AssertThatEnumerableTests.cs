using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    [TestClass]
    public class AssertThatEnumerableTests : AssertTests
    {
        [TestMethod]
        public void IsEqualTo_Success()
        {
            IEnumerable<string> actual = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };
            IEnumerable<string> expected = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };

            Assert.That.Enumerable(actual).IsEqualTo(expected);
        }

        [TestMethod]
        public void IsEqualTo_Fail_DifferentValues()
        {
            IEnumerable<string> actual = new[] { "file3.txt", "file4.txt", "-------", "file6.txt" };
            IEnumerable<string> expected = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };

            try
            {
                Assert.That.Enumerable(actual).IsEqualTo(expected);
            }
            catch (AssertFailedException e) when (e.Message.Contains("-------") && e.Message.Contains("file5.txt"))
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void IsEqualTo_Fail_DifferentCount()
        {
            IEnumerable<string> actual = new[] { "file3.txt", "file4.txt", "file5.txt" };
            IEnumerable<string> expected = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };

            try
            {
                Assert.That.Enumerable(actual).IsEqualTo(expected);
            }
            catch (AssertFailedException)
            {
                return;
            }
            Assert.Fail();
        }
    }
}
