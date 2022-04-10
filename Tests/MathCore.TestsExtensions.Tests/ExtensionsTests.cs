using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    [TestClass]
    public class ExtensionsTests : AssertTests
    {
        private const double __Eps = 1e-14;

        [TestMethod]
        public void SuccessesTest()
        {
            var list = new List<int>();
            Assert.That.Value(list)
               .AssertThat(l => l.IsNotNull())
               .Where(l => l.Count).Check(Count => Count.IsEqual(0))
               .Where(l => l.Capacity).Check(Capacity => Capacity.IsEqual(0));
        }

        [TestMethod]
        public void LessOrEqualsThan_WithAccuracy_ValueChecker_Success()
        {
            const double expected_value = 1;
            const double actual_value = 1.1;
            const double eps = 0.1;

            Assert.That.Value(actual_value).LessOrEqualsThan(expected_value, eps);
        }

        [TestMethod]
        public void LessOrEqualsThan_WithAccuracy_ValueChecker_Fail()
        {
            const double expected_value = 1;
            const double actual_value = 1.2;
            const double eps = 0.1;

            IsAssertFail(() => Assert.That.Value(actual_value).LessOrEqualsThan(expected_value, eps));
        }

        [TestMethod]
        public void LessOrEqualsThan_ValueChecker_Success()
        {
            const double expected_value = 1;
            const double actual_value = 1;

            Assert.That.Value(actual_value).LessOrEqualsThan(expected_value);
            Assert.That.Value(actual_value - __Eps).LessOrEqualsThan(expected_value);
        }

        [TestMethod]
        public void LessOrEqualsThan_ValueChecker_Fail()
        {
            const double expected_value = 1;
            const double actual_value = 1 + __Eps;

            IsAssertFail(() => Assert.That.Value(actual_value).LessOrEqualsThan(expected_value));
        }

        [TestMethod]
        public void LessThan_ValueChecker_Success()
        {
            const double expected_value = 1 + __Eps;
            const double actual_value = 1;

            Assert.That.Value(actual_value).LessThan(expected_value);
        }

        [TestMethod]
        public void LessThan_ValueChecker_Fail()
        {
            const double expected_value = 1 - __Eps;
            const double actual_value = 1;

            IsAssertFail(() => Assert.That.Value(actual_value).LessThan(expected_value));
        }

        [TestMethod]
        public void GreaterOrEqualsThan_WithAccuracy_ValueChecker_Success1()
        {
            const double expected_value = 1;
            const double actual_value = 0.9;
            const double eps = 0.1;

            Assert.That.Value(actual_value).GreaterOrEqualsThan(expected_value, eps);
        }

        [TestMethod]
        public void GreaterOrEqualsThan_WithAccuracy_ValueChecker_Fail()
        {
            const double expected_value = 1;
            const double actual_value = 0.89;
            const double eps = 0.1;

            try
            {
                Assert.That.Value(actual_value).GreaterOrEqualsThan(expected_value, eps);
            }
            catch (AssertFailedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void GreaterThan_ValueChecker_Success()
        {
            const double expected_value = 1 - __Eps;
            const double actual_value = 1;

            Assert.That.Value(actual_value).GreaterThan(expected_value);
            Assert.That.Value(actual_value + __Eps).GreaterThan(expected_value);
        }
    }
}
