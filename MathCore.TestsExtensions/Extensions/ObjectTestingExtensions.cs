using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting.Attributes;

using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace Microsoft.VisualStudio.TestTools.UnitTesting.Extensions;

/// <summary>Набор расширений для упрощения написания проверок в тестах</summary>
public static class ObjectTestingExtensions
{
    /// <summary>Гарантирует, что ссылка не равна null и возвращает проверяемое значение</summary>
    /// <typeparam name="T">Тип значения</typeparam>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="Message">Сообщение об ошибке, по умолчанию используется выражение аргумента</param>
    /// <returns>Непустое значение</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    [return: NotNullIfNotNull(nameof(value))]
    // ReSharper disable ConvertToExtensionBlock
    public static T AssertNotNull<T>(this T value, [CallerArgumentExpression(nameof(value))] string? Message = null) => value.AssertThatValue().AsNotNull(Message)!;

    /// <summary>Гарантирует, что ссылка равна null</summary>
    /// <typeparam name="T">Тип значения</typeparam>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="Message">Сообщение об ошибке, по умолчанию используется выражение аргумента</param>
    public static void AssertIsNull<T>(this T value, [CallerArgumentExpression(nameof(value))] string? Message = null) => value.AssertThatValue().IsNull(Message);
    // ReSharper restore ConvertToExtensionBlock

    extension<T>(T value)
    {
        /// <summary>Проверяет, что значение удовлетворяет переданному логическому выражению</summary>
        /// <param name="expression">Логическое выражение проверки</param>
        /// <param name="Message">Сообщение об ошибке, по умолчанию используется выражение аргумента</param>
        /// <returns>Объект проверки значения</returns>
        public ValueChecker<T> Assert(Expression<Func<T, bool>> expression, [CallerArgumentExpression(nameof(expression))] string? Message = null)
        {
            var tester = expression.Compile(true);

            if (tester(value)) return new(value);

            FormattableString message = $"{Message.AddSeparator()}Значение {value} не удовлетворяет условию {expression}";
            throw new AssertFailedException(message.ToStringInvariant())
                .AddData("value", value)
                .AddData("Expression", expression);

        }

        /// <summary>Проверяет, что выполнение выражения над значением приводит к ожидаемому исключению</summary>
        /// <typeparam name="TException">Тип ожидаемого исключения</typeparam>
        /// <param name="ThrowException">Выражение, которое должно сгенерировать исключение</param>
        /// <param name="Message">Сообщение об ошибке, по умолчанию используется выражение аргумента</param>
        /// <returns>Объект проверки исключения</returns>
        public ValueChecker<TException> AssertThrow<TException>(Expression<Action<T>> ThrowException, [CallerArgumentExpression(nameof(ThrowException))] string? Message = null) where TException : Exception
        {
            var tester = ThrowException.Compile(true);

            try
            {
                tester(value);
                FormattableString message = $"{Message.AddSeparator()}Для значения {value} выражение {ThrowException} не вызвало ошибки";
                throw new AssertFailedException(message.ToStringInvariant())
                    .AddData("value", value)
                    .AddData("Expression", ThrowException);
            }
            catch (TException exception)
            {
                return new(exception);
            }
            catch (Exception exception)
            {
                FormattableString message = $"{Message.AddSeparator()}Для значения {value} выражение {ThrowException} вызвало неожиданную ошибку {exception.GetType()}:{exception.Message}";
                throw new AssertFailedException(message.ToStringInvariant())
                    .AddData("value", value)
                    .AddData("Expression", ThrowException)
                    .AddData("exception", exception);
            }
        }
    }

    /// <summary>Результат является истинным</summary>
    /// <param name="value">Проверяемое выражение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки выражения типа <see cref="bool"/></returns>
    // ReSharper disable once ConvertToExtensionBlock
    public static ValueChecker<bool> AssertTrue(this bool value, [CallerArgumentExpression(nameof(value))] string? Message = null) =>
        That
           .Value(value)
           .IsEqual(true, Message);

    /// <summary>Результат является ложным</summary>
    /// <param name="value">Проверяемое выражение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки выражения типа <see cref="bool"/></returns>
    public static ValueChecker<bool> AssertFalse(this bool value, [CallerArgumentExpression(nameof(value))] string? Message = null) =>
        That
           .Value(value)
           .IsEqual(false, Message);
    // ReSharper restore ConvertToExtensionBlock

