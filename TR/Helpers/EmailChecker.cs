using System;
using System.Net.Mail;

namespace TR
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailChecker
    {
        /// <summary>
        /// Проверяет является ли переданный в параметре почтовый ящик таковым
        /// </summary>
        /// <param name="email">почтовый ящик</param>
        /// <returns></returns>
        public static Boolean IsValidEmail(string email)
        {
            try
            {
                MailAddress addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
