using System;
using System.Collections.ObjectModel;
using TR.Properties;
using System.Linq;
using MySql.Data.MySqlClient;

namespace TR.Data
{
    public class EmployeeService
    {
        protected static MySqlHelper _mySqlHelper;

        /// <summary>
        /// Строка подключения
        /// </summary>
        public static String CONNECTION_STRING;

        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        public static ObservableCollection<Employee> UsersCollection { get; private set; }
        /// <summary>
        /// Конструктор типа
        /// </summary>
        static EmployeeService()
        {
            _mySqlHelper = new MySqlHelper();

            UsersCollection = _mySqlHelper.GetEmployersTable();
        }
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="newUser">Новый пользователь</param>
        public void AddUser(Employee newUser)
        {
            UsersCollection.Add(newUser);

            String query = "INSERT INTO Employers (RNumber,FIO,Phone,Email,Login,Password,Image) values (@Room,@FIO,@Phone,@Email,@Login,@Password,@Image)";
            using (var connection = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);


            }
        }

    }
}
