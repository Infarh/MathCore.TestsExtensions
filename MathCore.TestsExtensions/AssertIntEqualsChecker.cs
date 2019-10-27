using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки равенства/неравенства целых чисел</summary>
    public  sealed class AssertIntEqualsChecker
    {
        /// <summary>Проверяемое значение</summary>
        private readonly int _ActualValue;

        /// <summary>Инициализация нового объекта вроверки целого числа</summary>
        /// <param name="ExpectedValue">Проверяемое целое число</param>
        internal AssertIntEqualsChecker(int ExpectedValue) => _ActualValue = ExpectedValue;

        /// <summary>Проверка, что проверяемое значение равно ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(int ExpectedValue, string Message = null) => 
            Assert.AreEqual(ExpectedValue, _ActualValue,
                "{0}error:{1}", 
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue));

        /// <summary>Проверка, что проверяемое значение равно ожидаемому с заданной точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(int ExpectedValue, double Accuracy, string Message = null) => 
            Assert.AreEqual(ExpectedValue, _ActualValue, Accuracy,
                "{0}error:{1}, eps:{2}", 
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue), Accuracy);

        /// <summary>Проверка, что проверяемое значение не равно ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNotEqual(int ExpectedValue, string Message = null) => 
            Assert.AreNotEqual(ExpectedValue, _ActualValue,
                "{0}error:{1}",
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue));

        /// <summary>Проверка, что проверяемое значение не равно ожидаемому с заданной точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNotEqual(int ExpectedValue, double Accuracy, string Message = null) =>
            Assert.AreNotEqual(ExpectedValue, _ActualValue, Accuracy,
                "{0}error:{1}, eps:{2}",
                Message.AddSeparator(), Math.Abs(ExpectedValue - _ActualValue), Accuracy);

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue > ExpectedValue,
                "{0}Значение {1} должно быть больше {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterOrEqualsThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue >= ExpectedValue,
                "{0}Нарушено условие ({1} >= {2}). delta:{3:e2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue, ExpectedValue - _ActualValue);

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterOrEqualsThan(int ExpectedValue, int Accuracy, string Message = null) =>
            Assert.IsTrue(_ActualValue - ExpectedValue <= Accuracy,
                "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue, Accuracy, ExpectedValue - _ActualValue);

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue < ExpectedValue,
                "{0}Значение {1} должно быть меньше {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessOrEqualsThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(_ActualValue < ExpectedValue,
                "{0}Значение {1} должно быть меньше {2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessOrEqualsThan(int ExpectedValue, int Accuracy, string Message = null) =>
            Assert.IsTrue(ExpectedValue - _ActualValue <= Accuracy,
                "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}",
                Message.AddSeparator(), _ActualValue, ExpectedValue, Accuracy, ExpectedValue - _ActualValue);
    }
}