using System.Collections;
using System.Text;

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Объект проверки коллекций вещественных чисел</summary>
public class DoubleCollectionChecker : ICollection<double>
{
    /// <summary>Объект проверки равенства элементов вещественной коллекции чисел с заданной точностью</summary>
    public sealed class EqualityCheckerWithAccuracy : IDisposable
    {
        /// <summary>Проверяемая коллекция значение</summary>
        private readonly ICollection<double> _ActualValues;

        /// <summary>Значения, с которыми требуется провести сравнение</summary>
        private readonly ICollection<double> _ExpectedValues;

        /// <summary>Проверка на неравенство</summary>
        private readonly bool _Not;

        private bool _IsChecked;

        /// <summary>Инициализация нового объекта сравнения чисел с заданной точностью</summary>
        /// <param name="ActualValues">Проверяемая коллекция</param>
        /// <param name="ExpectedValues">Требуемые значения</param>
        /// <param name="Not">Проверять на неравенство</param>
        internal EqualityCheckerWithAccuracy(ICollection<double> ActualValues, double[] ExpectedValues, bool Not = false)
        {
            _ActualValues = ActualValues;
            _ExpectedValues = ExpectedValues;
            _Not = Not;
        }

        /// <summary>Проверка с задаваемой точностью</summary>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public EqualityCheckerWithAccuracy WithAccuracy(double Accuracy, string? Message = null)
        {
            if (double.IsNaN(Accuracy))
                throw new ArgumentException("Точность не может быть в значении NaN", nameof(Accuracy));

            _IsChecked = true;

            IEnumerator<double>? expected_collection_enumerator = null;
            IEnumerator<double>? actual_collection_enumerator = null;

            try
            {
                expected_collection_enumerator = _ExpectedValues.GetEnumerator();
                actual_collection_enumerator = _ActualValues.GetEnumerator();

                var index = 0;
                var assert_fails = new List<FormattableString>();
                var min_delta = double.PositiveInfinity;
                var max_delta = double.NegativeInfinity;
                var max_error = double.NegativeInfinity;
                if (_Not)
                    while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                    {
                        var expected = expected_collection_enumerator.Current;
                        var actual = actual_collection_enumerator.Current;
                        var delta = expected - actual;
                        var delta_abs = Math.Abs(delta);

                        if (double.IsNaN(actual))
                            throw new AssertFailedException($"{Message}error[{index}]: значение было равно NaN");

                        if (delta_abs < Accuracy)
                        {
                            assert_fails.Add($"[{index,3}]\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n      err:{delta_abs:e3}(rel.err:{delta_abs / expected})\r\n      eps:{Accuracy}");
                            //FormattableString message = $"{Message}error[{index}]\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n    accuracy:{Accuracy}\r\n    err:{delta:e3}(rel.err:{delta / expected})";
                            //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                            min_delta = Math.Min(min_delta, delta);
                            max_delta = Math.Max(max_delta, delta);
                            max_error = Math.Max(max_error, delta_abs);
                        }

                        //Assert.AreNotEqual(
                        //    expected, actual, Accuracy,
                        //    "{0}error[{1}]: ожидалось({2}), получено({3}), accuracy:{4}, err:{5:e3}; rel.err:{6}",
                        //    Message, index, expected, actual, Accuracy, delta, delta / expected);

                        index++;
                    }
                else
                    while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
                    {
                        var expected = expected_collection_enumerator.Current;
                        var actual = actual_collection_enumerator.Current;
                        var delta = expected - actual;
                        var delta_abs = Math.Abs(delta);

                        if (double.IsNaN(actual))
                            throw new AssertFailedException($"{Message}error[{index}]: значение было равно NaN");

                        if (delta_abs > Accuracy)
                        {
                            assert_fails.Add($"[{index,3}]\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n      err:{delta_abs:e3}(rel.err:{delta_abs / expected})\r\n      eps:{Accuracy}");
                            //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n    accuracy:{Accuracy}\r\n    err:{delta:e3}(rel.err:{delta / expected})";
                            //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                            min_delta = Math.Min(min_delta, delta);
                            max_delta = Math.Max(max_delta, delta);
                            max_error = Math.Max(max_error, delta_abs);
                        }

                        //Assert.AreEqual(
                        //    expected, actual, Accuracy,
                        //    "{0}error[{1}]: ожидалось({2}), получено({3}), accuracy:{4}, err:{5:e3}; rel.err:{6}",
                        //    Message, index, expected, actual, Accuracy, delta, delta / expected);

                        index++;
                    }

                if (assert_fails.Count == 0) return this;

                if (assert_fails.Count > 1)
                {
                    assert_fails.Add($"  min delta:{min_delta:e3}");
                    assert_fails.Add($"  max delta:{max_delta:e3}");
                    assert_fails.Add($"  max error:{max_error:e3}");
                }

                var message = assert_fails.Aggregate(
                    new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                    (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                    S => S.ToString());
                throw new AssertFailedException(message)
                   .AddData("Expected", _ExpectedValues)
                   .AddData("Actual", _ActualValues)
                   .AddData("Accuracy", Accuracy);
            }
            finally
            {
                expected_collection_enumerator?.Dispose();
                actual_collection_enumerator?.Dispose();
            }
        }

        void IDisposable.Dispose()
        {
            if (_IsChecked) return;
            Assert.Fail($"Проверка на {(_Not ? "не" : null)}равенство не выполнена");
        }
    }

