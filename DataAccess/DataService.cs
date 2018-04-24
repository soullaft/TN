using System;
using System.Collections.Generic;
using DataAccess.Properties;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class DataService : IDataService
    {
        protected static MyDBWorker _dbWorker;

        /// <summary>
        /// Строка подключения
        /// </summary>
        protected readonly static String CONNECTION_STRING;

        private static List<User> users = new List<User>();

        /// <summary>
        /// Конструктор типа
        /// </summary>
        static DataService()
        {
            CONNECTION_STRING = $"server={Settings.Default.server};user={Settings.Default.user};database={Settings.Default.database};password={Settings.Default.password};charset={Settings.Default.charset};";
            _dbWorker = new MyDBWorker(CONNECTION_STRING);
        }

        public IList<User> UsersList()
        {
            //_dbWorker.GetTableValues("SELECT * FROM Employers");
            return new List<User>();
        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="newUser">Новый пользователь</param>
        public void AddUser(User newUser)
        {
            users.Add(newUser);
        }

        /// <summary>
        /// Добавляет пользователя в базу данных
        /// </summary>
        /// <param name="newUser">Новый пользователь</param>
        private void AddUserToDataBase(User newUser)
        {
            String query = "INSERT INTO Employers (RNumber,FIO,Phone,Email,Login,Password,Image) values (@Room,@FIO,@Phone,@Email,@Login,@Password,@Image)";
            using (var connection = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                
            }
        }
    }
}
