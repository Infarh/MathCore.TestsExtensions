using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

// ReSharper disable UnusedMember.Global

// ReSharper disable UnusedType.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

internal delegate FormattableString ErrorFormatter(int index, object? Expected, object? Actual);

internal static class ErrorFormatterBuilder
{
    private static FormattableString GetErrorStr(int index, object? Expected, object? Actual) => $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}";

    private static FormattableString GetErrorSingleStr(int index, object? Expected, object? Actual)
    {
        var expected = (float?)Expected;
        var actual = (float?)Actual;

        var err = expected - actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorDoubleStr(int index, object? Expected, object? Actual)
    {
        var expected = (double?)Expected;
        var actual = (double?)Actual;

        var err = expected - actual;
        var err_rel = err / expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorByteStr(int index, object? Expected, object? Actual)
    {
        var expected = (byte?)Expected;
        var actual = (byte?)Actual;

        var err = (short?)expected - (short?)actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorSByteStr(int index, object? Expected, object? Actual)
    {
        var expected = (sbyte?)Expected;
        var actual = (sbyte?)Actual;

        var err = expected - actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorShortStr(int index, object? Expected, object? Actual)
    {
        var expected = (short?)Expected;
        var actual = (short?)Actual;

        var err = expected - actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorUShortStr(int index, object? Expected, object? Actual)
    {
        var expected = (short?)Expected;
        var actual = (short?)Actual;

        var err = (int?)expected - (int?)actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorIntStr(int index, object? Expected, object? Actual)
    {
        var expected = (int?)Expected;
        var actual = (int?)Actual;

        var err = expected - actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorUIntStr(int index, object? Expected, object? Actual)
    {
        var expected = (uint?)Expected;
        var actual = (uint?)Actual;

        var err = (long?)expected - (long?)actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorLongStr(int index, object? Expected, object? Actual)
    {
        var expected = (long?)Expected;
        var actual = (long?)Actual;

        var err = expected - actual;
        var err_rel = err / (double?)expected;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err:e2}(err.rel:{err_rel:e2})";
    }

    private static FormattableString GetErrorULongStr(int index, object? Expected, object? Actual)
    {
        var expected = (ulong?)Expected;
        var actual = (ulong?)Actual;

        var negative = expected < actual;
        var err = negative ? actual - expected : expected - actual;
        var err_rel = err / (double?)expected;

        var sign = negative ? '-' : (char?)null;

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{sign}{err:e2}(err.rel:{sign}{err_rel:e2})";
    }


    private static ErrorFormatter MakeFormatter(MethodInfo Subtraction, MethodInfo? Division) => (index, Expected, Actual) =>
    {
        var err = Subtraction.Invoke(null, new[] { Expected, Actual });
        if (Division is null)
            return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err}";

        var err_rel = Division.Invoke(null, new[] { err, Expected });

        return $"[{index,3}]:\r\n    ожидалось:{Expected}\r\n     получено:{Actual}\r\n          err:{err}(err.rel:{err_rel})";
    };

    private static ErrorFormatter GetFormatter(Type type)
    {
        var op_subtraction = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
           .FirstOrDefault(m =>
           {
               if (m.Name != "op_Subtraction") return false;
               if (m.ReturnType != m.DeclaringType) return false;

               var parameters = m.GetParameters();
               if (parameters.Length != 2) return false;

               var declaring_type = m.DeclaringType!;
               return parameters[0].ParameterType == declaring_type
                   && parameters[1].ParameterType == declaring_type;
           });

        if (op_subtraction is null)
            return GetErrorStr;

        var op_division = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
           .FirstOrDefault(m =>
           {
               if (m.Name != "op_Division") return false;
               if (m.ReturnType != m.DeclaringType) return false;

               var parameters = m.GetParameters();
               if (parameters.Length != 2) return false;

               var declaring_type = m.DeclaringType!;
               return parameters[0].ParameterType == declaring_type
                   && parameters[1].ParameterType == declaring_type;
           });

        return MakeFormatter(op_subtraction, op_division);
    }

    private static ErrorFormatter MakeFormatterByType(Type type)
    {
        if (type == typeof(double) || type == typeof(double?)) return GetErrorDoubleStr;
        if (type == typeof(float) || type == typeof(float?)) return GetErrorSingleStr;
        if (type == typeof(byte) || type == typeof(byte?)) return GetErrorByteStr;
        if (type == typeof(sbyte) || type == typeof(sbyte?)) return GetErrorSByteStr;
        if (type == typeof(short) || type == typeof(short?)) return GetErrorShortStr;
        if (type == typeof(ushort) || type == typeof(ushort?)) return GetErrorUShortStr;
        if (type == typeof(int) || type == typeof(int?)) return GetErrorIntStr;
        if (type == typeof(uint) || type == typeof(uint?)) return GetErrorUIntStr;
        if (type == typeof(long) || type == typeof(long?)) return GetErrorLongStr;
        if (type == typeof(ulong) || type == typeof(ulong?)) return GetErrorULongStr;
        return GetFormatter(type);
    }

    private static readonly ConcurrentDictionary<Type, ErrorFormatter> __Formatters = new();

    public static ErrorFormatter MakeFormatter(Type type) => __Formatters.GetOrAdd(type, t => MakeFormatterByType(t));
}

/// <summary>Объект проверки перечисления</summary>
/// <typeparam name="T">Тип элементов перечисления</typeparam>
public class EnumerableChecker<T>
{
    /// <summary>Проверяемое перечисление</summary>
    public IEnumerable<T> ActualValue { get; }

    /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
    public Assert And => Assert.That;

    /// <summary>Инициализация нового объекта проверки перечисления</summary>
    /// <param name="ActualEnumerable">Проверяемое перечисление</param>
    internal EnumerableChecker(IEnumerable<T> ActualEnumerable) => ActualValue = ActualEnumerable;

    private static void CountItems(bool IsNotEmpty, IEnumerator enumerator, ref int count)
    {
        if(!IsNotEmpty) return;
        do { count++; } while (enumerator.MoveNext());
    }

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> IsEqualTo(IEnumerable<T> ExpectedEnumerable, string? Message = null)
    {
        using var expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
        using var actual_collection_enumerator = ActualValue.GetEnumerator();

        var formatter = ErrorFormatterBuilder.MakeFormatter(typeof(T));

        var index = 0;
        bool actual_move_next, expected_move_next;
        var comparer = EqualityComparer<T>.Default;
        var assert_fails = new List<FormattableString>();
        while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
        {
            var expected = expected_collection_enumerator.Current;
            var actual = actual_collection_enumerator.Current;

            if (!comparer.Equals(expected, actual))
            {
                assert_fails.Add(formatter(index, expected, actual));
                //assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
            }

            index++;
        }

        if (actual_move_next != expected_move_next)
        {
            var count_expected = index;
            var count_actual = index;
            CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
            CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
            assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
        }

        if (assert_fails.Count == 0) return this;

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData("Expected", ExpectedEnumerable)
           .AddData("Actual", ActualValue);
    }

    /// <summary>Метод сравнения значений элементов перечисления</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
    public delegate bool EqualityComparer(T ExpectedValue, T ActualValue);

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Метод проверки элементов перечисления</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> IsEqualTo(IEnumerable<T> ExpectedEnumerable, EqualityComparer Comparer, string? Message = null)
    {
        var formatter = ErrorFormatterBuilder.MakeFormatter(typeof(T));

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual))
                {
                    assert_fails.Add(formatter(index, expected, actual));

                    //assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);
                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Метод сравнения значений элементов перечисления</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="Index">Индекс проверяемого элемента</param>
    /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
    public delegate bool EqualityPositionalComparer(T ExpectedValue, T ActualValue, int Index);

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Метод проверки элементов перечисления</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> IsEqualTo(IEnumerable<T> ExpectedEnumerable, EqualityPositionalComparer Comparer, string? Message = null)
    {
        var formatter = ErrorFormatterBuilder.MakeFormatter(typeof(T));

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                if (!Comparer(expected, actual, index))
                {
                    assert_fails.Add(formatter(index, expected, actual));
                    //assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);
                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Объект проверки элементов перечисления</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> IsEqualTo(IEnumerable<T> ExpectedEnumerable, IEqualityComparer<T> Comparer, string? Message = null)
    {
        var formatter = ErrorFormatterBuilder.MakeFormatter(typeof(T));

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer.Equals(expected, actual))
                {
                    assert_fails.Add(formatter(index, expected, actual));
                    //assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer.Equals(expected, actual),
                //    "{0}error[{1}]: ожидалось({2}), получено({3})",
                //    Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Объект проверки элементов перечисления</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> IsEqualTo(IEnumerable<T> ExpectedEnumerable, IComparer<T> Comparer, string? Message = null)
    {
        var formatter = ErrorFormatterBuilder.MakeFormatter(typeof(T));

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (Comparer.Compare(expected, actual) != 0)
                {
                    assert_fails.Add(formatter(index, expected, actual));
                    //assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer.Equals(expected, actual),
                //    "{0}error[{1}]: ожидалось({2}), получено({3})",
                //    Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Максимальное значение в перечислении</summary>
    /// <param name="Selector">Метод оценки элемента перечисления</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Max(Func<T, double> Selector) => Assert.That.Value(ActualValue.Max(Selector));

    /// <summary>Минимальное значение в перечислении</summary>
    /// <param name="Selector">Метод оценки элемента перечисления</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Min(Func<T, double> Selector) => Assert.That.Value(ActualValue.Min(Selector));

    /// <summary>Среднее значение в перечислении</summary>
    /// <param name="Selector">Метод оценки элемента перечисления</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Average(Func<T, double> Selector) => Assert.That.Value(ActualValue.Average(Selector));

    /// <summary>Проверка, что перечисление содержит указанный элемент</summary>
    /// <param name="item">Элемент, который должен быть найден в перечислении</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> Contains(T item, string? Message = null)
    {
        if (ActualValue.Contains(item))
            return this;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Перечисление не содержит элемент {item}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Проверка, что перечисление содержит элемент, удовлетворяющий указанному критерию</summary>
    /// <param name="Predicate">Критерий проверки наличия элемента в перечислении</param>
    /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
    public EnumerableChecker<T> Contains(Func<T, bool> Predicate, string? Message = null)
    {
        if (!ActualValue.Any(Predicate))
            return this;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Перечисление не содержит элемент";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
           .AddData(Predicate)
           .AddData("Actual", ActualValue);
    }

    /// <summary>Проверка, что указанного элемента нет в перечислении</summary>
    /// <param name="item">Элемент, которого не должно быть в перечислении</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> NotContains(T item, string? Message = null)
    {
        if (!ActualValue.Contains(item))
            return this;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Перечисление содержит элемент {item}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
           .AddData("NotExpected", item)
           .AddData("Actual", ActualValue);
    }

    /// <summary>Проверка, что перечисление НЕ содержит элемент, удовлетворяющий указанному критерию</summary>
    /// <param name="Predicate">Критерий проверки наличия элемента в перечислении</param>
    /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
    public EnumerableChecker<T> NotContains(Func<T, bool> Predicate, string? Message = null)
    {
        if (!ActualValue.Any(Predicate))
            return this;

        var msg = Message.AddSeparator();
        FormattableString message = $"{msg}Перечисление не содержит элемент, удовлетворяющий заданным параметрам";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
           .AddData(Predicate)
           .AddData("Actual", ActualValue);
    }

    /// <summary>Выполнение проверки для всех элементов перечисления</summary>
    /// <param name="Check">Метод проверки значения</param>
    /// <returns>Исходный объект проверки перечисления</returns>
    public EnumerableChecker<T> All(Action<ValueChecker<T>> Check)
    {
        foreach (var item in ActualValue)
            Check(new ValueChecker<T>(item));
        return this;
    }

    /// <summary>Выполнение проверки для всех элементов перечисления</summary>
    /// <param name="Check">Позиционный метод проверки значения</param>
    /// <returns>Исходный объект проверки перечисления</returns>
    public EnumerableChecker<T> All(Action<ValueChecker<T>, int> Check)
    {
        var index = 0;
        foreach (var item in ActualValue)
            Check(new ValueChecker<T>(item), index++);
        return this;
    }

    /// <summary>Выполнение проверки элементов перечисления</summary>
    /// <param name="Check">Метод проверки элементов перечисления</param>
    /// <returns>Исходный объект проверки перечисления</returns>
    public EnumerableChecker<T> Items(Action<ValueChecker<T>> Check)
    {
        foreach (var value in ActualValue.Select(value => new ValueChecker<T>(value)))
            Check(value);
        return this;
    }

    /// <summary>Выполнение проверки элементов перечисления</summary>
    /// <param name="Check">Метод проверки элементов перечисления с учётом порядкового номера</param>
    /// <returns>Исходный объект проверки перечисления</returns>
    public EnumerableChecker<T> Items(Action<ValueChecker<T>, int> Check)
    {
        var i = 0;
        foreach (var value in ActualValue.Select(value => new ValueChecker<T>(value)))
            Check(value, i++);
        return this;
    }

    /// <summary>Объект проверки числа элементов</summary>
    public ValueChecker<int> ItemsCount => new(ActualValue.Count());

    /// <summary>Проверка на соответствие размера перечисления ожидаемому значению</summary>
    /// <param name="ExpectedCount">Ожидаемое значение размера перечисления</param>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsItemsCount(int ExpectedCount, string? Message = null)
    {
        var count = ActualValue.Count();
        if (count != ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер перечисления {count} не совпадает с ожидаемым {ExpectedCount}")
               .AddData(ExpectedCount)
               .AddData("Actual", ActualValue);
        return this;
    }

    /// <summary>Проверка - перечисление должна быть пуста</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsEmpty(string? Message = null)
    {
        var count = ActualValue.Count();
        if (count != 0)
            throw new AssertFailedException($"{Message.AddSeparator()}Число элементов перечисления {count} не равно 0 - перечисление не пуста")
               .AddData("ItemsCount", count)
               .AddData("Actual", ActualValue);
        return this;
    }

    /// <summary>Проверка - перечисление должна быть не пуста</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsNotEmpty(string? Message = null)
    {
        if (!ActualValue.Any())
            throw new AssertFailedException($"{Message.AddSeparator()}Перечисление пусто")
               .AddData("Actual", ActualValue);
        return this;
    }

    /// <summary>Проверка - перечисление должна содержать один единственный элемент</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsSingleItem(string? Message = null)
    {
        var count = ActualValue.Count();
        if (count != 1)
            throw new AssertFailedException($"{Message.AddSeparator()}Число элементов перечисления {count} не равно 1 - перечисление содержит не один единственный элемент")
               .AddData("ItemsCount", count)
               .AddData("Actual", ActualValue);
        return this;
    }
}

/// <summary>Объект проверки перечисления</summary>
public class EnumerableChecker
{
    private static void CountItems(bool IsNotEmpty, IEnumerator enumerator, ref int count)
    {
        if (!IsNotEmpty) return;
        do { count++; } while (enumerator.MoveNext());
    }

    /// <summary>Проверяемое перечисление</summary>
    private readonly IEnumerable _ActualEnumerable;

    /// <summary>Инициализация нового объекта проверки перечисления</summary>
    /// <param name="ActualEnumerable">Проверяемое перечисление</param>
    internal EnumerableChecker(IEnumerable ActualEnumerable) => _ActualEnumerable = ActualEnumerable;

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker IsEqualTo(IEnumerable ExpectedEnumerable, string? Message = null)
    {
        IEnumerator? expected_collection_enumerator = null;
        IEnumerator? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = _ActualEnumerable.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Equals(expected, actual))
                {
                    if (actual is not null)
                    {
                        var formatter = ErrorFormatterBuilder.MakeFormatter(actual.GetType());
                        assert_fails.Add(formatter(index, expected, actual));
                    }
                    else
                        assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(expected, actual, "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", _ActualEnumerable);
        }
        finally
        {
            (expected_collection_enumerator as IDisposable)?.Dispose();
            (actual_collection_enumerator as IDisposable)?.Dispose();
        }
    }

    /// <summary>Метод сравнения значений элементов перечисления</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
    public delegate bool EqualityComparer(object? ExpectedValue, object? ActualValue);

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Метод сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker IsEqualTo(IEnumerable ExpectedEnumerable, EqualityComparer Comparer, string? Message = null)
    {
        IEnumerator? expected_collection_enumerator = null;
        IEnumerator? actual_collection_enumerator = null;

        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = _ActualEnumerable.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual))
                {
                    if (actual is not null)
                    {
                        var formatter = ErrorFormatterBuilder.MakeFormatter(actual.GetType());
                        assert_fails.Add(formatter(index, expected, actual));
                    }
                    else
                        assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(
                //    Comparer(expected, actual),
                //    "{0}error[{1}]: ожидалось({2}), получено({3})",
                //    Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", _ActualEnumerable);
        }
        finally
        {
            (expected_collection_enumerator as IDisposable)?.Dispose();
            (actual_collection_enumerator as IDisposable)?.Dispose();
        }
    }

    /// <summary>Метод сравнения значений элементов перечисления</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="Index">Индекс проверяемого элемента</param>
    /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
    public delegate bool EqualityPositionalComparer(object? ExpectedValue, object? ActualValue, int Index);

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Метод сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker IsEqualTo(IEnumerable ExpectedEnumerable, EqualityPositionalComparer Comparer, string? Message = null)
    {
        IEnumerator? expected_collection_enumerator = null;
        IEnumerator? actual_collection_enumerator = null;

        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = _ActualEnumerable.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual, index))
                {
                    if (actual is not null)
                    {
                        var formatter = ErrorFormatterBuilder.MakeFormatter(actual.GetType());
                        assert_fails.Add(formatter(index, expected, actual));
                    }
                    else
                        assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", _ActualEnumerable);
        }
        finally
        {
            (expected_collection_enumerator as IDisposable)?.Dispose();
            (actual_collection_enumerator as IDisposable)?.Dispose();
        }
    }

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Comparer">Объект сравнения</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker IsEqualTo(IEnumerable ExpectedEnumerable, IEqualityComparer Comparer, string? Message = null)
    {
        IEnumerator? expected_collection_enumerator = null;
        IEnumerator? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = _ActualEnumerable.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer.Equals(expected, actual))
                {
                    if (actual is not null)
                    {
                        var formatter = ErrorFormatterBuilder.MakeFormatter(actual.GetType());
                        assert_fails.Add(formatter(index, expected, actual));
                    }
                    else
                        assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(
                //    Comparer.Equals(expected, actual),
                //    "{0}error[{1}]: ожидалось({2}), получено({3})",
                //    Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
            {
                var count_expected = index;
                var count_actual = index;
                CountItems(expected_move_next, expected_collection_enumerator, ref count_expected);
                CountItems(actual_move_next, actual_collection_enumerator, ref count_actual);
                assert_fails.Add($"Размеры перечислений не совпадают.\r\n    размер актуальной коллекции:{count_actual}\r\n     размер ожидаемой коллекции:{count_expected}");
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedEnumerable)
               .AddData("Actual", _ActualEnumerable);
        }
        finally
        {
            (expected_collection_enumerator as IDisposable)?.Dispose();
            (actual_collection_enumerator as IDisposable)?.Dispose();
        }
    }

    /// <summary>Минимальное значение в перечислении</summary>
    /// <param name="Selector">Метод оценки элемента перечисления</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Max(Func<object, double> Selector) => Assert.That.Value(_ActualEnumerable.Cast<object>().Max(Selector));

    /// <summary>Минимальное значение в перечислении</summary>
    /// <param name="Selector">Метод оценки элемента перечисления</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Min(Func<object, double> Selector) => Assert.That.Value(_ActualEnumerable.Cast<object>().Min(Selector));

    /// <summary>Среднее значение в перечислении</summary>
    /// <param name="Selector">Метод оценки элемента перечисления</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Average(Func<object, double> Selector) => Assert.That.Value(_ActualEnumerable.Cast<object>().Average(Selector));
}