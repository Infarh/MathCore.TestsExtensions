// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Методы-расширения для объекта проверки целочисленных значений</summary>
public static class ValueCheckerIntExtensions
{
    /// <param name="Checker">Объект проверки целочисленного значения</param>
    extension(ValueChecker<int> Checker)
    {
        /// <summary>Проверка, что проверяемое значение равно ожидаемому с заданной точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> IsEqual(int ExpectedValue, int Accuracy, string? Message = null)
        {
            var delta = Math.Abs(ExpectedValue - Checker.ActualValue);
            if (delta <= Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}actual:{Checker.ActualValue}
                                             != {ExpectedValue}
                                                  err:{delta}(err.rel:{(ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue:e3})
                                             accuracy:{Accuracy}
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue)
                .AddData(Accuracy);
        }

        /// <summary>Проверка, что проверяемое значение не равно ожидаемому с заданной точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> IsNotEqual(int ExpectedValue, int Accuracy, string? Message = null)
        {
            var delta = Math.Abs(ExpectedValue - Checker.ActualValue);
            if (delta >= Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}actual:{Checker.ActualValue}
                                             == {ExpectedValue}
                                                  err:{delta}(err.rel:{(ExpectedValue - Checker.ActualValue) / (double)Checker.ActualValue:e3})
                                             accuracy:{Accuracy}
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue)
                .AddData(Accuracy);
        }

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> GreaterThan(int ExpectedValue, string? Message = null)
        {
            if (Checker.ActualValue > ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}Значение
                                             {Checker.ActualValue} должно быть больше
                                             {ExpectedValue}
                                             err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue);
        }

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> GreaterOrEqualsThan(int ExpectedValue, string? Message = null)
        {
            if (Checker.ActualValue >= ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}Нарушено условие
                                             {Checker.ActualValue}
                                          >= {ExpectedValue}
                                             err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue);
        }

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> GreaterOrEqualsThan(int ExpectedValue, int Accuracy, string? Message = null)
        {
            if (Checker.ActualValue - ExpectedValue <= Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}Нарушено условие
                                             {Checker.ActualValue}
                                          >= {ExpectedValue}
                                             err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})
                                             accuracy:{Accuracy}
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue)
                .AddData(Accuracy);
        }

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> LessThan(int ExpectedValue, string? Message = null)
        {
            if (Checker.ActualValue < ExpectedValue)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}Значение
                                             {Checker.ActualValue} должно быть меньше
                                             {ExpectedValue}
                                             err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue);
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> LessOrEqualsThan(int ExpectedValue, string? Message = null)
        {
            if (Checker.ActualValue <= ExpectedValue)
                return Checker;

            FormattableString message = $"""
                                         {Message.AddSeparator()}Значение
                                             {Checker.ActualValue} должно быть меньше, либо равно
                                             {ExpectedValue}
                                             err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue);
        }

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> LessOrEqualsThan(int ExpectedValue, int Accuracy, string? Message = null)
        {
            if (ExpectedValue - Checker.ActualValue <= Accuracy)
                return Checker;

            var msg = Message.AddSeparator();
            FormattableString message = $"""
                                         {msg}Нарушено условие
                                             {Checker.ActualValue}
                                          >= {ExpectedValue}
                                             err:{ExpectedValue - Checker.ActualValue:e3}(err.rel:{(ExpectedValue - Checker.ActualValue) / ExpectedValue:e3})
                                             accuracy:{Accuracy}
                                         """;
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Expected", ExpectedValue)
                .AddData("Actual", Checker.ActualValue)
                .AddData(Accuracy);
        }

        /// <summary>Проверка - является ли число чётным?</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> IsEven(string? Message = null)
        {
            if (Checker.ActualValue % 2 == 0)
                return Checker;

            FormattableString message = $"{Message.AddSeparator()}Число {Checker.ActualValue} не является чётным";
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Actual", Checker.ActualValue);
        }

        /// <summary>Проверка - является ли число нечётным?</summary>
        /// <param name="Message">Сообщение, выводимое в случае ошибки</param>
        /// <returns>Объект проверки целочисленного значения</returns>
        public ValueChecker<int> IsOdd(string? Message = null)
        {
            if (Checker.ActualValue % 2 != 0)
                return Checker;

            FormattableString message = $"{Message.AddSeparator()}Число {Checker.ActualValue} является чётным";
            throw new AssertFailedException(message.ToString(CultureInfo.InvariantCulture))
                .AddData("Actual", Checker.ActualValue);
        }
    }
}