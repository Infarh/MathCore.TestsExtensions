namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Утилиты для создания сравнителей точности и пользовательских сравнителей/проверок равенства</summary>
public static class Accuracy
{
    /// <summary>Создаёт сравнитель чисел типа <see cref="double"/> с заданной абсолютной точностью</summary>
    /// <param name="eps">Допустимое отклонение (ε). Должно быть неотрицательным и не NaN</param>
    /// <returns>Экземпляр <see cref="IEqualityComparer{Double}"/> и <see cref="IComparer{Double}"/></returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="eps"/> &lt; 0</exception>
    /// <exception cref="ArgumentException">Если <paramref name="eps"/> равен <see cref="double.NaN"/></exception>
    public static IEqualityComparer<double> Eps(double eps) => new AccuracyComparer(eps);

    /// <summary>Создаёт сравнитель целых чисел типа <see cref="int"/> с заданной абсолютной точностью</summary>
    /// <param name="eps">Допустимое отклонение (ε). Должно быть неотрицательным</param>
    /// <returns>Экземпляр <see cref="IEqualityComparer{Int32}"/> и <see cref="IComparer{Int32}"/></returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="eps"/> &lt; 0</exception>
    public static IEqualityComparer<int> Eps(int eps) => new AccuracyComparer(eps);

    /// <summary>Создаёт универсальный сравнитель равенства на основе делегатов</summary>
    /// <typeparam name="T">Тип сравниваемых объектов</typeparam>
    /// <param name="Comparer">Делегат, выполняющий проверку равенства</param>
    /// <param name="Hasher">Делегат, вычисляющий хеш-код</param>
    /// <returns><see cref="IEqualityComparer{T}"/> использующий предоставленные делегаты</returns>
    /// <exception cref="ArgumentNullException">Если один из делегатов равен null</exception>
    public static IEqualityComparer<T> Equals<T>(Func<T, T, bool> Comparer, Func<T, int> Hasher) =>
        new AccuracyEqualityComparer<T>(Comparer ?? throw new ArgumentNullException(nameof(Comparer)),
            Hasher ?? throw new ArgumentNullException(nameof(Hasher)));

    /// <summary>Создаёт универсальный сравнитель порядка на основе делегата <see cref="Comparison{T}"/></summary>
    /// <typeparam name="T">Тип сравниваемых объектов</typeparam>
    /// <param name="Comparer">Делегат сравнения</param>
    /// <returns><see cref="IComparer{T}"/> использующий предоставленный делегат</returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="Comparer"/> равен null</exception>
    public static IComparer<T> Compare<T>(Comparison<T> Comparer) =>
        new AccuracyComparer<T>(Comparer ?? throw new ArgumentNullException(nameof(Comparer)));
}

/// <summary>Сравнитель значений типов <see cref="double"/> и <see cref="int"/> с учётом допустимой абсолютной погрешности</summary>
/// <remarks>
/// Для чисел с плавающей точкой равенство определяется условием |x - y| ≤ ε.
/// Для операций хеширования используется нормализация по шагу ε (округление к сетке).
/// </remarks>
/// <param name="Eps">Допустимое отклонение (ε). Должно быть неотрицательным</param>
public readonly struct AccuracyComparer(double Eps) :
    IEqualityComparer<double>, IComparer<double>,
    IEqualityComparer<int>, IComparer<int>
{
    /// <summary>Допустимое абсолютное отклонение</summary>
    private double Eps { get; init; } = Eps switch
    {
        < 0 => throw new ArgumentOutOfRangeException(nameof(Eps), Eps, "Значение точности не должно быть меньше нуля"),
        double.NaN => throw new ArgumentException("Значение точности не должно быть NaN", nameof(Eps)),
        _ => Eps
    };

    /// <summary>Проверяет равенство двух значений <see cref="double"/> с учётом точности</summary>
    public bool Equals(double x, double y) => Math.Abs(x - y) <= Eps;

    /// <summary>Вычисляет хеш-код для значения <see cref="double"/>, нормализуя его относительно точности</summary>
    /// <param name="x">Значение</param>
    /// <returns>Хеш-код</returns>
    public int GetHashCode(double x) => x is double.NaN
        ? x.GetHashCode()
        : (Math.Round(x * Eps) / Eps).GetHashCode();

    /// <summary>Сравнивает два значения <see cref="double"/> с учётом допустимой погрешности</summary>
    /// <param name="x">Первое значение</param>
    /// <param name="y">Второе значение</param>
    /// <returns>0 если считаются равными; -1 или 1 в зависимости от знака разности</returns>
    /// <exception cref="InvalidOperationException">Если одно из значений равно <see cref="double.NaN"/></exception>
    public int Compare(double x, double y)
    {
        var delta = x - y;
        if (delta is double.NaN)
            throw new InvalidOperationException("Сравнение с NaN")
            {
                Data =
                {
                    { nameof(x), x },
                    { nameof(y), y },
                }
            };
        return Math.Abs(delta) <= Eps
            ? 0
            : Math.Sign(delta);
    }

    /// <summary>Проверяет равенство двух значений <see cref="int"/> с учётом допустимого отклонения</summary>
    public bool Equals(int x, int y) => Math.Abs(x - y) <= Eps;

    /// <summary>Вычисляет хеш-код для целого значения с учётом нормализации к сетке точности</summary>
    public int GetHashCode(int x) => (Math.Round(x * Eps) / Eps).GetHashCode();

    /// <summary>Сравнивает два значения <see cref="int"/> с учётом допустимого отклонения</summary>
    /// <returns>0 если считаются равными; -1 или 1 в зависимости от знака разности</returns>
    public int Compare(int x, int y)
    {
        var delta = x - y;
        return Math.Abs(delta) <= Eps
            ? 0
            : Math.Sign(delta);
    }
}

/// <summary>Универсальный сравнитель равенства на основе переданных делегатов</summary>
/// <typeparam name="T">Тип сравниваемых объектов</typeparam>
/// <param name="Comparer">Делегат проверки равенства</param>
/// <param name="Hasher">Делегат вычисления хеш-кода</param>
public class AccuracyEqualityComparer<T>(Func<T, T, bool> Comparer, Func<T, int> Hasher) : IEqualityComparer<T>
{
    /// <summary>Проверяет равенство двух объектов</summary>
    public bool Equals(T x, T y) => Comparer(x, y);

    /// <summary>Вычисляет хеш-код объекта</summary>
    public int GetHashCode(T obj) => Hasher(obj);
}

/// <summary>Универсальный сравнитель порядка на основе делегата <see cref="Comparison{T}"/></summary>
/// <typeparam name="T">Тип сравниваемых объектов</typeparam>
/// <param name="Comparer">Делегат сравнения</param>
public class AccuracyComparer<T>(Comparison<T> Comparer) : IComparer<T>
{
    /// <summary>Сравнивает два значения</summary>
    public int Compare(T x, T y) => Comparer(x, y);
}
