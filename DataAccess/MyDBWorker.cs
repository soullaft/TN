using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess
{
    /// <summary>
    /// Класс, помогающий работать с базой данных MySQL
    /// </summary>
    public class MyDBWorker
    {

        protected static String Connection { get; private set; }

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public MyDBWorker(String connectionString)
        {
            Connection = connectionString;
        }

        /// <summary>
        /// Получить данные из таблицы
        /// </summary>
        /// <param name="query">SQL Запрос для базы данных </param>
        public IEnumerable<DataRow> GetTableValues(string query)
        {
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, Connection))
            {
                //Создаем  таблицу для хранения данных
                DataTable data = new DataTable();

                //Заполняем таблицу данными
                adapter.Fill(data);

                //Возвращаем заполненную коллекцию
                return data.AsEnumerable();
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
    }
}
