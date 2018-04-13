using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR
{
    /// <summary>
    /// Класс для работы с кодированием данных
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Возвращает ХЭШ
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static String GenerateHash(String text)
        {
            var password = Encoding.UTF8.GetBytes(text);

            var data = System.Security.Cryptography.MD5.Create().ComputeHash(password);

            return Convert.ToBase64String(data);
        }
    }
}
