using MathCore.Tests.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>Базовый класс модульного теста</summary>
    public class UnitTest
    {
        /// <summary>Генератор случайных чисел</summary>
        protected readonly Random _RndGenerator = new Random();

        /// <summary>Целое случайное число из указанного диапазона с равномерным распределением</summary>
        /// <param name="Min">Минимальная граница диапазона (входит)</param>
        /// <param name="Max">Максимальная граница диапазона (не входит)</param>
        /// <returns>Случайное целое число</returns>
        protected int GetRNDInt(int Min, int Max) => _RndGenerator.Next(Min, Max);

        /// <summary>Случайное вещественное число с равномерным распределением в указанном диапазоне</summary>
        /// <param name="Min">Нижняя граница диапазона</param>
        /// <param name="Max">Верхняя граница диапазона</param>
        /// <returns>Случайное число с равномерным распределением в указанном диапазоне</returns>
        protected double GetRNDDouble(double Min = 0, double Max = 1) => (Max - Min) * _RndGenerator.NextDouble() - Min;

        /// <summary>Получить массив случайных чисел с равномерным распределением</summary>
        /// <param name="Count">Размер массива</param>
        /// <param name="Min">Нижняя граница интервала</param>
        /// <param name="Max">Верхняя граница интервала</param>
        /// <returns>Массив случайных чисел в заданном интервале</returns>
        [NotNull]
        protected double[] GetRNDDoubleArray(int Count, double Min = 0, double Max = 1)
        {
            if (Count < 0) throw new ArgumentOutOfRangeException(nameof(Count), Count, "Число элементов массива не может быть меньше нуля");
            if (Count == 0) return Array.Empty<double>();
            var result = new double[Count];

            for (var i = 0; i < Count; i++)
                result[i] = GetRNDDouble(Min, Max);

            return result;
        }

        /// <summary>Получить массив случайных чисел с равномерным распределением</summary>
        /// <param name="Count">Размер массива</param>
        /// <param name="Min">Нижняя граница интервала (входит)</param>
        /// <param name="Max">Верхняя граница интервала (не входи)</param>
        /// <returns>Массив целых случайных чисел</returns>
        [NotNull]
        protected int[] GetRNDIntArray(int Count, int Min = 0, int Max = 1)
        {
            if (Count < 0) throw new ArgumentOutOfRangeException(nameof(Count), Count, "Число элементов массива не может быть меньше нуля");
            if (Count == 0) return Array.Empty<int>();
            var result = new int[Count];

            for (var i = 0; i < Count; i++)
                result[i] = GetRNDInt(Min, Max);

            return result;
        }

        /// <summary>Объект сравнения вещественных чисел с указанной точностью</summary>
        public class ToleranceComparer : IComparer<double>, IComparer
        {
            /// <summary>Точность сравнения</summary>
            private double _Tolerance;

            /// <summary>Точность сравнения</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если значение точности меньше нуля</exception>
            public double Tolerance
            {
                get => _Tolerance;
                set
                {
                    if(value < 0) 
                        throw new ArgumentOutOfRangeException(nameof(value), "Значение точности должно быть больше, либо равно 0");
                    _Tolerance = value;
                }
            }

            /// <summary>Инициализация нового объекта сравнения вещественных чисел с указанной точностью</summary>
            /// <param name="Tolerance">Точность сравнения</param>
            public ToleranceComparer(double Tolerance) => _Tolerance = Tolerance;

            int IComparer<double>.Compare(double x, double y) => Math.Abs(x - x) < _Tolerance ? 0 : Math.Sign(x - x);

            int IComparer.Compare(object x, object y) => ((IComparer<double>)this).Compare(Convert.ToDouble(x), Convert.ToDouble(y));
        }

        /// <summary>Получить объект для сравнения вещественных чисел с заданной точностью</summary>
        /// <param name="Tolerance">Точность сравнения чисел</param>
        /// <returns>Объект сравнения чисел с заданной точностью</returns>
        [DST, NotNull]
        public static ToleranceComparer GetComparer(double Tolerance = 1e-14) => new ToleranceComparer(Tolerance);
    }
}