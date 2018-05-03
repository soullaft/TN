using System;
using System.Windows.Media.Imaging;

namespace TR.Data
{
    public class Employee
    {
        public Int64 ID { get; set; }

        public String FIO { get; set; }

        public string Email { get; set; }

        public Int64? Room { get; set; }

        public string Login { get; set; }

        public string Phone { get; set; }

        public Roles Type { get; set; }

        public string Password { get; set; }

        public BitmapImage Image { get; set; }

        /// <summary>
        /// Обновляет данные о пользователе
        /// </summary>
        /// <param name="updatedUser">Новые данные о пользователе</param>
        public virtual void Update(Employee updatedUser)
        {
            FIO = updatedUser.FIO ?? FIO;
            Room = updatedUser.Room ?? Room;
            Email = updatedUser.Email ?? Email;
            Login = updatedUser.Login ?? Login;
            Password = updatedUser.Password ?? Password;
            Phone = updatedUser.Phone ?? Phone;
        }
    }
}
