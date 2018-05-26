using System;

namespace TR
{
    /// <summary>
    /// Класс для генерирования случайных чисел
    /// </summary>
    public static class Randomizer
    {
        /// <summary>
        /// Возвращает случайно сгенерированное 4х значное число
        /// </summary>
        /// <param name="min">от</param>
        /// <param name="max">до</param>
        /// <returns></returns>
        public static String GetNumber(Int32 min, Int32 max)
        {
            return new Random().Next(min, max).ToString("D4");
        }
    }
}
