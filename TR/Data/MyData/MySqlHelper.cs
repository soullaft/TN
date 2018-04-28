using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace TR.Data
{
    /// <summary>
    /// Класс работающий с базой данных MySQL
    /// </summary>
    public class MySqlHelper
    {
        protected static String Connection { get; private set; }

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public MySqlHelper(String connectionString)
        {
            Connection = connectionString;
        }

        /// <summary>
        /// Вовзращает данные из таблицы 'Employers'
        /// </summary>
        public ObservableCollection<Employee> GetEmployersTable()
        {
            ObservableCollection<Employee> users = new ObservableCollection<Employee>();

            String query = "SELECT * FROM Employers";

            using (var connection = new MySqlConnection(Connection))
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
                                Email = reader.GetString("Email"),
                                Image = ConvertImage(reader["Image"]),
                            };

                            users.Add(user);
                        }
                        return users;
                    }
                    else
                        throw new ArgumentException("Table was empty");
                }
            }
        }


        /// <summary>
        /// Выполняет запросы к базе данных такие как : UPDATE, DELETE, INSERT
        /// </summary>
        /// <param name="query">Запрос для базы данных</param>
        public void QueryToDataBase(String query)
        {
            if (!query.ToUpper().Contains("UPDATE") && !query.ToUpper().Contains("DELETE") && !query.ToUpper().Contains("INSERT"))
                throw new FormatException("Неправильный запрос к базе данных");

            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                //Создаем SQL запрос
                MySqlCommand cmd = new MySqlCommand(query, con);

                //Открываем подключение
                con.Open();

                //Выполняем MySQL запрос
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Конвертирует массив байтов в BitmapImage
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private BitmapImage ConvertImage(Object array)
        {
            byte[] imageData = array as byte[];

            if (imageData == null || imageData.Length == 0) return null;
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
    }
}