    /// <summary>Проверяемая коллекция</summary>
    private readonly ICollection<double> _ActualCollection;

    /// <summary>Инициализация нового объекта проверки коллекции вещественных чисел</summary>
    /// <param name="ActualCollection"></param>
    internal DoubleCollectionChecker(ICollection<double> ActualCollection) => _ActualCollection = ActualCollection;

    /// <summary>Проверка значений коллекции на равенство</summary>
    /// <param name="ExpectedValues">Ожидаемые значения</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public EqualityCheckerWithAccuracy ValuesAreEqualTo(params double[] ExpectedValues)
    {
        Assert.That
           .Value(_ActualCollection.Count)
           .IsEqual(ExpectedValues.Length, $"Размер коллекции {_ActualCollection.Count} не совпадает с ожидаемым размером {ExpectedValues.Length}");

        return new(_ActualCollection, ExpectedValues);
    }

    /// <summary>Проверка значений коллекции на равенство</summary>
    /// <param name="ExpectedValues">Ожидаемые значения</param>
    /// <returns>Объект сравнения с задаваемой точностью</returns>
    public EqualityCheckerWithAccuracy ValuesAreNotEqualTo(params double[] ExpectedValues)
    {
        Assert.That
           .Value(_ActualCollection.Count)
           .IsEqual(ExpectedValues.Length, $"Размер коллекции {_ActualCollection.Count} не совпадает с ожидаемым размером {ExpectedValues.Length}");

        return new(_ActualCollection, ExpectedValues, Not: true);
    }

    /// <summary>Проверка на эквивалентность с задаваемым набором значений</summary>
    /// <param name="ExpectedValues">Ожидаемые значения коллекции</param>
    public DoubleCollectionChecker ValuesAreEqual(params double[] ExpectedValues)
    {
        IsEqualTo(ExpectedValues);
        return this;
    }

    /// <summary>Проверка на эквивалентность с задаваемым набором значений</summary>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <param name="ExpectedValues">Ожидаемые значения коллекции</param>
    public DoubleCollectionChecker ValuesAreEqual(string Message, params double[] ExpectedValues)
    {
        IsEqualTo(ExpectedValues, Message);
        return this;
    }

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker IsEqualTo(ICollection<double> ExpectedCollection, string? Message = null)
    {
        Assert.That
           .Value(_ActualCollection.Count)
           .IsEqual(ExpectedCollection.Count, $"Размер коллекции {_ActualCollection.Count} не совпадает с ожидаемым размером {ExpectedCollection.Count}");

        IEnumerator<double>? expected_collection_enumerator = null;
        IEnumerator<double>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            var min_delta = double.PositiveInfinity;
            var max_delta = double.NegativeInfinity;
            var max_error = double.NegativeInfinity;
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                var delta = expected - actual;
                var delta_abs = Math.Abs(delta);

                if (!expected.Equals(actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n      err:{delta_abs:e3}(rel.err:{delta_abs / expected})");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n    err:{delta:e3}(rel.err:{delta / expected})";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));

                    min_delta = Math.Min(min_delta, delta);
                    max_delta = Math.Max(max_delta, delta);
                    max_error = Math.Max(max_error, delta_abs);
                }

