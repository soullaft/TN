using System;
using System.Windows.Media.Imaging;

namespace TR.Data
{
    public class Employee
    {
        public Int64 ID { get; set; }
        public String FIO { get; set; }

        public int? Age { get; set; }

        public string Email { get; set; }

        public Int64? Room { get; set; }

        public string Login { get; set; }

        public string Phone { get; set; }

        public BitmapImage Image { get; set; }

        /// <summary>
        /// Обновляет данные о пользователе
        /// </summary>
        /// <param name="updatedUser">Новые данные о пользователе</param>
        public void Update(Employee updatedUser)
        {
            FIO = updatedUser.FIO ?? FIO;
            Age = updatedUser.Age ?? Age;
            Room = updatedUser.Room ?? Room;
            Email = updatedUser.Email ?? Email;
            Login = updatedUser.Login ?? Login;
            Phone = updatedUser.Phone ?? Phone;
        }
    }
}
