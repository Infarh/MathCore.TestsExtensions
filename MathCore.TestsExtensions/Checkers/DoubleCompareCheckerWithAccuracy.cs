using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект сравнения чисел с заданной точностью</summary>
    public sealed class DoubleCompareCheckerWithAccuracy : IDisposable
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
        internal DoubleCompareCheckerWithAccuracy(double ActualValue, double ExpectedValue, bool IsEquals = true, bool IsLessChecking = false)
        {
            _ActualValue = ActualValue;
            _ExpectedValue = ExpectedValue;
            _IsEquals = IsEquals;
            _IsLessChecking = IsLessChecking;
        }

        /// <summary>Сравнение с задаваемой точностью</summary>
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
            Assert.Fail("Проверка значения не выполнена");
        }
    }
}