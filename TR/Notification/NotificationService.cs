using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TR.Data;
using TR.MainData;
using TR.Notification.Controls;

namespace TR.Notification
{
    /// <summary>
    /// Реализует логику отправки уведомлений
    /// </summary>
    public class NotificationService
    {
        #region Закрытые поля
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        private static string connectionString;

        /// <summary>
        /// Таймер
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// Лист уведомлений
        /// </summary>
        private static ObservableCollection<NotificationModel> notificationList = new ObservableCollection<NotificationModel>();
        #endregion

        #region Публичные поля(свойства)
        /// <summary>
        /// Коллекция уведомления для привязки
        /// </summary>
        public static ObservableCollection<StandardNotification> StandardNotificationsCollection = new ObservableCollection<StandardNotification>();

        public static event EventHandler NewNotification = delegate { };
        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор типа
        /// </summary>
        static NotificationService()
        {
            connectionString = ConnectionDB.Connection;
        }

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        /// <param name="target">Список заявок, которые будут вывены. 0 - если администатор ID cотрудника, если стоит получать уведомления только для cотрудника</param>
        public NotificationService(int target)
        {
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(10);

            timer.Tick += delegate
            {
                Task.Run(() => { GetNotifications(target); });
            };

            timer.Start();
        }

        #endregion

        #region Main Logic
        /// <summary>
        /// Получает все заявки из базы данных
        /// </summary>
        /// <param name="target">Список заявок, которые будут вывены. 0 - если администатор ID cотрудника, если стоит получать уведомления только для cотрудника</param>
        private void GetNotifications(int target)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                //Берем все данные из таблицы 'Notifications'
                string query = $"SELECT Notifications.ID, Notifications.Text, Notifications.Target, Notifications.State, Employers.FIO FROM Notifications INNER JOIN Employers ON Notifications.IDEmployee = Employers.ID WHERE Notifications.Target = {target}";

                var cmd = new MySqlCommand(query, connection);

                //Открываем подключение
                connection.Open();

                //Выполняем запрос к базе данных
                using (var reader = cmd.ExecuteReader())
                {
                    //Если в базе есть уведомления
                    if (reader.HasRows)
                    {
                        //Пробегаемся по каждому уведомлению
                        while (reader.Read())
                        {
                            var id = reader.GetInt64("ID");
                            //Если данного уведомления нет в коллекции, то добавляем его
                            if (!IsExisted(id))
                            {
                                var notification = new NotificationModel()
                                {
                                    ID = id,
                                    Text = reader.GetString("Text"),
                                };
                                notificationList.Add(notification);

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    (Application.Current.MainWindow as MenuWindow).kek.Children.Add(new StandardNotification(notification));
                                    AnimationHelper.StartAnimation((Application.Current.MainWindow as MenuWindow), "Ding", delegate { });
                                    //ContexTrayMenu.Show("Новая заявка", $"{reader.GetString("FIO")} отправил заявку", System.Windows.Forms.ToolTipIcon.Info);
                                });
                                break;
                            }
                        }
                    }
                    else
                        return;
                }
            }
        }

        public static void AddNotification(Request.Request request, string text)
        {
            if (request == null)
                return;

            var notificatationQuery = $"INSERT INTO Notifications(IDEmployee, IDType, Text, Target) VALUES ({request.EmployeeID}, 1, '{text}', {request.EmployeeID})";

            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                connection.Open();
                new MySqlCommand(notificatationQuery, connection).ExecuteNonQuery();
                connection.Close();
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Проверяет является ли уведомление в списке уведомлений, если нет, то добавляет его туда
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExisted(long id)
        {
            if (notificationList.Count(noti => noti.ID == id) != 0)
                return true;
            return false;
        }

        #endregion
    }
}
