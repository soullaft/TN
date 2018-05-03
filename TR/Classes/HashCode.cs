using System;
using System.Text;

namespace TR.Classes
{
    /// <summary>
    /// Хэш-код
    /// </summary>
    public static class HashCode
    {
        /// <summary>
        /// Генерирует хэшкод
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GenerateHash(string text)
        {
            var password = Encoding.UTF8.GetBytes(text);

            var data = System.Security.Cryptography.MD5.Create().ComputeHash(password);

            return Convert.ToBase64String(data);
        }
    }
}
