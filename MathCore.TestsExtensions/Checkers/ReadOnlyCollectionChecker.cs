using System.Text;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Объект проверки коллекции</summary>
/// <typeparam name="T">Тип элементов коллекции</typeparam>
public class ReadOnlyCollectionChecker<T>
{
    /// <summary>Проверяемая коллекция</summary>
    public IReadOnlyCollection<T> ActualValue { get; }

    /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
    public CollectionAssert And => CollectionAssert.That;

    /// <summary>Инициализация нового объекта проверки коллекции</summary>
    /// <param name="ActualReadOnlyCollection">Проверяемая коллекция</param>
    internal ReadOnlyCollectionChecker(IReadOnlyCollection<T> ActualReadOnlyCollection) => ActualValue = ActualReadOnlyCollection;

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedReadOnlyCollection">Ожидаемая коллекция значений</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <returns>Исходный объект проверки значений</returns>
    public ReadOnlyCollectionChecker<T> IsEqualTo(IReadOnlyCollection<T> ExpectedReadOnlyCollection, string? Message = null)
    {
        Assert.That
           .Value(ActualValue.Count)
           .IsEqual(ExpectedReadOnlyCollection.Count, $"Размер коллекции {ActualValue.Count} не совпадает с ожидаемым размером {ExpectedReadOnlyCollection.Count}");

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedReadOnlyCollection.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Equals(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(expected, actual, "{0}error[{1}]:\r\n    ожидалось:{2}\r\n     получено:{3}", Message, index, expected, actual);

                index++;
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedReadOnlyCollection)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Метод сравнения значений элементов коллекции</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
    public delegate bool EqualityComparer(T ExpectedValue, T ActualValue);

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedReadOnlyCollection">Ожидаемая коллекция значений</param>
    /// <param name="Comparer">Метод проверки элементов коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public ReadOnlyCollectionChecker<T> IsEqualTo(IReadOnlyCollection<T> ExpectedReadOnlyCollection, EqualityComparer Comparer, string? Message = null)
    {
        Assert.That
           .Value(ActualValue.Count)
           .IsEqual(ExpectedReadOnlyCollection.Count, $"Размер коллекции {ActualValue.Count} не совпадает с ожидаемым размером {ExpectedReadOnlyCollection.Count}");

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedReadOnlyCollection.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual), "{0}error[{1}]:\r\n    ожидалось:{2}\r\n     получено:{3}", Message, index, expected, actual);

