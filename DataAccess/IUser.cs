using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Представляет обычного пользователя
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Имя
        /// </summary>
        String Name { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        String Surname { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        String Middlename { get; set; }
        /// <summary>
        /// Возраст
        /// </summary>
        Int32 Age { get; set; }
        /// <summary>
        /// Почтовый ящик
        /// </summary>
        String Email { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        String Login { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        Int32 Phone { get; set; }

        void Update(User updatedUser);
    }
}
