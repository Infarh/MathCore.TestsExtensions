# MathCore.TestsExtensions

Расширения для MSTest, добавляющие удобный fluent-интерфейс к стандартным ассертам. Библиотека предоставляет точки расширения `Assert.That`, `CollectionAssert.That` и `StringAssert.That`, позволяющие писать выразительные, компактные и читаемые проверки без потери информативности сообщений об ошибках.

- Целевая платформа: .NET Standard 2.0 (работает в большинстве тестовых проектов на современном .NET)
- Зависимости: MSTest.TestFramework ≥ 4.0.2
- Лицензия: MIT

## Возможности

- Fluent-интерфейс для проверок значений, коллекций, перечислений и строк
- Расширения: `Assert.That`, `CollectionAssert.That`, `StringAssert.That`
- Чекеры:
  - `ValueChecker<T>` и специализированный `DoubleValueChecker` (сравнения с точностью, границы, преобразования типов)
  - `CollectionChecker<T>` и `EnumerableChecker<T>` (поэлементное сравнение, позиционные проверки, агрегаты `Max/Min/Average`, проверки наличия/отсутствия элементов)
  - Поддержка точности для числовых типов и информативные отчёты о расхождениях (включая относительную ошибку)
- Проверки исключений для методов/функций: `Assert.That.Method(...).Throw<TException>()`
- Строковые проверки через `StringAssert.That.Value(string)` с `StartWith`, `EndWith`, `Contains`, `Matches`, `DoesNotMatch`, `Is(Not)NullOr(White)Space`
- Понятные сообщения об ошибках с указанием индексов и сводок по различиям, а также данными `Expected/Actual` в `Exception.Data`
- Утилиты для логирования результатов теста: `TestResultExtensions` (`LogWriteLine`, `ToDebug`, `ToDebugEnum`)

## Установка

NuGet-пакет: MathCore.TestsExtensions

Пример установки:

```powershell
# PackageReference
Install-Package MathCore.TestsExtensions

# dotnet CLI
dotnet add package MathCore.TestsExtensions
```

## Быстрый старт

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SampleTests
{
    [TestMethod]
    public void Values_and_Collections()
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

## Точки расширения и fluent-паттерны

- `Assert.That`
  - `Value(T)`, `Value(double)` — проверки значений и чисел с учётом точности
  - `Method(Action/Func)` — проверки исключений
  - `Enumerable(IEnumerable<T>)` — проверки перечислений
  - `Collection(ICollection<T>/T[])` — проверки коллекций
- `CollectionAssert.That`
  - `Collection(…)` — проверки `ICollection<T>`, массивов и специальных форматов (включая `double[,]`)
- `StringAssert.That`
  - `Value(string)` — строковые проверки и проверки по регулярным выражениям

Каждый чекер поддерживает цепочки и имеет свойство `And => Assert.That`, упрощающее продолжение проверок.

## Примеры

Значения и точность:
```csharp
Assert.That.Value(1.1).LessOrEqualsThan(1.0, 0.1);
Assert.That.Value(10).GreaterThan(5);
```

Исключения:
```csharp
Assert.That.Method(() => throw new InvalidOperationException())
    .Throw<InvalidOperationException>();
```

Перечисления:
```csharp
IEnumerable<string> actual   = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };
IEnumerable<string> expected = new[] { "file3.txt", "file4.txt", "file5.txt", "file6.txt" };

Assert.That.Enumerable(actual)
    .IsEqualTo(expected)
    .Contains(s => s.EndsWith(".txt"));
```

Коллекции:
```csharp
var items    = new[] { 1, 3, 5, 7 };
var expected = new[] { 1, 3, 5, 7 };

CollectionAssert.That.Collection(items)
    .IsItemsCount(4)
    .Contains(5)
    .IsEqualTo(expected);
```

Строки:
```csharp
StringAssert.That.Value("Hello, World!")
    .StartWith("Hello")
    .Contains("World")
    .EndWith("!")
    .Matches(@"^Hello,\sWorld!$");
```

## Структура репозитория

- `MathCore.TestsExtensions/` — библиотека (`MathCore.TestsExtensions.csproj`), сборка пакета NuGet
- `Tests/MathCore.TestsExtensions.Tests/` — модульные тесты (`MathCore.TestsExtensions.Tests.csproj`)
- `.github/workflows/` — CI-пайплайны для тестирования и публикации

## CI/CD

- Тестирование и сборка выполняются в GitHub Actions (`.github/workflows/testing.yml`)
- Публикация пакета в NuGet через GitHub Actions (`.github/workflows/publish.yml`)
- В пакете включены символы и SourceLink для удобной отладки

## Обратная связь и вклад

- Идеи и баг-репорты — через Issues
- PR приветствуются: тесты, улучшение сообщений об ошибках, новые fluent-проверки

## Лицензия

MIT