using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using TR.Data;
using TR.MainData;

namespace TR.Request
{
    /// <summary>
    /// Логика заявок
    /// </summary>
    class RequestsService
    {

        #region Public Fields
        /// <summary>
        /// Заявки находящиеся в ожидании
        /// </summary>
        public static ObservableCollection<Request> WaitRequestsCollection { get; private set; }

        public static ObservableCollection<Request> AcceptRequestsCollection { get; private set; }

        public static ObservableCollection<Request> CanselRequestsCollection { get; private set; }

        #endregion

        #region Constuctor
        static RequestsService()
        {
            WaitRequestsCollection = new ObservableCollection<Request>();

            AcceptRequestsCollection = new ObservableCollection<Request>();

            CanselRequestsCollection = new ObservableCollection<Request>();

            GetRequests();
        }

        #endregion

        #region Work With DataBase

        /// <summary>
        /// Получаем все заявки
        /// </summary>
        public static void GetRequests()
        {
            Task.Factory.StartNew(() =>
            {

                WaitRequestsCollection.Clear();

                var query = "SELECT * FROM `Requests`";

                using (var connection = new MySqlConnection(ConnectionDB.Connection))
                {
                    var cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return;

                        while (reader.Read())
                        {
                            var state = (State)reader.GetInt64("State");

                            Request request = new Request()
                            {
                                ID = reader.GetInt64("ID"),
                                EmployeeID = reader.GetInt64("IDEmpl"),
                                Text = reader.GetString("Text"),
                                DateTime = reader.GetDateTime("Date").ToString(),
                                State = state,
                                FIOUser = EmployeeService.UsersCollection.Where(x => x.ID == reader.GetInt64("IDEmpl")).First().FIO
                            };
                            if (state == State.Wait)
                                Dispatcher.CurrentDispatcher.Invoke(() => WaitRequestsCollection.Add(request));
                            else if (state == State.Accepted)
                                Dispatcher.CurrentDispatcher.Invoke(() => AcceptRequestsCollection.Add(request));
                            else
                                Dispatcher.CurrentDispatcher.Invoke(() => CanselRequestsCollection.Add(request));
                        }
                    }
                }


            });
        }

        public static void RefreshWaitingRequests()
        {
            WaitRequestsCollection.Clear();

            var query = "SELECT * FROM `Requests` WHERE State = 0";

            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                var cmd = new MySqlCommand(query, connection);

                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return;

                    while (reader.Read())
                    {
                        var state = (State)reader.GetInt64("State");

                        Request request = new Request()
                        {
                            ID = reader.GetInt64("ID"),
                            EmployeeID = reader.GetInt64("IDEmpl"),
                            Text = reader.GetString("Text"),
                            DateTime = reader.GetDateTime("Date").ToString(),
                            State = state,
                            FIOUser = EmployeeService.UsersCollection.Where(x => x.ID == reader.GetInt64("IDEmpl")).First().FIO
                        };
                            Dispatcher.CurrentDispatcher.Invoke(() => WaitRequestsCollection.Add(request));
                    }
                }
            }
        }

        /// <summary>
        /// Изменяем состояние заявки
        /// </summary>
        /// <param name="request"></param>
        /// <param name="state"></param>
        public static void ChangeRequestsStatus(Request request, State state)
        {
            if (request == null)
                return;

            var query = $"UPDATE Requests SET State = {(long)state} WHERE ID = {request.ID}";

            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                var cmd = new MySqlCommand(query, connection);

                Task.Run(() =>
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                });

                if (state == State.Accepted)
                {
                    WaitRequestsCollection.Remove(GetRequest(request.ID));
                    AcceptRequestsCollection.Add(request);
                }
                else
                {
                    WaitRequestsCollection.Remove(GetRequest(request.ID));
                    CanselRequestsCollection.Add(request);
                }

            }
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Получаем заявку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Request GetRequest(long id) => WaitRequestsCollection.Where(request => request.ID == id).First();
        #endregion
    }
}