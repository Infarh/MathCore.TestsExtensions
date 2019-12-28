using System;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки равенства чисел с заданной точностью</summary>
    public sealed class DoubleEqualityCheckerWithAccuracy : IDisposable
    {
        /// <summary>Проверяемое значение</summary>
        private readonly double _ActualValue;
        /// <summary>Значение, с которым требуется провести сравнение</summary>
        private readonly double _ExpectedValue;
        /// <summary>Проверка на неравенство</summary>
        private readonly bool _Not;

        /// <summary>Проверка выполнена</summary>
        private bool _IsChecked;

        /// <summary>Инициализация нового объекта сравнения чисел с заданной точностью</summary>
        /// <param name="ActualValue">Проверяемое значение</param>
        /// <param name="ExpectedValue">Требуемое значение</param>
        /// <param name="Not">Проверять на неравенство</param>
        internal DoubleEqualityCheckerWithAccuracy(double ActualValue, double ExpectedValue, bool Not = false)
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
}