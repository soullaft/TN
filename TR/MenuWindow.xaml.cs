using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using TR.Classes;
using TR.Notification;
using TR.Pages;

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

        private int prevpage;

        /// <summary>
        /// Сервис уведомлений
        /// </summary>
        private NotificationService notivicationService = new NotificationService();
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
            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = new HistoryPage();
        }
        /// <summary>
        /// Отправить заявку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendrequest_Click(object sender, RoutedEventArgs e)
        {
            PageHunter.ChangePage(sender as MenuItem);
            mainFrame.Content = new SendRequestPage();
        }
        #endregion

    }
}
