using System.Numerics;

namespace MathCore.TestsExtensions.Tests;

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
        const double actual_value   = 1.1;
        const double eps            = 0.1;

        Assert.That.Value(actual_value).LessOrEqualsThan(expected_value, eps);
    }

    [TestMethod]
    public void LessOrEqualsThan_WithAccuracy_ValueChecker_Fail()
    {
        const double expected_value = 1;
        const double actual_value   = 1.2;
        const double eps            = 0.1;

        IsAssertFail(() => Assert.That.Value(actual_value).LessOrEqualsThan(expected_value, eps));
    }

    [TestMethod]
    public void LessOrEqualsThan_ValueChecker_Success()
    {
        const double expected_value = 1;
        const double actual_value   = 1;

        Assert.That.Value(actual_value).LessOrEqualsThan(expected_value);
        Assert.That.Value(actual_value - __Eps).LessOrEqualsThan(expected_value);
    }

    [TestMethod]
    public void LessOrEqualsThan_ValueChecker_Fail()
    {
        const double expected_value = 1;
        const double actual_value   = 1 + __Eps;

        IsAssertFail(() => Assert.That.Value(actual_value).LessOrEqualsThan(expected_value));
    }

    [TestMethod]
    public void LessThan_ValueChecker_Success()
    {
        const double expected_value = 1 + __Eps;
        const double actual_value   = 1;

        Assert.That.Value(actual_value).LessThan(expected_value);
    }

    [TestMethod]
    public void LessThan_ValueChecker_Fail()
    {
        const double expected_value = 1 - __Eps;
        const double actual_value   = 1;

        IsAssertFail(() => Assert.That.Value(actual_value).LessThan(expected_value));
    }

    [TestMethod]
    public void GreaterOrEqualsThan_WithAccuracy_ValueChecker_Success1()
    {
        const double expected_value = 1;
        const double actual_value   = 0.9;
        const double eps            = 0.1;

        Assert.That.Value(actual_value).GreaterOrEqualsThan(expected_value, eps);
    }

    [TestMethod]
    public void GreaterOrEqualsThan_WithAccuracy_ValueChecker_Fail()
    {
        const double expected_value = 1;
        const double actual_value   = 0.89;
        const double eps            = 0.1;

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
        const double actual_value   = 1;

        Assert.That.Value(actual_value).GreaterThan(expected_value);
        Assert.That.Value(actual_value + __Eps).GreaterThan(expected_value);
    }

    public readonly struct TupleAccuracyComparer : IEqualityComparer<(double a, double b)>
    {
        private double Eps { get; init; }

        public TupleAccuracyComparer(double Eps)
        {
            if (Eps < 0.0)
                throw new ArgumentOutOfRangeException(nameof(Eps), (object)Eps, "Значение точности не должно быть меньше нуля");
            this.Eps = !double.IsNaN(Eps) ? Eps : throw new ArgumentException("Значение точности не должно быть NaN", nameof(Eps));
        }

        public bool Equals((double a, double b) x, (double a, double b) y)
        {
            var (xa, xb) = x;
            var (ya, yb) = y;

            var eps = Eps;
            var (delta_re, delta_im) = (xa - ya, xb - yb);
            return Math.Abs(delta_re) <= eps
                && Math.Abs(delta_im) <= eps;
        }

        public int GetHashCode((double a, double b) z)
        {
            var (a, b) = z;

            if (double.IsNaN(a) || double.IsNaN(b)) return z.GetHashCode();
            var eps = Eps;
            var value = new Complex(
                Math.Round(a * eps) / eps,
                Math.Round(b * eps) / eps
            );
            return value.GetHashCode();
        }
    }

    [TestMethod]
    public void AssertEquals_Collection_with_Accuracy_Success()
    {
        IEnumerable<(double, double)> values = new[]
        {
            (-0.425984051389412477, 2.141566444565760730),
            (-0.425984051389412477, -2.141566444565760730),
            (-1.213099943899241140, 1.815532366728786817),
            (-1.213099943899241140, -1.815532366728786817),
            (-1.815532366728786817, 1.213099943899241584),
            (-1.815532366728786817, -1.213099943899241584),
            (-2.141566444565760730, 0.425984051389413365), 
            (-2.141566444565760730, -0.425984051389413365)
        };

        var equality_comparer = new TupleAccuracyComparer(1e-14);
        values.AssertEquals(equality_comparer,
            /*[ 0]*/ (-0.425984051389412477, 2.141566444565760730),
            /*[ 1]*/ (-0.425984051389412477, -2.141566444565760730),
            /*[ 2]*/ (-1.213099943899241140, 1.815532366728786817),
            /*[ 3]*/ (-1.213099943899241140, -1.815532366728786817),
            /*[ 4]*/ (-1.815532366728786817, 1.213099943899241584),
            /*[ 5]*/ (-1.815532366728786817, -1.213099943899241584),
            /*[ 6]*/ (-2.141566444565760730, 0.425984051389413365),
            /*[ 7]*/ (-2.141566444565760730, -0.425984051389413365)
        );
    }

    [TestMethod]
    public void AssertEquals_Collection_with_Accuracy_Fail()
    {
        IEnumerable<(double, double)> values = new[]
        {
            (-0.425984051389412477, 2.141566444565760730),
            (-0.425984051389412477, -2.141566444565760730),
            (-1.213099943899241140, 1.815532366728786817),
            (-1.213099943899241140, -1.815532366728786817),
            (-1.815532366728786817, 1.213099943899241584),
            (-1.815532366728786817, -1.213099943899241584),
            (-2.141566444565760730, 0.425984051389413365), 
            (-2.141566444565760730, -0.425984051389413365)
        };

        var equality_comparer = new TupleAccuracyComparer(1e-14);
        try
        {
            values.AssertEquals(equality_comparer,
                /*[ 0]*/ (-0.425984051389412477, 2.141566444565760730),
                /*[ 1]*/ (-0.425984051389412477, -2.141566444565760730),
                /*[ 2]*/ (-1.213099943899241140, 1.815532366728786817),
                /*[ 3]*/ (-1.213099943899241140, -1.815532366728786817),
                /*[ 4]*/ (-1.815532366728786817, 1.213099943899241584),
                /*[ 5]*/ (-1.815532366728786817, -1.213099943899241584),
                /*[ 6]*/ (-2.141566444565760730, 0.425984051389413365)
            );

            Assert.Fail("Проверка была выполнена и не нашла проблемы");
        }
        catch (AssertFailedException)
        {
                
        }
    }
}