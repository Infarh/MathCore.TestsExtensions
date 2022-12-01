// ReSharper disable PossibleMultipleEnumeration

namespace MathCore.TestsExtensions.Tests;

[TestClass]
public class AssertThatCollectionTests : AssertTests
{
    [TestMethod]
    public void IsItemsCount_Success()
    {
        const int        expected_count    = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, expected_count).ToList();

        Assert.That.Collection(actual_collection).IsItemsCount(expected_count);
    }

    [TestMethod]
    public void IsItemsCount_Fail()
    {
        const int        expected_count    = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, expected_count + 20).ToList();

        IsAssertFail(() => Assert.That.Collection(actual_collection).IsItemsCount(expected_count));
    }

    [TestMethod]
    public void IsEqualTo_Success()
    {
        const int        count               = 10;
        ICollection<int> actual_collection   = Enumerable.Range(0, count).ToList();
        ICollection<int> expected_collection = Enumerable.Range(0, count).ToList();

        Assert.That.Collection(actual_collection).IsEqualTo(expected_collection);
    }

    [TestMethod]
    public void IsEqualTo_Success2()
    {
        var actual = new[] { "file1.bin", "file2.bin" };

        Assert.That.Collection(actual).IsEqualTo("Error message", "file1.bin", "file2.bin");
    }

    [TestMethod]
    public void IsEqualTo_Fail_WithDifferentCount()
    {
        const int        count               = 10;
        ICollection<int> actual_collection   = Enumerable.Range(0, count).ToList();
        ICollection<int> expected_collection = Enumerable.Range(0, count + 10).ToList();

        IsAssertFail(() => Assert.That.Collection(actual_collection).IsEqualTo(expected_collection));
    }

    [TestMethod]
    public void IsEmpty_Success()
    {
        ICollection<int> actual_collection = new List<int>();

        Assert.That.Collection(actual_collection).IsEmpty();
    }

    [TestMethod]
    public void IsEmpty_Fail()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

        IsAssertFail(() => Assert.That.Collection(actual_collection).IsEmpty());
    }

    [TestMethod]
    public void IsNotEmpty_Success()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

        Assert.That.Collection(actual_collection).IsNotEmpty();
    }

    [TestMethod]
    public void IsNotEmpty_Fail()
    {
        ICollection<int> actual_collection = new List<int>();

        IsAssertFail(() => Assert.That.Collection(actual_collection).IsNotEmpty());
    }

    [TestMethod]
    public void IsSingleItem_Success()
    {
        const int        count             = 1;
        ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

        Assert.That.Collection(actual_collection).IsSingleItem();
    }

    [TestMethod]
    public void IsSingleItem_Fail()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

        IsAssertFail(() => Assert.That.Collection(actual_collection).IsSingleItem());
    }

    [TestMethod]
    public void ItemsCount_Success()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

        Assert.That.Collection(actual_collection).ItemsCount.IsEqual(count);
    }

    [TestMethod]
    public void ItemsCount_Fail()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count + 10).ToList();

        IsAssertFail(() => Assert.That.Collection(actual_collection).ItemsCount.IsEqual(count));
    }

    [TestMethod]
    public void AllItems_Success()
    {
        const int        count             = 10;
        const int        expected_value    = 100;
        ICollection<int> actual_collection = Enumerable.Repeat(expected_value, count).ToArray();

        Assert.That.Collection(actual_collection).AllItems(v => v.IsEqual(expected_value));
    }

    [TestMethod]
    public void AllItems_FailWithDifferentValue()
    {
        const int        count             = 10;
        const int        expected_value    = 100;
        ICollection<int> actual_collection = Enumerable.Repeat(expected_value + 5, count).ToArray();

        IsAssertFail(() => Assert.That.Collection(actual_collection).AllItems(v => v.IsEqual(expected_value)));
    }

    [TestMethod]
    public void AllItems_WithIndex_Success()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

        Assert.That.Collection(actual_collection).AllItems((v, i) => v.IsEqual(i));
    }

    [TestMethod]
    public void AllItems_WithIndex_Fail()
    {
        const int        count             = 10;
        ICollection<int> actual_collection = Enumerable.Range(0, count).Select(i => i + 100).ToList();

        IsAssertFail(() => Assert.That.Collection(actual_collection).AllItems((v, i) => v.IsEqual(i)));
    }

    [TestMethod]
    public void IsEquals_ToParamsValues_Success()
    {
        int[] test_collection = { 1, 3, 5, 7 };

        Assert.That.Collection(test_collection).IsEqualTo(1, 3, 5, 7);
    }

    [TestMethod]
    public void IsEqualTo_DoubleDeclensionsArray_Success()
    {
        double[,] actual_x =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 },
        };

        double[,] expected_x =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 },
        };

        Assert.That.Collection(actual_x).IsEqualTo(expected_x);
    }

    [TestMethod]
    public void FluentLambdaCollectionContains_Success()
    {
        int[] items = { 1, 3, 5, 7 };
        Assert.That.Collection(items).Contains(item => item == 5);
    }

    [TestMethod]
    public void IsEqualToEnumerable()
    {
        var enumerable = Enumerable.Range(1, 10);

        var collection = enumerable.ToArray();

        Assert.That.Collection(collection)
           .IsEqualTo(enumerable);
    }

    [TestMethod]
    public void Collection_IsEqualsTo_WithAccuracy_Success()
    {
        double[] actual = {
            +1,
            -4.572417374538311,
            +9.501375642075562,
            -13.058914538431761,
            +14.174606787779414,
            -12.080552289476486,
            +7.886943621934985,
            -4.575356118364454,
            +2.5486464717152706,
            -1.2368047918884044,
            +0.8178436415754909,
            -0.5751215311689866,
            +0.17026409592167924
        };

        double[] expected = {
            +1,
            -4.572417374538311,
            +9.501375642075566,
            -13.058914538431761,
            +14.174606787779418,
            -12.08055228947649,
            +7.886943621934992,
            -4.57535611836446,
            +2.5486464717152755,
            -1.2368047918884075,
            +0.8178436415754919,
            -0.5751215311689868,
            +0.17026409592167924
        };

        var checker = Assert.That.Collection(actual);
        checker.IsEqualTo(expected, 7.2e-15);
    }

    [TestMethod]
    public void Collection_IsEqualsTo_WithAccuracy_Fail()
    {
        double[] A = {
            +1,
            -4.572417374538311,
            +9.501375642075566,
            -13.058914538431761,
            +14.174606787779418,
            -12.08055228947649,
            +7.886943621934992,
            -4.57535611836446,
            +2.5486464717152755,
            -1.2368047918884075,
            +0.8178436415754919,
            -0.5751215311689868,
            +0.17026409592167924
        };

        double[] B = {
            +0.0342439351432325,
            -0.3108325662349939,
            +1.3810596668728097,
            -3.9254734972857412,
            +7.9065947919517265,
            -11.850402005076203,
            +13.530077111010863,
            -11.850402005076198,
            +7.906594791951721,
            -3.9254734972857364,
            +1.3810596668728072,
            -0.31083256623499333,
            +0.034243935143232415
        };

        try
        {
            Assert.That.Collection(A).IsEqualTo(B, 1e-5);
            Assert.Fail("Тест был пройден на неверном наборе данных");
        }
        catch (AssertFailedException)
        {
            // штатно тест должен быть провален на неверном наборе данных
        }
    }


}