                index++;
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedReadOnlyCollection)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Метод сравнения значений элементов коллекции</summary>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="ActualValue">Проверяемое значение</param>
    /// <param name="Index">Индекс проверяемого элемента</param>
    /// <returns>Истина, если проверяемое значение соответствует ожидаемому</returns>
    public delegate bool EqualityPositionalComparer(T ExpectedValue, T ActualValue, int Index);

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedReadOnlyCollection">Ожидаемая коллекция значений</param>
    /// <param name="Comparer">Метод проверки элементов коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public ReadOnlyCollectionChecker<T> IsEqualTo(IReadOnlyCollection<T> ExpectedReadOnlyCollection, EqualityPositionalComparer Comparer, string? Message = null)
    {
        Assert.That.Value(ActualValue.Count).IsEqual(ExpectedReadOnlyCollection.Count);

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedReadOnlyCollection.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer(expected, actual, index))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer(expected, actual, index), "{0}error[{1}]:\r\n    ожидалось:{2}\r\n     получено:{3}", Message, index, expected, actual);

                index++;
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedReadOnlyCollection)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedReadOnlyCollection">Ожидаемая коллекция значений</param>
    /// <param name="Comparer">Объект проверки элементов коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public ReadOnlyCollectionChecker<T> IsEqualTo(IReadOnlyCollection<T> ExpectedReadOnlyCollection, IEqualityComparer<T> Comparer, string? Message = null)
    {
        Assert.That.Value(ActualValue.Count).IsEqual(ExpectedReadOnlyCollection.Count);

        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedReadOnlyCollection.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            while (actual_collection_enumerator.MoveNext() && expected_collection_enumerator.MoveNext())
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Comparer.Equals(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.IsTrue(Comparer.Equals(expected, actual),
                //    "{0}error[{1}]:\r\n    ожидалось:{2}\r\n     получено:{3}",
                //    Message, index, expected, actual);

                index++;
            }

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedReadOnlyCollection)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Проверка коллекции на совпадение с указанным набором значений</summary>
    /// <param name="items">Значения, из которых составлена коллекция</param>
    /// <returns>Исходный объект проверки значений</returns>
    public ReadOnlyCollectionChecker<T> IsEqualTo(params T[] items)
    {
        CountEquals(items.Length);

        var index = 0;
        var assert_fails = new List<FormattableString>();
        foreach (var actual in ActualValue)
        {
            if (index >= items.Length)
            {
                assert_fails.Add($"Размер актуальной коллекции больше ожидаемой ({items.Length})");
                break;
            }

            var expected = items[index];
            if (!Equals(actual, expected))
                assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
            //Assert.That.Value(actual).IsEqual(items[index], $"item[{index}]");
            index++;
        }
        if (index < items.Length)
            assert_fails.Add($"Размер актуальной коллекции ({index}) меньше размера ожидаемой коллекции ({items.Length})");

        if (assert_fails.Count == 0) return this;

        var message = assert_fails.Aggregate(
            new StringBuilder(),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData("Expected", items)
           .AddData("Actual", ActualValue);
    }

    /// <summary>Проверка коллекции на совпадение с указанным набором значений</summary>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <param name="items">Значения, из которых составлена коллекция</param>
    /// <returns>Исходный объект проверки значений</returns>
    public ReadOnlyCollectionChecker<T> IsEqualTo(string Message, params T[] items)
    {
        CountEquals(items.Length);

        var index = 0;
        var assert_fails = new List<FormattableString>();
        foreach (var actual in ActualValue)
        {
            if (index >= items.Length)
            {
                assert_fails.Add($"Размер актуальной коллекции больше ожидаемой ({items.Length})");
                break;
            }

            var expected = items[index];
            if (!Equals(actual, expected))
                assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
            //Assert.That.Value(actual).IsEqual(items[index], $"item[{index}]{Message}");
            index++;
        }

        if (assert_fails.Count == 0) return this;

        var message = assert_fails.Aggregate(
            new StringBuilder(Message.AddSeparator(Environment.NewLine)),
            (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
            S => S.ToString());
        throw new AssertFailedException(message)
           .AddData("Expected", items)
           .AddData("Actual", ActualValue);
    }

    /// <summary>По размеру и поэлементно эквивалентна ожидаемой коллекции</summary>
    /// <param name="ExpectedItems">Ожидаемый набор элементов</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <returns>Исходный объект проверки значений</returns>
    public ReadOnlyCollectionChecker<T> IsEqualTo(IEnumerable<T> ExpectedItems, string? Message = null)
    {
        IEnumerator<T>? expected_collection_enumerator = null;
        IEnumerator<T>? actual_collection_enumerator = null;
        try
        {
            expected_collection_enumerator = ExpectedItems.GetEnumerator();
            actual_collection_enumerator = ActualValue.GetEnumerator();

            var index = 0;
            var assert_fails = new List<FormattableString>();
            bool actual_move_next, expected_move_next;
            while ((actual_move_next = actual_collection_enumerator.MoveNext()) & (expected_move_next = expected_collection_enumerator.MoveNext()))
            {
                var expected = expected_collection_enumerator.Current;
                var actual = actual_collection_enumerator.Current;

                if (!Equals(expected, actual))
                {
                    assert_fails.Add($"[{index,3}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}");
                    //FormattableString message = $"{Message}error[{index}]:\r\n    ожидалось:{expected}\r\n     получено:{actual}";
                    //throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture));
                }

                //Assert.AreEqual(expected, actual, "{0}error[{1}]:\r\n    ожидалось:{2}\r\n     получено:{3}", Message, index, expected, actual);

                index++;
            }
            if (actual_move_next != expected_move_next)
                assert_fails.Add($"Размеры коллекций не совпадают. В актуальной коллекции найдено {index} элементов");
            //throw new AssertFailedException($"{Message.AddSeparator()}Размеры перечислений не совпадают");

            if (assert_fails.Count == 0) return this;

            var message = assert_fails.Aggregate(
                new StringBuilder(Message.AddSeparator(Environment.NewLine)),
                (S, s) => S.AppendLine(s.ToString(CultureInfo.InvariantCulture)),
                S => S.ToString());
            throw new AssertFailedException(message)
               .AddData("Expected", ExpectedItems)
               .AddData("Actual", ActualValue);
        }
        finally
        {
            expected_collection_enumerator?.Dispose();
            actual_collection_enumerator?.Dispose();
        }
    }

    /// <summary>Првоерка на соответствие размера коллекции ожидаемому значению</summary>
    /// <param name="ExpectedCount">Ожидаемый размер коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    /// <returns>Исходный объект проверки значений</returns>
    public ReadOnlyCollectionChecker<T> CountEquals(int ExpectedCount, string? Message = null)
    {
        var actual_count = ActualValue.Count;
        if (actual_count != ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {actual_count} не соответствует ожидаемому {ExpectedCount}")
               .AddData("ExpectedCount", ExpectedCount)
               .AddData("Actual", ActualValue); ;
        return this;
    }

    public ReadOnlyCollectionChecker<T> CountGreaterThan(int ExpectedCount, string? Message = null)
    {
        var actual_count = ActualValue.Count;
        if (actual_count > ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {actual_count} должен быть больше {ExpectedCount}")
               .AddData("ExpectedCount", ExpectedCount)
               .AddData("Actual", ActualValue); ;
        return this;
    }

    public ReadOnlyCollectionChecker<T> CountLessThan(int ExpectedCount, string? Message = null)
    {
        var actual_count = ActualValue.Count;
        if (actual_count < ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {actual_count} должен быть меньше {ExpectedCount}")
               .AddData("ExpectedCount", ExpectedCount)
               .AddData("Actual", ActualValue); ;
        return this;
    }

    public ReadOnlyCollectionChecker<T> CountGreaterOrEqualThan(int ExpectedCount, string? Message = null)
    {
        var actual_count = ActualValue.Count;
        if (actual_count >= ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {actual_count} должен быть больше, либо равен {ExpectedCount}")
               .AddData("ExpectedCount", ExpectedCount)
               .AddData("Actual", ActualValue); ;
        return this;
    }

    public ReadOnlyCollectionChecker<T> CountLessOrEqualThan(int ExpectedCount, string? Message = null)
    {
        var actual_count = ActualValue.Count;
        if (actual_count <= ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {actual_count} должен быть меньше, либо равен {ExpectedCount}")
               .AddData("ExpectedCount", ExpectedCount)
               .AddData("Actual", ActualValue); ;
        return this;
    }

    /// <summary>Максимальное значение в коллекции</summary>
    /// <param name="Selector">Метод оценки элемента коллекции</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Max(Func<T, double> Selector) => Assert.That.Value(ActualValue.Max(Selector));

    /// <summary>Минимальное значение в коллекции</summary>
    /// <param name="Selector">Метод оценки элемента коллекции</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Min(Func<T, double> Selector) => Assert.That.Value(ActualValue.Min(Selector));

    /// <summary>Среднее значение в коллекции</summary>
    /// <param name="Selector">Метод оценки элемента коллекции</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public DoubleValueChecker Average(Func<T, double> Selector) => Assert.That.Value(ActualValue.Average(Selector));

    /// <summary>Проверка, что коллекция содержит указанный элемент</summary>
    /// <param name="item">Элемент, который должен быть найден в коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public ReadOnlyCollectionChecker<T> Contains(T item, string? Message = null)
    {
        if (ActualValue.Contains(item))
            return this;

        FormattableString message = $"{Message.AddSeparator()}Коллекция не содержит элемент {item}";
        throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
           .AddData("ExpectedItem", item)
           .AddData("Actual", ActualValue);

        //Assert.IsTrue(ActualValue.Contains(item), "{0}Коллекция не содержит элемент {1}", Message.AddSeparator(), item);
    }

    /// <summary>Проверка, что коллекция содержит элемент, удовлетворяющий указанному критерию</summary>
    /// <param name="Predicate">Критерий проверки наличия элемента в коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
    public ReadOnlyCollectionChecker<T> Contains(Func<T, bool> Predicate, string? Message = null)
    {
        Assert.IsTrue(ActualValue.Any(Predicate), "{0}Коллекция не содержит элемент, удовлетворяющий заданным параметрам", Message.AddSeparator());
        return this;
    }

    /// <summary>Проверка, что указанного элемента нет в коллекции</summary>
    /// <param name="item">Элемент, которого не должно быть в коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
    public ReadOnlyCollectionChecker<T> NotContains(T item, string? Message = null)
    {
        Assert.IsTrue(!ActualValue.Contains(item), "{0}Коллекция содержит элемент {1}", Message.AddSeparator(), item);
        return this;
    }

    /// <summary>Проверка, что коллекция НЕ содержит элемент, удовлетворяющий указанному критерию</summary>
    /// <param name="Predicate">Критерий проверки наличия элемента в коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае если условие не выполнено</param>
    public ReadOnlyCollectionChecker<T> NotContains(Func<T, bool> Predicate, string? Message = null)
    {
        Assert.IsFalse(ActualValue.Any(Predicate), "{0}Коллекция не содержит элемент, удовлетворяющий заданным параметрам", Message.AddSeparator());
        return this;
    }

    /// <summary>Выполнение проверки элементов коллекции</summary>
    /// <param name="Check">Метод проверки элементов коллекции</param>
    /// <returns>Исходный объект проверки коллекции</returns>
    public ReadOnlyCollectionChecker<T> AllItems(Action<ValueChecker<T>> Check)
    {
        foreach (var value in ActualValue.Select(value => new ValueChecker<T>(value)))
            Check(value);
        return this;
    }

    /// <summary>Выполнение проверки элементов коллекции</summary>
    /// <param name="Check">Метод проверки элементов коллекции с учётом порядкового номера</param>
    /// <returns>Исходный объект проверки коллекции</returns>
    public ReadOnlyCollectionChecker<T> AllItems(Action<ValueChecker<T>, int> Check)
    {
        var i = 0;
        foreach (var value in ActualValue.Select(value => new ValueChecker<T>(value)))
            Check(value, i++);
        return this;
    }

    /// <summary>Объект проверки числа элементов</summary>
    public ValueChecker<int> ItemsCount => new(ActualValue.Count);

    /// <summary>Проверка на соответствие размера коллекции ожидаемому значению</summary>
    /// <param name="ExpectedCount">Ожидаемое значение размера коллекции</param>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки коллекции</returns>
    public ReadOnlyCollectionChecker<T> IsItemsCount(int ExpectedCount, string? Message = null)
    {
        var count = ActualValue.Count;
        if (count != ExpectedCount)
            throw new AssertFailedException($"{Message.AddSeparator()}Размер коллекции {count} не совпадает с ожидаемым {ExpectedCount}");
        return this;
    }

    /// <summary>Проверка - коллекция должна быть пуста</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки коллекции</returns>
    public ReadOnlyCollectionChecker<T> IsEmpty(string? Message = null)
    {
        var count = ActualValue.Count;
        if (count != 0)
            throw new AssertFailedException($"{Message.AddSeparator()}Число элементов коллекции {count} не равно 0 - коллекция не пуста");
        return this;
    }

    /// <summary>Проверка - коллекция должна быть не пуста</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки коллекции</returns>
    public ReadOnlyCollectionChecker<T> IsNotEmpty(string? Message = null)
    {
        if (ActualValue.Count == 0)
            throw new AssertFailedException($"{Message.AddSeparator()}Коллекция пуста");
        return this;
    }

    /// <summary>Проверка - коллекция должна содержать один единственный элемент</summary>
    /// <param name="Message">Сообщение, выводимое в случае нарушения условия</param>
    /// <returns>Объект проверки коллекции</returns>
    public ReadOnlyCollectionChecker<T> IsSingleItem(string? Message = null)
    {
        var count = ActualValue.Count;
        if (count != 1)
            throw new AssertFailedException($"{Message.AddSeparator()}Число элементов коллекции {count} не равно 1 - коллекция содержит не один единственный элемент");
        return this;
    }
}