                //Assert.AreEqual(
                //    expected, actual,
                //    "{0}error[{1}]: ожидалось({2}), получено({3}), err:{4}(rel:{5})",
                //    Message, index, expected, actual,
                //    delta.ToString("e3", CultureInfo.InvariantCulture),
                //    (delta / expected).ToString(CultureInfo.InvariantCulture));

                index++;
            }

            if (assert_fails.Count == 0) return this;

            if (assert_fails.Count > 1)
            {
                assert_fails.Add($"  min delta:{min_delta:e3}");
                assert_fails.Add($"  max delta:{max_delta:e3}");
                assert_fails.Add($"  max error:{max_error:e3}");
            }

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedCollection)
               .AddData("Actual", _ActualCollection);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
    /// <param name="Accuracy">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker IsEqualTo(ICollection<double> ExpectedCollection, double Accuracy, string? Message = null)
    {
        if (double.IsNaN(Accuracy))
            throw new ArgumentException("Значение точности не может быть равно NaN", nameof(Accuracy));

        Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count, "Размеры коллекций не совпадают");

        IEnumerator<double>? expected_collection_enumerator = null;
        IEnumerator<double>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            var min_delta = double.PositiveInfinity;
            var max_delta = double.NegativeInfinity;
            var max_error = double.NegativeInfinity;
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                var delta = expected - actual;
                var delta_abs = Math.Abs(delta);

                if (double.IsNaN(actual))
                {
                    assert_fails.Add($"[{index,3}]: полученное значение  равно NaN");
                    //throw new AssertFailedException($"{Message}error[{index}]: полученное значение  равно NaN");
                }

                if (double.IsNaN(actual))
                {
                    assert_fails.Add($"[{index,3}]: значение было равно NaN");
                    //throw new AssertFailedException($"{Message}error[{index}]: значение было равно NaN");
                }

                if (delta_abs > Accuracy)
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n      err:{delta_abs:e3}(rel.err:{delta_abs / expected})\r\n      eps:{Accuracy}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n    eps:{Accuracy}\r\n    err:{delta:e3}(rel.err:{delta / expected})";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                    min_delta = Math.Min(min_delta, delta);
                    max_delta = Math.Max(max_delta, delta);
                    max_error = Math.Max(max_error, delta_abs);
                }

                //Assert.AreEqual(
                //    expected, actual, Accuracy,
                //    "{0}error[{1}]: ожидалось({2}), получено({3}), eps:{4}, err:{5}(rel:{6})",
                //    Message, index, expected, actual, Accuracy,
                //    delta.ToString("e3", CultureInfo.InvariantCulture),
                //    (delta / expected).ToString(CultureInfo.InvariantCulture));

                index++;
            }

            if (assert_fails.Count == 0) return this;

            if (assert_fails.Count > 1)
            {
                assert_fails.Add($"  min delta:{min_delta:e3}");
                assert_fails.Add($"  max delta:{max_delta:e3}");
                assert_fails.Add($"  max error:{max_error:e3}");
            }

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)));
            throw new AssertFailedException(message.ToString())
               .AddData("Expected", ExpectedCollection)
               .AddData("Actual", _ActualCollection)
               .AddData("Accuracy", Accuracy);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedCollection">Ожидаемая коллекция значений</param>
    /// <param name="Comparer">Точность сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker IsEqualTo(ICollection<double> ExpectedCollection, IEqualityComparer<double> Comparer, string? Message = null)
    {
        if (Comparer is null) throw new ArgumentNullException(nameof(Comparer));

        Assert.That.Value(_ActualCollection.Count).IsEqual(ExpectedCollection.Count, "Размеры коллекций не совпадают");

        IEnumerator<double>? expected_collection_enumerator = null;
        IEnumerator<double>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedCollection.GetEnumerator();
            actual_collection_enumerator = _ActualCollection.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            var min_delta = double.PositiveInfinity;
            var max_delta = double.NegativeInfinity;
            var max_error = double.NegativeInfinity;
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                var delta = expected - actual;
                var delta_abs = Math.Abs(delta);

                if (double.IsNaN(actual))
                {
                    assert_fails.Add($"[{index,3}]: полученное значение  равно NaN");
                    //throw new AssertFailedException($"{Message}error[{index}]: полученное значение  равно NaN");
                }

                if (double.IsNaN(actual))
                {
                    assert_fails.Add($"[{index,3}]: значение было равно NaN");
                    //throw new AssertFailedException($"{Message}error[{index}]: значение было равно NaN");
                }

                if (!Comparer.Equals(actual, expected))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n      err:{delta_abs:e3}(rel.err:{delta_abs / expected})");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}\r\n    eps:{Accuracy}\r\n    err:{delta:e3}(rel.err:{delta / expected})";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                    min_delta = Math.Min(min_delta, delta);
                    max_delta = Math.Max(max_delta, delta);
                    max_error = Math.Max(max_error, delta_abs);
                }

                //Assert.AreEqual(
                //    expected, actual, Accuracy,
                //    "{0}error[{1}]: ожидалось({2}), получено({3}), eps:{4}, err:{5}(rel:{6})",
                //    Message, index, expected, actual, Accuracy,
                //    delta.ToString("e3", CultureInfo.InvariantCulture),
                //    (delta / expected).ToString(CultureInfo.InvariantCulture));

                index++;
            }

            if (assert_fails.Count == 0) return this;

            if (assert_fails.Count > 1)
            {
                assert_fails.Add($"  min delta:{min_delta:e3}");
                assert_fails.Add($"  max delta:{max_delta:e3}");
                assert_fails.Add($"  max error:{max_error:e3}");
            }

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)));
            throw new AssertFailedException(message.ToString())
               .AddData("Expected", ExpectedCollection)
               .AddData("Actual", _ActualCollection)
               .AddData("Comparer", Comparer);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Все элементы коллекции равны заданному значению</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker ElementsAreEqualTo(double ExpectedValue, string? Message = null)
    {
        var index = 0;
        var assert_fails = new List<FormattableString>();
        var min_delta = double.PositiveInfinity;
        var max_delta = double.NegativeInfinity;
        var max_error = double.NegativeInfinity;
        foreach (var actual_value in _ActualCollection)
        {
            if (!actual_value.Equals(ExpectedValue))
            {
                var delta = ExpectedValue - actual_value;
                var delta_abs = Math.Abs(delta);
                assert_fails.Add($"[{index,3}]:\r\n    {actual_value}\r\n != {ExpectedValue}\r\n      err:{delta_abs:e2}(err.rel:{delta_abs / ExpectedValue:e2})");
                min_delta = Math.Min(min_delta, delta);
                max_delta = Math.Max(max_delta, delta);
                max_error = Math.Max(max_error, delta_abs);
                //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
            }

            //Assert.AreEqual(ExpectedValue, actual_value,
            //    "{0}error[{1}]:{2:e2}", Message.AddSeparator(), index, Math.Abs(ExpectedValue - actual_value));

            index++;
        }

        if (assert_fails.Count == 0) return this;

        if (assert_fails.Count > 1)
        {
            assert_fails.Add($"  min delta:{min_delta:e3}");
            assert_fails.Add($"  max delta:{max_delta:e3}");
            assert_fails.Add($"  max error:{max_error:e3}");
        }

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData("Expected", ExpectedValue)
           .AddData("Actual", _ActualCollection);
    }

    /// <summary>Все элементы коллекции равны заданному значению</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy"></param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker ElementsAreEqualTo(double ExpectedValue, double Accuracy, string? Message = null)
    {
        if (double.IsNaN(Accuracy))
            throw new ArgumentException("Значение точности не может быть равно NaN", nameof(Accuracy));

        var index = 0;
        var assert_fails = new List<FormattableString>();
        var min_delta = double.PositiveInfinity;
        var max_delta = double.NegativeInfinity;
        var max_error = double.NegativeInfinity;
        foreach (var actual_value in _ActualCollection)
        {
            if (double.IsNaN(actual_value))
                throw new AssertFailedException($"{Message}error[{index}]: значение было равно NaN");

            var delta = ExpectedValue - actual_value;
            var delta_abs = Math.Abs(delta);
            if (delta_abs > Accuracy)
            {
                var rel_delta = delta / ExpectedValue;
                assert_fails.Add($"[{index,3}]:\r\n    {actual_value}\r\n != {ExpectedValue}\r\n      err:{delta_abs:e2}(rel.err:{rel_delta:e3})\r\n      eps:{Accuracy}");
                //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                min_delta = Math.Min(min_delta, delta);
                max_delta = Math.Max(max_delta, delta);
                max_error = Math.Max(max_error, delta_abs);
            }

            //Assert.AreEqual(
            //    ExpectedValue, actual_value, Accuracy,
            //    "{0}error[{1}]:{2}({3}), eps:{4}",
            //    Message.AddSeparator(), index,
            //    Math.Abs(ExpectedValue - actual_value).ToString("e2", CultureInfo.InvariantCulture),
            //    ((ExpectedValue - actual_value) / ExpectedValue).ToString("e3", CultureInfo.InvariantCulture),
            //    Accuracy);

            index++;
        }

        if (assert_fails.Count == 0) return this;

        if (assert_fails.Count > 1)
        {
            assert_fails.Add($"    min delta:{min_delta:e3}");
            assert_fails.Add($"    max delta:{max_delta:e3}");
            assert_fails.Add($"    max error:{max_error:e3}");
        }

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData("Expected", ExpectedValue)
           .AddData("Actual", _ActualCollection)
           .AddData("Accuracy", Accuracy);
    }

    /// <summary>Критерий проверки элементов коллекции</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Истина, если элемент соответствует критерию проверки</returns>
    public delegate bool ElementChecker(double ActualValue);

    /// <summary>Все элементы коллекции удовлетворяют условию</summary>
    /// <param name="Condition">Условие проверки всех элементов</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker ElementsAreSatisfyCondition(ElementChecker Condition, string? Message = null)
    {
        var index = 0;
        var assert_fails = new List<FormattableString>();
        foreach (var actual_value in _ActualCollection)
        {
            if (!Condition(actual_value))
                assert_fails.Add($"[{index,3}]:{actual_value} не удовлетворяет заданному условию");
            //Assert.IsTrue(Condition(actual_value), "{0}err.value[{1}]:{2}", Message, index, actual_value.ToString(CultureInfo.InvariantCulture));
            index++;
        }

        if (assert_fails.Count == 0) return this;

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData("Condition", Condition)
           .AddData("Actual", _ActualCollection);
    }

    /// <summary>Позиционный критерий проверки элементов коллекции</summary>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="ItemIndex">Индекс проверяемого элемента в коллекции</param>
    /// <returns>Истина, если элемент соответствует критерию проверки</returns>
    public delegate bool PositionElementChecker(double ActualValue, int ItemIndex);

    /// <summary>Все элементы коллекции удовлетворяют условию</summary>
    /// <param name="Condition">Условие проверки всех элементов</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public DoubleCollectionChecker ElementsAreSatisfyCondition(PositionElementChecker Condition, string? Message = null)
    {
        var index = 0;
        var assert_fails = new List<FormattableString>();
        foreach (var actual_value in _ActualCollection)
        {
            if (!Condition(actual_value, index))
                assert_fails.Add($"[{index,3}]:{actual_value} не удовлетворяет заданному условию");
            //Assert.IsTrue(Condition(actual_value, index), "{0}err.value[{1}]:{2}", Message, index, actual_value.ToString(CultureInfo.InvariantCulture));
            index++;
        }

        if (assert_fails.Count == 0) return this;

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData(Condition)
           .AddData("Actual", _ActualCollection);
    }

    /// <summary>Максимальное значение в коллекции</summary>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Max() => Assert.That.Value(_ActualCollection.Max());

    /// <summary>Минимальное значение в коллекции</summary>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Min() => Assert.That.Value(_ActualCollection.Min());

    /// <summary>Среднее значение в коллекции</summary>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Average() => Assert.That.Value(_ActualCollection.Average());

    #region Implementation of IEnumerable

    public IEnumerator<double> GetEnumerator() => _ActualCollection.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_ActualCollection).GetEnumerator();

    #endregion

    #region Implementation of ICollection<double>

    public void Add(double item) => _ActualCollection.Add(item);
    public void Clear() => _ActualCollection.Clear();
    public bool Contains(double item) => _ActualCollection.Contains(item);
    public void CopyTo(double[] array, int arrayIndex) => _ActualCollection.CopyTo(array, arrayIndex);
    public bool Remove(double item) => _ActualCollection.Remove(item);

    public int Count => _ActualCollection.Count;

    public bool IsReadOnly => _ActualCollection.IsReadOnly;

    #endregion
}