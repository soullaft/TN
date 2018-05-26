using System.Windows.Controls;
using TR.Request;
using System.Windows;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using TR.MainData;
using TR.Data;
using TR.Notification;

namespace TR.Pages
{
    /// <summary>
    /// Interaction logic for RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        public RequestsPage()
        {
            InitializeComponent();

            datagrid.ItemsSource = null;
            datagrid.ItemsSource = RequestsService.WaitRequestsCollection;
        }

        /// <summary>
        /// Принятые заявки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptedRequests_Checked(object sender, RoutedEventArgs e)
        {
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = RequestsService.AcceptRequestsCollection;

            Block();
        }

        /// <summary>
        /// Заявки в ожидании
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitingRequests_Checked(object sender, RoutedEventArgs e)
        {
            if (datagrid == null)
                return;

            datagrid.ItemsSource = null;
            datagrid.ItemsSource = RequestsService.WaitRequestsCollection;

            UnBlock();
        }

        /// <summary>
        /// Отмененные заявки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanseledRequests_Checked(object sender, RoutedEventArgs e)
        {
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = RequestsService.CanselRequestsCollection;

            Block();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanselRequest_Click(object sender, RoutedEventArgs e)
        {
            var request = datagrid.SelectedItem as Request.Request;
            RequestsService.ChangeRequestsStatus(request, State.Canseled);

            NotificationService.AddNotification(request, "Администратор отклонил вашу заявку");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            var request = datagrid.SelectedItem as Request.Request;

            RequestsService.ChangeRequestsStatus(request, State.Accepted);

            NotificationService.AddNotification(request, "Администратор принял вашу заявку");
        }

        private void Block()
        {
            acceptItem.IsEnabled = false;
            canselItem.IsEnabled = false;
        }

        private void UnBlock()
        {
            acceptItem.IsEnabled = true;
            canselItem.IsEnabled = true;
        }
    }
}
