using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using TR.Data;
using TR.Notification;
using TR.Pages;
using TR.Request;

namespace TR
{
    /// <summary>
    /// Главное окно программы
    /// </summary>
    public partial class MenuWindow : Window
    {
        #region Приватные Поля
        /// <summary>
        /// Предыдущая страница
        /// </summary>
        private MenuItem _prevItem;

        /// <summary>
        /// Предыдущая страница
        /// </summary>
        private int prevpage;

        /// <summary>
        /// Сервис уведомлений
        /// </summary>
        private NotificationService notivicationService;

        #endregion

        #region Конструктор
        /// <summary>
        /// Конструктор по-умолчанию 
        /// </summary>
        public MenuWindow(int prevItem)
        {
            InitializeComponent();

            prevpage = prevItem;
        }

        #endregion


        #region При загрузки страницы
        /// <summary>
        /// Происходит при загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _prevItem = horrizontalMenu.Items[prevpage] as MenuItem;

            PageHunter.ChangePage(_prevItem);

            ContexTrayMenu.Show(CurrentUser.Role);

            new RequestsService();

            var role = EmployeeService.UsersCollection.Where(user => user.ID == CurrentUser.ID).First().Type;

            //Если вошел администратор, то получаем все уведомления с таргетом 0
            if (role == Roles.Admin)
            {
                this.Title = "Администатор";
                notivicationService = new NotificationService(0);
                Task.Factory.StartNew(() => NotificationService.ClearNotifications(0));
            }
            //Если вошел администратор, то получаем все уведомления с таргетом идентификатор пользователя
            else if (role == Roles.User)
            {
                this.Title = "Пользователь";
                notivicationService = new NotificationService(Convert.ToInt32(CurrentUser.ID));
                ForUser.RequestsService.GetRequests(CurrentUser.ID);
                Task.Factory.StartNew(() => NotificationService.ClearNotifications(Convert.ToInt32(CurrentUser.ID)));
            }
            else
            //Если вошел администратор, то получаем все уведомления с таргетом 1
            {
                this.Title = "Технический отдел";
                notivicationService = new NotificationService(1);
            }
        }

        #endregion

        #region Навигация по страницам
        /// <summary>
        /// Открыть "Уведомления"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notify_Click(object sender, RoutedEventArgs e)
        {
            if(notificationPanel.Visibility == Visibility.Collapsed)
                AnimationHelper.StartAnimation(this, "showNotifications", (x, y) => 
                {
                    BlurEffect effect = new BlurEffect()
                    {
                        Radius = 3,
                        RenderingBias = RenderingBias.Quality
                    };

                    mainFrame.Effect = effect;
                });
            else
                AnimationHelper.StartAnimation(this, "hideNotifications", (x, y) => 
                {
                    mainFrame.Effect = null;
                });
        }

        /// <summary>
        /// Переход на страницу "Пользователи"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsersPage_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content.ToString() == "TR.Pages.UsersPage")
                return;

            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = new UsersPage();
        }
        /// <summary>
        /// Переход на страницу "Заявки"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestsPage_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content.ToString() == "TR.Pages.RequestsPage")
                return;
            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = new RequestsPage();
        }
        /// <summary>
        /// Переход на страницу "История"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryPage_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content.ToString() == "TR.Pages.HistoryPage")
                return;
            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = new HistoryPage();
        }
        /// <summary>
        /// Отправить заявку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content.ToString() == "TR.Pages.SendRequestPage")
                return;
            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = new SendRequestPage();
        }

        /// <summary>
        /// Перейти к заявкам пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsersRequests_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content.ToString() == "TR.Pages.UsersRequestsPage")
                return;

            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = ForUser.RequestsService.UserRequests;
        }
        #endregion


    }
}
