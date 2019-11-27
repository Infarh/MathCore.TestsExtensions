using System;
using MathCore.Tests.Annotations;
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки равенства/неравенства чисел с плавающей запятой двойной точности</summary>
    public sealed class AssertDoubleEqualsChecker
    {
        /// <summary>Объект проверки равенства чисел с заданной точностью</summary>
        public sealed class EqualityCheckerWithAccuracy : IDisposable
        {
            /// <summary>Проверяемое значение</summary>
            private readonly double _ActualValue;
            /// <summary>Значение, с которым требуется провести сравнение</summary>
            private readonly double _ExpectedValue;
            /// <summary>Проверка на неравенство</summary>
            private readonly bool _Not;

            private bool _IsChecked;

            /// <summary>Инициализация нового объекта сравнения чисел с заданной точностью</summary>
            /// <param name="ActualValue">Проверяемое значение</param>
            /// <param name="ExpectedValue">Требуемое значение</param>
            /// <param name="Not">Проверять на неравенство</param>
            internal EqualityCheckerWithAccuracy(double ActualValue, double ExpectedValue, bool Not = false)
            {
                _ActualValue = ActualValue;
                _ExpectedValue = ExpectedValue;
                _Not = Not;
            }

            /// <summary>Проверка с задаваемой точностью</summary>
            /// <param name="Accuracy">Точность сравнения</param>
            /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
            public void WithAccuracy(double Accuracy, string Message = null)
            {
                _IsChecked = true;
                if (_Not)
                    Assert.AreNotEqual(_ExpectedValue, _ActualValue, Accuracy, "{0}error:{1:e2}", Message.AddSeparator(), Math.Abs(_ExpectedValue - _ActualValue));
                else
                    Assert.AreEqual(_ExpectedValue, _ActualValue, Accuracy, "{0}error:{1:e2}", Message.AddSeparator(), Math.Abs(_ExpectedValue - _ActualValue));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Ожидание>")]
            void IDisposable.Dispose()
            {
                if (_IsChecked) return;
                Assert.Fail($"Проверка на {(_Not ? "не" : null)}равенство не выполнена");
            }
        }

        /// <summary>Объект сравнения чисел с заданной точностью</summary>
        public sealed class CompareCheckerWithAccuracy : IDisposable
        {
            /// <summary>Проверяемое значение</summary>
            private readonly double _ActualValue;
            /// <summary>Ожидаемое значение</summary>
            private readonly double _ExpectedValue;
            /// <summary>Проверка на равенство</summary>
            private readonly bool _IsEquals;
            /// <summary>Проверка - меньше</summary>
            private readonly bool _IsLessChecking;

            /// <summary>Проверка выполнена</summary>
            private bool _IsChecked;

            /// <summary>Инициализация объекта сравнения</summary>
            /// <param name="ActualValue">Проверяемое значение</param>
            /// <param name="ExpectedValue">Ожидаемое значение</param>
            /// <param name="IsEquals">Проверять на равенство</param>
            /// <param name="IsLessChecking">Проверка - на "меньше"</param>
            internal CompareCheckerWithAccuracy(double ActualValue, double ExpectedValue, bool IsEquals = true, bool IsLessChecking = false)
            {
                _ActualValue = ActualValue;
                _ExpectedValue = ExpectedValue;
                _IsEquals = IsEquals;
                _IsLessChecking = IsLessChecking;
            }

            /// <summary>Срвенение с задаваеомой точностью</summary>
            /// <param name="Accuracy">Точность сравнения</param>
            /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
            public void WithAccuracy(double Accuracy, string Message = null)
            {
                _IsChecked = true;
                if (_IsLessChecking)
                    if (_IsEquals)
                        Assert.IsTrue(_ExpectedValue - _ActualValue <= Accuracy,
                            "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}", 
                            Message.AddSeparator(), _ActualValue, _ExpectedValue, Accuracy, _ExpectedValue - _ActualValue);
                    else
                        Assert.IsTrue(_ExpectedValue - _ActualValue < Accuracy,
                            "{0}Нарушено условие ({1} > {2}) при точности {3:e2} delta:{4:e2}",
                            Message.AddSeparator(), _ActualValue, _ExpectedValue, Accuracy, _ExpectedValue - _ActualValue);

                else if (_IsEquals)
                    Assert.IsTrue(_ActualValue - _ExpectedValue <= Accuracy,
                        "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}", 
                        Message.AddSeparator(), _ActualValue, _ExpectedValue, Accuracy, _ExpectedValue - _ActualValue);
                else
                    Assert.IsTrue(_ActualValue - _ExpectedValue < Accuracy,
                        "{0}Нарушено условие ({1} > {2}) при точности {3:e2} delta:{4:e2}", 
                        Message.AddSeparator(), _ActualValue, _ExpectedValue, Accuracy, _ExpectedValue - _ActualValue);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Ожидание>")]
            void IDisposable.Dispose()
            {
                if (_IsChecked) return;
                Assert.Fail("Проверка значения на выполнена");
            }
        }

        /// <summary>Сравниваемое значение</summary>
        private readonly double _ActualValue;

        /// <summary>Инициализация нового объекта сравнения чисел с плавающей запятой</summary>
        /// <param name="ExpectedValue">Проверяемое значение</param>
        internal AssertDoubleEqualsChecker(double ExpectedValue) => _ActualValue = ExpectedValue;

        /// <summary>Проверка значения на равенство</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public EqualityCheckerWithAccuracy IsEqualTo(double ExpectedValue) => new EqualityCheckerWithAccuracy(_ActualValue, ExpectedValue);

        /// <summary>Проверка значения на неравенство</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public EqualityCheckerWithAccuracy IsNotEqualTo(double ExpectedValue) => new EqualityCheckerWithAccuracy(_ActualValue, ExpectedValue, true);

        /// <summary>Сравнение с ожидаемым значением</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsEqual(double ExpectedValue, string Message = null) =>
            Assert.AreEqual(ExpectedValue, _ActualValue,
                "{0}error:{1:e2}",
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue));

        /// <summary>Сравнение с ожидаемым значением с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsEqual(double ExpectedValue, double Accuracy, string Message = null) =>
            Assert.AreEqual(ExpectedValue, _ActualValue, Accuracy,
                "{0}error:{1:e2}", 
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue));

        /// <summary>Проверка на неравенство</summary>
        /// <param name="ExpectedValue">Неожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsNotEqual(double ExpectedValue, string Message = null) =>
            Assert.AreNotEqual(ExpectedValue, _ActualValue,
                "{0}error:{1:e2}", 
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue));

        /// <summary>Проверка на неравенство</summary>
        /// <param name="ExpectedValue">Неожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void IsNotEqual(double ExpectedValue, double Accuracy, string Message = null) =>
            Assert.AreNotEqual(
                ExpectedValue, _ActualValue, Accuracy,
                "{0}error:{1:e2}",
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue));

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterThan(double ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue > ExpectedValue,
                "{0}Значение {1} должно быть больше {2}", 
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterOrEqualsThan(double ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue >= ExpectedValue,
                "{0}Нарушено условие ({1} >= {2}). delta:{3:e2}", 
                Message.AddSeparator(), _ActualValue, ExpectedValue, ExpectedValue - _ActualValue);

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterOrEqualsThan(double ExpectedValue, double Accuracy, string Message = null) =>
            Assert.IsTrue(_ActualValue - ExpectedValue <= Accuracy,
                "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}", 
                Message.AddSeparator(), _ActualValue, ExpectedValue, Accuracy, ExpectedValue - _ActualValue);

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessThan(double ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue < ExpectedValue,
                "{0}Значение {1} должно быть меньше {2}", 
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessOrEqualsThan(double ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue < ExpectedValue,
                "{0}Значение {1} должно быть меньше {2}", 
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessOrEqualsThan(double ExpectedValue, double Accuracy, string Message = null) =>
            Assert.IsTrue(ExpectedValue - _ActualValue <= Accuracy,
                "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}", 
                Message.AddSeparator(), _ActualValue, ExpectedValue, Accuracy, ExpectedValue - _ActualValue);

        /// <summary>Значение больше (строго), чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public CompareCheckerWithAccuracy Greater(double ExpectedValue) =>
            new CompareCheckerWithAccuracy(_ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: false);

        /// <summary>Значение больше, чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public CompareCheckerWithAccuracy GreaterOrEqual(double ExpectedValue) =>
            new CompareCheckerWithAccuracy(_ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: false);

        /// <summary>Значение меньше (строго), чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public CompareCheckerWithAccuracy Less(double ExpectedValue) =>
            new CompareCheckerWithAccuracy(_ActualValue, ExpectedValue, IsEquals: false, IsLessChecking: true);

        /// <summary>Значение меньше, чем указанное с задаваемой точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <returns>Объект сравнения с задаваемой точностью</returns>
        public CompareCheckerWithAccuracy LessOrEqual(double ExpectedValue) =>
            new CompareCheckerWithAccuracy(_ActualValue, ExpectedValue, IsEquals: true, IsLessChecking: true);

        /// <summary>Проверить, что значение не является не-числом</summary>
        /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
        public void IsNotNaN(string Message = null) => Assert.IsFalse(double.IsNaN(_ActualValue), Message);

        /// <summary>Проверить, что значение является не-числом</summary>
        /// <param name="Message">Сообщение, выводимое в случае если проверка провалена</param>
        public void IsNaN(string Message = null) => Assert.IsTrue(double.IsNaN(_ActualValue), Message);
    }
}