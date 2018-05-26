using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TR.MainData;
using TR.Pages;

namespace TR.Notification.Controls
{
    /// <summary>
    /// Interaction logic for StandardNotification.xaml
    /// </summary>
    public partial class StandardNotification : UserControl
    {
        private NotificationModel model;

        public StandardNotification(NotificationModel model)
        {
            InitializeComponent();

            textBox.Text = model.Text;
            this.model = model;
        }
        /// <summary>
        /// Удалить уведомление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRequest_Click(object sender, MouseButtonEventArgs e)
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                var query = $"DELETE FROM Notifications WHERE ID = {model.ID}";

                Task.Run(() =>
                {
                    connection.Open();

                    new MySqlCommand(query, connection).ExecuteNonQuery();
                });
            }

            AnimationHelper.StartAnimation(this, "OnMouseLeftButtonDown1", (x, y) =>
            {
                (Application.Current.MainWindow as MenuWindow).kek.Children.Remove(this);
                NotificationService.StandardNotificationsCollection.Remove(this);
            });


        }

        /// <summary>
        /// Перейти на страницу с заявками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowRequest_Click(object sender, MouseButtonEventArgs e)
        {
            (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new RequestsPage();

            AnimationHelper.StartAnimation((Application.Current.MainWindow as MenuWindow), "hideNotifications", delegate 
            {
                (Application.Current.MainWindow as MenuWindow).mainFrame.Effect = null;
                (Application.Current.MainWindow as MenuWindow).kek.Children.Remove(this);
            });
        }
    }
}
