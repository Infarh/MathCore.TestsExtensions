using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Объект проверки равенства/неравенства целых чисел</summary>
    public  sealed class IntValueChecker
    {
        /// <summary>Проверяемое значение</summary>
        public int ActualValue { get; }

        /// <summary>Продолжение (перезапуск) цепочки тестирования</summary>
        public Assert And => Assert.That;

        /// <summary>Инициализация нового объекта проверки целого числа</summary>
        /// <param name="ExpectedValue">Проверяемое целое число</param>
        internal IntValueChecker(int ExpectedValue) => ActualValue = ExpectedValue;

        /// <summary>Проверка, что проверяемое значение равно ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(int ExpectedValue, string Message = null) => 
            Assert.AreEqual(ExpectedValue, ActualValue,
                "{0}error:{1}", 
                Message.AddSeparator(), Math.Abs(ExpectedValue - ActualValue));

        /// <summary>Проверка, что проверяемое значение равно ожидаемому с заданной точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsEqual(int ExpectedValue, double Accuracy, string Message = null) => 
            Assert.AreEqual(ExpectedValue, ActualValue, Accuracy,
                "{0}error:{1}, eps:{2}", 
                Message.AddSeparator(), Math.Abs(ExpectedValue - ActualValue), Accuracy);

        /// <summary>Проверка, что проверяемое значение не равно ожидаемому</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNotEqual(int ExpectedValue, string Message = null) => 
            Assert.AreNotEqual(ExpectedValue, ActualValue,
                "{0}error:{1}",
                Message.AddSeparator(), Math.Abs(ExpectedValue - ActualValue));

        /// <summary>Проверка, что проверяемое значение не равно ожидаемому с заданной точностью</summary>
        /// <param name="ExpectedValue">Ожидаемое значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае неудачи</param>
        public void IsNotEqual(int ExpectedValue, double Accuracy, string Message = null) =>
            Assert.AreNotEqual(ExpectedValue, ActualValue, Accuracy,
                "{0}error:{1}, eps:{2}",
                Message.AddSeparator(), Math.Abs(ExpectedValue - ActualValue), Accuracy);

        /// <summary>Проверка, что значение больше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(ActualValue > ExpectedValue,
                "{0}Значение {1} должно быть больше {2}",
                Message.AddSeparator(), ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение больше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterOrEqualsThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(ActualValue >= ExpectedValue,
                "{0}Нарушено условие ({1} >= {2}). delta:{3:e2}",
                Message.AddSeparator(), ActualValue, ExpectedValue, ExpectedValue - ActualValue);

        /// <summary>Проверка, что значение больше, либо равно заданному с заданной точностью</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void GreaterOrEqualsThan(int ExpectedValue, int Accuracy, string Message = null) =>
            Assert.IsTrue(ActualValue - ExpectedValue <= Accuracy,
                "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}",
                Message.AddSeparator(), ActualValue, ExpectedValue, Accuracy, ExpectedValue - ActualValue);

        /// <summary>Проверка, что значение меньше заданного</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(ActualValue < ExpectedValue,
                "{0}Значение {1} должно быть меньше {2}",
                Message.AddSeparator(), ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessOrEqualsThan(int ExpectedValue, string Message = null) =>
            Assert.IsTrue(ActualValue < ExpectedValue,
                "{0}Значение {1} должно быть меньше {2}",
                Message.AddSeparator(), ActualValue, ExpectedValue);

        /// <summary>Проверка, что значение меньше, либо равно заданному</summary>
        /// <param name="ExpectedValue">Опорное значение</param>
        /// <param name="Accuracy">Точность сравнения</param>
        /// <param name="Message">Сообщение, выводимое в случае ошибки сравнения</param>
        public void LessOrEqualsThan(int ExpectedValue, int Accuracy, string Message = null) =>
            Assert.IsTrue(ExpectedValue - ActualValue <= Accuracy,
                "{0}Нарушено условие ({1} >= {2}) при точности {3:e2} delta:{4:e2}",
                Message.AddSeparator(), ActualValue, ExpectedValue, Accuracy, ExpectedValue - ActualValue);
    }
}