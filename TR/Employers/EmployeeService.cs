using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TR.MainData;
using TR.Notification;

namespace TR.Data
{
    /// <summary>
    /// Содержит данные и методы над пользователями
    /// </summary>
    public class EmployeeService
    {
        #region Public Fields
        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        public static ObservableCollection<Employee> UsersCollection { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор типа
        /// </summary>
        static EmployeeService()
        {
            UsersCollection = new ObservableCollection<Employee>();
        }

        #endregion

        #region Work With DataBase
        /// <summary>
        /// Вовзращает данные из таблицы 'Employers'
        /// </summary>
        private static ObservableCollection<Employee> GetEmployersTable()
        {
            ObservableCollection<Employee> users = new ObservableCollection<Employee>();

            String query = "SELECT * FROM Employers";

            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                var command = new MySqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Employee user = new Employee()
                            {
                                ID = reader.GetInt64("ID"),
                                FIO = reader.GetString("FIO"),
                                Phone = reader.GetString("Phone"),
                                Room = reader.GetInt64("RNumber"),
                                Login = reader.GetString("Login"),
                                Type = (Roles)reader.GetInt64("Role"),
                                Password = reader.GetString("Password"),
                                Email = reader.GetString("Email"),
                                Image = ConvertImage(reader["Image"]),
                            };

                            switch (reader.GetInt64("Online"))
                            {
                                case 1:
                                    user.Status = "В сети";
                                    break;
                                case 2:
                                    user.Status = "Не в сети";
                                    break;
                            }

                            users.Add(user);
                        }

                        return users;

                    }
                    return new ObservableCollection<Employee>();
                }
            }
        }

        /// <summary>
        /// Асинхронное заполнение пользователей
        /// </summary>
        /// <returns></returns>
        public async static Task<bool> FillEmployeersAsync()
        {
            var result = await Task.Factory.StartNew(() => GetEmployersTable());

            UsersCollection.Clear();

            UsersCollection = result;

            GetMaxID();

            return true;
        }
        
        /// <summary>
        /// Добавить сотрудника
        /// </summary>
        /// <param name="employee">Cотрудник</param>
        /// <param name="imageType">Тип изображения</param>
        /// <param name="source">Источник изображения</param>
        public static void AddUser(Employee employee, string imageType = "", ImageSource source = null)
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                string query = "INSERT INTO Employers (RNumber,FIO,Phone,Email,Login,Password,Image, Role, Online) values (@Room,@FIO,@Phone,@Email,@Login,@Password,@Image, @Role, 2)";

                var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.Add("@Role", MySqlDbType.Int64);
                cmd.Parameters["@Role"].Value = Convert.ToInt64(employee.Type);

                cmd.Parameters.Add("@FIO", MySqlDbType.VarChar);
                cmd.Parameters["@FIO"].Value = employee.FIO;

                cmd.Parameters.Add("@Room", MySqlDbType.Int64);
                cmd.Parameters["@Room"].Value = employee.Room;

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar);
                cmd.Parameters["@Email"].Value = employee.Email;

                cmd.Parameters.Add("@Phone", MySqlDbType.Int64);
                cmd.Parameters["@Phone"].Value = employee.Phone;

                cmd.Parameters.Add("@Login", MySqlDbType.VarChar);
                cmd.Parameters["@Login"].Value = employee.Login;

                cmd.Parameters.Add("@Password", MySqlDbType.VarChar);
                cmd.Parameters["@Password"].Value = employee.Password.Trim();

                if (employee.Image == null)
                    cmd.Parameters.Add("@Image", MySqlDbType.LongBlob);
                else
                {
                    cmd.Parameters.Add("@Image", MySqlDbType.LongBlob);

                    if (imageType == "png")
                        cmd.Parameters["@Image"].Value = ImageConverter.ConvertImage(new PngBitmapEncoder(), source);
                    else if (imageType == "jpg" || imageType == "jpeg")
                        cmd.Parameters["@Image"].Value = ImageConverter.ConvertImage(new JpegBitmapEncoder(), source);
                }

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// Добавить сотрудника асинхронно
        /// </summary>
        /// <param name="employee">Cотрудник</param>
        /// <param name="imageType">Тип изображения</param>
        /// <param name="source">Источник изображения</param>
        public async static void AddUserAsync(Employee employee, string imageType = null, ImageSource source = null)
        {
            await Task.Factory.StartNew(() => AddUser(employee, imageType, source));

            employee.ID = await Task.Factory.StartNew(() => GetMaxID());

            UsersCollection.Add(employee);

            ContexTrayMenu.ShowMessage("Уведомление!", "Пользователь был успешно добавлен", System.Windows.Forms.ToolTipIcon.Info);
        }

        /// <summary>
        /// Обновляет данные о пользователе
        /// </summary>
        /// <param name="employee"></param>
        public static void UpdateUser(Employee employee)
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                string query = $"UPDATE Employers SET RNumber = @Room,FIO = @FIO, Phone = @Phone, Email = @Email, Login = @Login Where ID = {employee.ID}";

                var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.Add("@FIO", MySqlDbType.VarChar);
                cmd.Parameters["@FIO"].Value = employee.FIO;

                cmd.Parameters.Add("@Room", MySqlDbType.Int64);
                cmd.Parameters["@Room"].Value = employee.Room;

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar);
                cmd.Parameters["@Email"].Value = employee.Email;

                cmd.Parameters.Add("@Phone", MySqlDbType.Int64);
                cmd.Parameters["@Phone"].Value = employee.Phone;

                cmd.Parameters.Add("@Login", MySqlDbType.VarChar);
                cmd.Parameters["@Login"].Value = employee.Login;

                UsersCollection[UsersCollection.IndexOf(UsersCollection.Where(user => user.ID == employee.ID).First())].Update(employee);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Обновляет данные о пользователе асинхронно
        /// </summary>
        /// <param name="employee"></param>
        public async static void UpdateUserAsync(Employee employee)
        {
            await Task.Factory.StartNew(() => UpdateUser(employee));
        }

        private static async void RefreshUserStatus()
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                await connection.OpenAsync();
                using (var reader = await new MySqlCommand("SELECT ID, Online FROM Employers", connection).ExecuteReaderAsync())
                {
                    while(reader.Read())
                    {
                        string str = "";
                        switch(reader.GetInt64(1))
                        {
                            case 1:
                                str = "В сети";
                                break;
                            case 2:
                                str = "Не в сети";
                                break;
                        }
                        UsersCollection.Where(user => user.ID == reader.GetInt64(0)).First().Update(new Employee() { Status = str});
                    }
                }
                await connection.CloseAsync();
            }
        }

        public static void DeleteUser(long id)
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                connection.Open();

                new MySqlCommand($"DELETE FROM Notifications WHERE IDEmployee ={id}", connection).ExecuteNonQuery();
                new MySqlCommand($"DELETE FROM Requests WHERE IDEmpl ={id}", connection).ExecuteNonQuery();
                new MySqlCommand($"DELETE FROM Employers WHERE ID ={id}", connection).ExecuteNonQuery();

                UsersCollection.Remove(UsersCollection.Where(x => x.ID == id).First());

                connection.Close();
            }
        }

        public static async void RefreshUsersStatusAsync()
        {
            await Task.Factory.StartNew(() => RefreshUserStatus());
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Возвращает последний(Максимальный) индекс пользователя
        /// </summary>
        /// <returns></returns>
        private static long GetMaxID()
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                connection.Open();
                var result = Convert.ToInt64(new MySqlCommand("SELECT MAX(ID) FROM Employers", connection).ExecuteScalar());
                connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Конвертирует массив байтов в BitmapImage
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static BitmapImage ConvertImage(Object array)
        {

            if (!(array is byte[] imageData) || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        /// <summary>
        /// Получаем пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Employee GetEmployee(long id) => UsersCollection.Where(user => user.ID == id).First();

        #endregion
    }
}
