using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    [TestClass]
    public class ValueCheckerDoubleExtensionsTests : AssertTests
    {
        [TestMethod]
        public void GreaterThan_WithAccuracy_Success()
        {
            const double expected = 1;
            const double accuracy = 0.1;

            Assert.That.Value(0.900001).GreaterThan(expected, accuracy);
            Assert.That.Value(1.0).GreaterThan(expected, accuracy);
            Assert.That.Value(1.1).GreaterThan(expected, accuracy);
        }

        [TestMethod]
        public void GreaterThan_WithAccuracy_Failure()
        {
            const double expected = 1;
            const double accuracy = 0.1;

            IsAssertFail(() => Assert.That.Value(0.899999).GreaterThan(expected, accuracy));
            IsAssertFail(() => Assert.That.Value(0.9).GreaterThan(expected, accuracy));
        }

        [TestMethod]
        public void Greater_WithAccuracy_Success()
        {
            const double expected = 1;
            const double accuracy = 0.1;

            Assert.That.Value(0.9).Greater(expected).WithAccuracy(accuracy);
        }

        [TestMethod]
        public void LessThan_WithAccuracy_Success()
        {
            const double expected = 1;
            const double accuracy = 0.1;

            Assert.That.Value(1.09).LessThan(expected, accuracy);
            Assert.That.Value(1.00).LessThan(expected, accuracy);
            Assert.That.Value(0.99).LessThan(expected, accuracy);
        }

        [TestMethod]
        public void LessThan_WithAccuracy_Failure()
        {
            const double expected = 1;
            const double accuracy = 0.1;

            IsAssertFail(() => Assert.That.Value(1.1).LessThan(expected, accuracy));
            IsAssertFail(() => Assert.That.Value(2.0).LessThan(expected, accuracy));
        }

        [TestMethod]
        public void Less_WithAccuracy_Success()
        {
            const double expected = 1;
            const double accuracy = 0.1;

            Assert.That.Value(0.9).Less(expected).WithAccuracy(accuracy);
        }
    }
}
