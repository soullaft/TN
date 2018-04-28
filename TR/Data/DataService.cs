using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TR.Data
{
    public class DataService
    {
        protected static MySqlHelper _sqlHelper;

        /// <summary>
        /// Строка подключения
        /// </summary>
        protected readonly static String CONNECTION_STRING;

        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        public static ObservableCollection<Employee> UsersCollection
        {
            get { return users; }
            private set { }
        }

        private static ObservableCollection<Employee> users = new ObservableCollection<Employee>();

        /// <summary>
        /// Конструктор типа
        /// </summary>
        static DataService()
        {
            //CONNECTION_STRING = $"server={Properties.Settings Settings.Default.server};user={Settings.Default.user};database={Settings.Default.database};password={Settings.Default.password};charset={Settings.Default.charset};";
            _sqlHelper = new MySqlHelper(CONNECTION_STRING);

            users = _sqlHelper.GetEmployersTable();
        }

        public IList<Employee> UsersList()
        {
            return new List<Employee>();
        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="newUser">Новый пользователь</param>
        public void AddUser(Employee newUser)
        {
            users.Add(newUser);

            String query = "INSERT INTO Employers (RNumber,FIO,Phone,Email,Login,Password,Image) values (@Room,@FIO,@Phone,@Email,@Login,@Password,@Image)";
            using (var connection = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);


            }
        }

        /// <summary>
        /// Возвращает работника
        /// </summary>
        /// <param name="id">Идентификатор работника</param>
        /// <returns></returns>
        public Employee GetEmployee(Int32 id)
        {
            return users.Where((x) => x.ID == id).FirstOrDefault();
        }
    }
}
