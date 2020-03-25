﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathCore.TestsExtensions.Tests
{
    [TestClass]
    public class AssertThatCollectionTests
    {
        private static AssertFailedException IsAssertFail(Action AssertAction) => ExpectedException<AssertFailedException>(AssertAction);

        private static TException ExpectedException<TException>(Action AssertAction) where TException : Exception
        {
            TException expected_exception = null;
            try
            {
                AssertAction();
            }
            catch (TException exception)
            {
                expected_exception = exception;
            }
            if (expected_exception is null)
                throw new AssertFailedException($"Требуемое исключение типа {typeof(TException).Name} выброшено не было");

            return expected_exception;
        }

        [TestMethod]
        public void IsItemsCount_Success()
        {
            const int expected_count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, expected_count).ToList();

            Assert.That.Collection(actual_collection).IsItemsCount(expected_count);
        }

        [TestMethod]
        public void IsItemsCount_Fail()
        {
            const int expected_count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, expected_count + 20).ToList();

            IsAssertFail(() => Assert.That.Collection(actual_collection).IsItemsCount(expected_count));
        }

        [TestMethod]
        public void IsEqualTo_Success()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();
            ICollection<int> expected_collection = Enumerable.Range(0, count).ToList();

            Assert.That.Collection(actual_collection).IsEqualTo(expected_collection);
        }

        [TestMethod]
        public void IsEqualTo_Fail_WithDifferentCount()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();
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
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

            IsAssertFail(() => Assert.That.Collection(actual_collection).IsEmpty());
        }

        [TestMethod]
        public void IsNotEmpty_Success()
        {
            const int count = 10;
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
            const int count = 1;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

            Assert.That.Collection(actual_collection).IsSingleItem();
        }

        [TestMethod]
        public void IsSingleItem_Fail()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

            IsAssertFail(() => Assert.That.Collection(actual_collection).IsSingleItem());
        }

        [TestMethod]
        public void ItemsCount_Success()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

            Assert.That.Collection(actual_collection).ItemsCount.IsEqual(count);
        }

        [TestMethod]
        public void ItemsCount_Fail()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count + 10).ToList();

            IsAssertFail(() => Assert.That.Collection(actual_collection).ItemsCount.IsEqual(count));
        }

        [TestMethod]
        public void AllItems_Success()
        {
            const int count = 10;
            const int expected_value = 100;
            ICollection<int> actual_collection = Enumerable.Repeat(expected_value, count).ToArray();

            Assert.That.Collection(actual_collection).AllItems(v => v.IsEqual(expected_value));
        }

        [TestMethod]
        public void AllItems_FailWithDifferentValue()
        {
            const int count = 10;
            const int expected_value = 100;
            ICollection<int> actual_collection = Enumerable.Repeat(expected_value + 5, count).ToArray();

            IsAssertFail(() => Assert.That.Collection(actual_collection).AllItems(v => v.IsEqual(expected_value)));
        }

        [TestMethod]
        public void AllItems_WithIndex_Success()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).ToList();

            Assert.That.Collection(actual_collection).AllItems((v, i) => v.IsEqual(i));
        }

        [TestMethod]
        public void AllItems_WithIndex_Fail()
        {
            const int count = 10;
            ICollection<int> actual_collection = Enumerable.Range(0, count).Select(i => i + 100).ToList();

            IsAssertFail(() => Assert.That.Collection(actual_collection).AllItems((v, i) => v.IsEqual(i)));
        }

        public void IsEquals_ToParamsValues_Success()
        {
            int[] test_collection = { 1, 3, 5, 7 };

            Assert.That.Collection(test_collection).IsEqualTo(1, 3, 5, 7);
        }
    }
}