using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TR.Notification.Controls;
using TR.Properties;

namespace TR.Notification
{
    /// <summary>
    /// Реализует логику отправки уведомлений
    /// </summary>
    public class NotificationService
    {
        #region Закрытые события
        /// <summary>
        /// Событие, которое вызывается тогда когда появляется новое уведомление в базе данных
        /// </summary>
        private EventHandler<NotifyEventArgs> fireNotification = delegate { };

        #endregion

        #region Закрытые поля
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        private static string connectionString;

        /// <summary>
        /// Лист уведомлений
        /// </summary>
        private ObservableCollection<NotificationModel> notificationList = new ObservableCollection<NotificationModel>();
        #endregion

        #region Публичные поля(свойства)
        /// <summary>
        /// Коллекция уведомления для привязки
        /// </summary>
        public ObservableCollection<StandardNotification> StandardNotificationsCollection = new ObservableCollection<StandardNotification>();
        
        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор типа
        /// </summary>
        static NotificationService()
        {
            connectionString = $"server={Settings.Default.server};user={Settings.Default.user};database={Settings.Default.database};password={Settings.Default.password};";
        }

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public NotificationService()
        {
            Task.Run(() => { GetNotifications(); });
        }

        #endregion

        /// <summary>
        /// Получает все заявки из базы данных
        /// </summary>
        private void GetNotifications()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                //Строка запроса к базе данных
                string query = "SELECT * FROM Notifications";

                var cmd = new MySqlCommand(query, connection);

                //Открываем подключение
                connection.Open();

                //Выполняем запрос к базе данных
                using (var reader = cmd.ExecuteReader())
                {
                    //Если в базе есть уведомления
                    if (reader.HasRows)
                    {
                        if (notificationList.Count <= 0)
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
                                    };
                                    notificationList.Add(notification);
                                    //StandardNotificationsCollection.Add();
                                    Application.Current.Dispatcher.Invoke(() =>
                                   {
                                       (Application.Current.MainWindow as MenuWindow).kek.Children.Add(new StandardNotification(notification));
                                   });
                                }
                            }
                        }
                    }
                    else
                        return;
                }
            }
        }
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
    }
}