    /// <summary>Создаёт объект проверки значения</summary>
    /// <typeparam name="T">Тип значения</typeparam>
    /// <param name="value">Проверяемое значение</param>
    /// <returns>Объект проверки значения</returns>
    public static ValueChecker<T> AssertThatValue<T>(this T value) => That.Value(value);

    /// <summary>Проверка, что вещественное значение равно указанному ожидаемому</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ActualValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertEquals(
        this double value,
        double ActualValue,
        [CallerArgumentExpression(nameof(value))]
        string? Message = null) =>
        (DoubleValueChecker)That
           .Value(value)
           .IsEqual(ActualValue, Message);

    /// <summary>Проверяет, что значение эквивалентно ожидаемому</summary>
    /// <typeparam name="T">Тип значения</typeparam>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ActualValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки значения</returns>
    public static ValueChecker<T> AssertEquals<T>(
        this T value, 
        T ActualValue, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .IsEqual(ActualValue, Message);

    /// <summary>Проверяет равенство вещественного значения с заданной точностью</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ActualValue">Ожидаемое значение</param>
    /// <param name="Eps">Допустимая погрешность</param>
    /// <param name="Message">Сообщение об ошибке (при отсутствии указывается проверяемое выражение)</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertEquals(
        this double value, 
        double ActualValue, 
        double Eps, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) =>
        (DoubleValueChecker)That
           .Value(value)
           .IsEqual(ActualValue, Eps, Message);

    /// <summary>Создаёт объект проверки коллекции</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertThatCollection<T>(this ICollection<T> collection) => 
        That
           .Collection(collection);

    /// <summary>Проверяет размер массива на равенство ожидаемому значению</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="Count">Ожидаемое количество элементов</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertCount<T>(this T[] collection, int Count, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .CountEquals(Count, Message);

    /// <summary>Проверяет размер списка на равенство ожидаемому значению</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="Count">Ожидаемое количество элементов</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertCount<T>(this List<T> collection, int Count, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .CountEquals(Count, Message);

    /// <summary>Проверяет размер коллекции на равенство ожидаемому значению</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="Count">Ожидаемое количество элементов</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    // ReSharper disable ConvertToExtensionBlock
    public static CollectionChecker<T> AssertCount<T>(this ICollection<T> collection, int Count, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .CountEquals(Count, Message);

    /// <summary>Проверяет, что коллекция пуста</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsEmpty(Message);
    // ReSharper restore ConvertToExtensionBlock

    /// <summary>Проверяет, что массив пуст</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsEmpty<T>(this T[] collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsEmpty(Message);

    /// <summary>Проверяет, что список пуст</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsEmpty<T>(this List<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsEmpty(Message);

    /// <summary>Проверяет, что коллекция не пуста</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsNotEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsNotEmpty(Message);

    /// <summary>Проверяет, что массив не пуст</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsNotEmpty<T>(this T[] collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsNotEmpty(Message);

    /// <summary>Проверяет, что список не пуст</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsNotEmpty<T>(this List<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsNotEmpty(Message);

    /// <summary>Проверяет, что коллекция содержит ровно один элемент</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsSingle<T>(this ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsSingleItem(Message);

    /// <summary>Проверяет, что массив содержит ровно один элемент</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsSingle<T>(this T[] collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsSingleItem(Message);

    /// <summary>Проверяет, что список содержит ровно один элемент</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertIsSingle<T>(this List<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) => 
        That
           .Collection(collection)
           .IsSingleItem(Message);

    /// <summary>Проверяет коллекцию на совпадение с указанным набором значений</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this T[] collection, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию на совпадение с указанным набором значений</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this List<T> collection, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию на совпадение с указанным набором значений</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this ReadOnlyCollection<T> collection, params T[] args) =>
        That
           .Collection((ICollection<T>)collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию на совпадение с указанным набором значений</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this IList<T> collection, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию на совпадение с указанным набором значений</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this ICollection<T> collection, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию вещественных чисел на совпадение с набором значений</summary>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleCollectionChecker AssertEquals(this ICollection<double> collection, params double[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию вещественных чисел на совпадение с набором значений</summary>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleCollectionChecker AssertEquals(this ReadOnlyCollection<double> collection, params double[] args) =>
        That
           .Collection((ICollection<double>)collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию на совпадение с набором значений с использованием компаратора</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this T[] collection, IEqualityComparer<T> Comparer, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию на совпадение с набором значений с использованием компаратора</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this List<T> collection, IEqualityComparer<T> Comparer, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию на совпадение с набором значений с использованием компаратора</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this ReadOnlyCollection<T> collection, IEqualityComparer<T> Comparer, params T[] args) =>
        That
           .Collection((ICollection<T>)collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию на совпадение с набором значений с использованием компаратора</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static CollectionChecker<T> AssertEquals<T>(this ICollection<T> collection, IEqualityComparer<T> Comparer, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию вещественных чисел на совпадение с набором значений с использованием компаратора</summary>
    /// <param name="collection">Проверяемый массив</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleCollectionChecker AssertEquals(this double[] collection, IEqualityComparer<double> Comparer, params double[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию вещественных чисел на совпадение с набором значений с использованием компаратора</summary>
    /// <param name="collection">Проверяемый список</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleCollectionChecker AssertEquals(this List<double> collection, IEqualityComparer<double> Comparer, params double[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию вещественных чисел на совпадение с набором значений с использованием компаратора</summary>
    /// <param name="collection">Проверяемая коллекция</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleCollectionChecker AssertEquals(this ICollection<double> collection, IEqualityComparer<double> Comparer, params double[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию вещественных чисел на совпадение с набором значений с использованием компаратора</summary>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleCollectionChecker AssertEquals(this ReadOnlyCollection<double> collection, IEqualityComparer<double> Comparer, params double[] args) =>
        That
           .Collection((ICollection<double>)collection)
           .IsEqualTo(args, Comparer);

    /* ------------------------------------------------------------------------------------------------------------- */

    /// <summary>Создаёт объект проверки коллекции только для чтения</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertThatCollection<T>(this IReadOnlyCollection<T> collection) =>
        That
           .Collection(collection);

    /// <summary>Проверяет размер коллекции только для чтения на равенство ожидаемому значению</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Count">Ожидаемое количество элементов</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertCount<T>(this IReadOnlyCollection<T> collection, int Count, [CallerArgumentExpression(nameof(collection))] string? Message = null) =>
        That
           .Collection(collection)
           .CountEquals(Count, Message);

    /// <summary>Проверяет, что коллекция только для чтения пуста</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertIsEmpty<T>(this IReadOnlyCollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) =>
        That
           .Collection(collection)
           .IsEmpty(Message);

    /// <summary>Проверяет, что коллекция только для чтения не пуста</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertIsNotEmpty<T>(this IReadOnlyCollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) =>
        That
           .Collection(collection)
           .IsNotEmpty(Message);

    /// <summary>Проверяет, что коллекция только для чтения содержит один элемент</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertIsSingle<T>(this IReadOnlyCollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? Message = null) =>
        That
           .Collection(collection)
           .IsSingleItem(Message);

    /// <summary>Проверяет коллекцию только для чтения на совпадение с набором значений</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertEquals<T>(this IReadOnlyCollection<T> collection, params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию вещественных чисел только для чтения на совпадение с набором значений</summary>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleReadOnlyCollectionChecker AssertEquals(this IReadOnlyCollection<double> collection, params double[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args);

    /// <summary>Проверяет коллекцию только для чтения на совпадение с набором значений с использованием компаратора</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции</returns>
    public static ReadOnlyCollectionChecker<T> AssertEquals<T>(
        this IReadOnlyCollection<T> collection,
        IEqualityComparer<T> Comparer,
        params T[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /// <summary>Проверяет коллекцию вещественных чисел только для чтения на совпадение с набором значений с использованием компаратора</summary>
    /// <param name="collection">Проверяемая коллекция только для чтения</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="args">Ожидаемые элементы</param>
    /// <returns>Объект проверки коллекции вещественных значений</returns>
    public static DoubleReadOnlyCollectionChecker AssertEquals(
        this IReadOnlyCollection<double> collection,
        IEqualityComparer<double> Comparer,
        params double[] args) =>
        That
           .Collection(collection)
           .IsEqualTo(args, Comparer);

    /* ------------------------------------------------------------------------------------------------------------- */

    /// <summary>Создаёт объект проверки перечисления</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertThatEnumerable<T>(this IEnumerable<T> items) => 
        That
           .Enumerable(items);

    /// <summary>Проверяет, что перечисление пусто</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertEnumerableIsEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? Message = null) => 
        That
           .Enumerable(items)
           .IsEmpty(Message);

    /// <summary>Проверяет, что перечисление не пусто</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertEnumerableIsNotEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? Message = null) => 
        That
           .Enumerable(items)
           .IsNotEmpty(Message);

    /// <summary>Проверяет, что перечисление содержит один элемент</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertEnumerableIsSingleItem<T>(this IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? Message = null) => 
        That
           .Enumerable(items)
           .IsSingleItem(Message);

    /// <summary>Проверяет, что количество элементов перечисления равно ожидаемому</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <param name="ExpectedCount">Ожидаемое количество элементов</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertEnumerableCount<T>(this IEnumerable<T> items, int ExpectedCount, [CallerArgumentExpression(nameof(items))] string? Message = null) => 
        That
           .Enumerable(items)
           .IsItemsCount(ExpectedCount, Message);

    /// <summary>Проверяет перечисление на совпадение с указанными значениями</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <param name="values">Ожидаемые элементы</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertEquals<T>(this IEnumerable<T> items, params T[] values) => 
        That
           .Enumerable(items)
           .IsEqualTo(values);

    /// <summary>Проверяет перечисление на совпадение с указанными значениями с использованием компаратора</summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="items">Проверяемое перечисление</param>
    /// <param name="Comparer">Компаратор сравнения элементов</param>
    /// <param name="values">Ожидаемые элементы</param>
    /// <returns>Объект проверки перечисления</returns>
    public static EnumerableChecker<T> AssertEquals<T>(
        this IEnumerable<T> items, 
        IEqualityComparer<T> Comparer, 
        params T[] values) => 
        That
           .Enumerable(items)
           .IsEqualTo(values, Comparer);

    /// <summary>Проверяет, что значение меньше ожидаемого</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertLessThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .LessThan(ExpectedValue, Message);
    
    /// <summary>Проверяет, что значение меньше ожидаемого с учётом допустимой погрешности</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Допустимая погрешность</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertLessThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .LessThan(ExpectedValue, Accuracy, Message);
    
    /// <summary>Проверяет, что значение меньше или равно ожидаемому</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertLessOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .LessOrEqualsThan(ExpectedValue, Message);
    
    /// <summary>Проверяет, что значение меньше или равно ожидаемому с учётом допустимой погрешности</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Допустимая погрешность</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertLessOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .LessOrEqualsThan(ExpectedValue, Accuracy, Message);
    
    /// <summary>Проверяет, что значение больше ожидаемого</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertGreaterThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .GreaterThan(ExpectedValue, Message);
    
    /// <summary>Проверяет, что значение больше ожидаемого с учётом допустимой погрешности</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Допустимая погрешность</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertGreaterThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .GreaterThan(ExpectedValue, Accuracy, Message);
    
    /// <summary>Проверяет, что значение больше или равно ожидаемому</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertGreaterOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .GreaterOrEqualsThan(ExpectedValue, Message);
    
    /// <summary>Проверяет, что значение больше или равно ожидаемому с учётом допустимой погрешности</summary>
    /// <param name="value">Проверяемое значение</param>
    /// <param name="ExpectedValue">Ожидаемое значение</param>
    /// <param name="Accuracy">Допустимая погрешность</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <returns>Объект проверки вещественного значения</returns>
    public static DoubleValueChecker AssertGreaterOrEqualsThan(
        this double value, 
        double ExpectedValue, 
        double Accuracy, 
        [CallerArgumentExpression(nameof(value))] 
        string? Message = null) => 
        That
           .Value(value)
           .GreaterOrEqualsThan(ExpectedValue, Accuracy, Message);
}
