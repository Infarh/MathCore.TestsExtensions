using System.Collections;
using System.Globalization;
using System.Text;

// ReSharper disable UnusedMember.Global

// ReSharper disable UnusedType.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

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

    /// <summary>По размеру и поэлементно эквивалентно ожидаемому перечислению</summary>
    /// <param name="ExpectedEnumerable">Ожидаемое перечисление значений</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public EnumerableChecker<T> IsEqualTo(IEnumerable<T> ExpectedEnumerable, string? Message = null)
    {
        using var expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
        using var actual_collection_enumerator = ActualValue.GetEnumerator();

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
                assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
            }

            index++;
        }
        if (actual_move_next != expected_move_next)
            assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
        //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

        if (assert_fails.Count == 0) return this;

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message);
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
        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);
                index++;
            }
            if (actual_move_next != expected_move_next)
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
                //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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
        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;
                if (!Comparer(expected, actual, index))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);
                index++;
            }
            if (actual_move_next != expected_move_next)
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
            //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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
        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedEnumerable.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer.Equals(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer.Equals(expected, actual),
                //    "{0}error[{1}]: ожидалось({2}), получено({3})",
                //    Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
            //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
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
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
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
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
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
            throw new AssertFailedException($"{Message.AddSeparator()}Размер перечисления {count} не совпадает с ожидаемым {ExpectedCount}");
        return this;
    }

    /// <summary>Проверка - перечисление должна быть пуста</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsEmpty(string Message)
    {
        var count = ActualValue.Count();
        if (count != 0)
            throw new AssertFailedException($"{Message.AddSeparator()}Число элементов перечисления {count} не равно 0 - перечисление не пуста");
        return this;
    }

    /// <summary>Проверка - перечисление должна быть не пуста</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsNotEmpty(string Message)
    {
        if (!ActualValue.Any())
            throw new AssertFailedException($"{Message.AddSeparator()}Перечисление пуста");
        return this;
    }

    /// <summary>Проверка - перечисление должна содержать один единственный элемент</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки перечисления</returns>
    public EnumerableChecker<T> IsSingleItem(string Message)
    {
        var count = ActualValue.Count();
        if (count != 1)
            throw new AssertFailedException($"{Message.AddSeparator()}Число элементов перечисления {count} не равно 1 - перечисление содержит не один единственный элемент");
        return this;
    }
}

/// <summary>Объект проверки перечисления</summary>
public class EnumerableChecker
{
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
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Equals(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(expected, actual, "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
                //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual))
                {
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
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
                //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual, index))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]: ожидалось({2}), получено({3})", Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
                //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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
            bool actual_move_next, expected_move_next = false;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) && (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer.Equals(expected, actual))
                {
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
                assert_fails.Add($"Размеры перечислений не совпадают. Проверено {index} элементов");
                //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message);
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