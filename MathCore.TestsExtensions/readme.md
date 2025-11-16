# MathCore.TestsExtensions

Расширения для MSTest, добавляющие удобный fluent-интерфейс к стандартным ассертам через точки расширения:
- `Assert.That`
- `CollectionAssert.That`
- `StringAssert.That`

Пакет помогает писать выразительные и компактные проверки со связным API: проверка значений, коллекций, перечислений, строк и исключений с наглядными сообщениями об ошибках.

## Установка

- Платформа: .NET Standard 2.0
- Зависимость: MSTest.TestFramework >= 4.0.2

NuGet-пакет: MathCore.TestsExtensions

## Быстрый старт

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SampleTests
{
    [TestMethod]
    public void Value_and_Collections()
    {
        Assert.That.Value(42).IsEqual(42);

        var items = new[] { 1, 3, 5, 7 };
        Assert.That.Collection(items)
            .IsNotEmpty()
            .Contains(5)
            .IsEqualTo(1, 3, 5, 7);
    }
}
```

## Точки расширения и fluent-интерфейс

Пакет добавляет расширение `That` к стандартным ассертам:
- `Assert.That` — проверки значений (`Value`), функций/действий (`Method`), перечислений (`Enumerable`) и коллекций (`Collection`)
- `CollectionAssert.That` — проверки коллекций (`Collection`)
- `StringAssert.That` — проверки строк через `Value(string)`

Базовый паттерн использования: начать с нужной «точки», получить «чекер» и вызывать цепочкой методы-проверки. Для удобства большинство чекеров имеют свойство `And => Assert.That`, позволяя продолжать цепочку с новой проверки.

### Примеры: Assert.That

Проверка значения и сравнение с точностью:
```csharp
Assert.That.Value(1.1).LessOrEqualsThan(1.0, 0.1);
Assert.That.Value(10).GreaterThan(5);
```

Проверка исключений:
```csharp
Assert.That.Method(() => throw new InvalidOperationException())
    .Throw<InvalidOperationException>();

Assert.That.Method(() => 1 / 0)
    .Throw<DivideByZeroException>();
```

Перечисления (`IEnumerable<T>`):
```csharp
IEnumerable<string> actual   = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };
IEnumerable<string> expected = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };

Assert.That.Enumerable(actual).IsEqualTo(expected);
Assert.That.Enumerable(actual).Contains(s => s.EndsWith(".txt"));
```

### Примеры: CollectionAssert.That

Проверки для `ICollection<T>` и массивов:
```csharp
var items = new[] { 1, 3, 5, 7 };

CollectionAssert.That.Collection(items)
    .IsItemsCount(4)
    .Contains(5)
    .IsEqualTo(1, 3, 5, 7);

var expected = new[] { 1, 3, 5, 7 };
CollectionAssert.That.Collection(items).IsEqualTo(expected);
```

Покрытие сценариев с точностью для double:
```csharp
double[] actual   = { 1.0, 2.0, 3.000000001 };
double[] expected = { 1.0, 2.0, 3.0 };

Assert.That.Collection(actual).IsEqualTo(expected, 1e-8);
```

### Примеры: StringAssert.That

Строковые проверки через `ValueChecker<string>`:
```csharp
StringAssert.That.Value("Hello, World!")
    .StartWith("Hello")
    .Contains("World")
    .EndWith("!")
    .Matches(@"^Hello,\sWorld!$");
```

## Дополнительно: работа с элементами перечислений/коллекций

Позиционные проверки и сводные метрики:
```csharp
var xs = Enumerable.Range(0, 10).ToArray();

Assert.That.Collection(xs)
    .ItemsCount.IsEqual(10)
    .AllItems((v, i) => v.IsEqual(i));

Assert.That.Enumerable(xs)
    .Max(x => x).IsEqual(9)
    .Min(x => x).IsEqual(0)
    .Average(x => x).IsEqual(4.5);
```

## Сообщения об ошибках

Чекеры формируют понятные сообщения: при неравенстве указываются индексы элементов и сводки по расхождениям (включая относительную ошибку для числовых типов), а также прикладываются `Expected` и `Actual` в `Exception.Data`.

## Лицензия

MIT
