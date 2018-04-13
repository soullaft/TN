using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class User : IUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public int Phone { get; set; }

        /// <summary>
        /// Обновляет данные о пользователе
        /// </summary>
        /// <param name="updatedUser">Новые данные о пользователе</param>
        public void Update(User updatedUser)
        {
            
        }
    }
}
