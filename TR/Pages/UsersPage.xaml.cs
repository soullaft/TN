using TR.Data;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TR.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {

        private ObservableCollection<Employee> result = new ObservableCollection<Employee>();

        public UsersPage()
        {
            InitializeComponent();

            this.DataContext = new EmployeeService();

        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new AddUserPage();
        }

        #region Контекстное меня таблицы
        /// <summary>
        /// Редактировать пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            //Если выбранный элемент таблицы не пуст
            if(dataGrid.SelectedItem != null)
            {
                //Переходим на страницу редактирования пользователя
                (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new EditUserPage((dataGrid.SelectedItem as Employee));
            }
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            //Если выбранный элемент таблицы не пуст
            if (dataGrid.SelectedItem != null)
            {
                //Переходим на страницу отправки сообщения
                (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new SendEmailPage((int)(dataGrid.SelectedItem as Employee).ID);
            }
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {

                deleteUserGrid.Visibility = Visibility.Visible;

                AnimationHelper.StartAnimation(this, "ShowDeleteGrid", delegate
                {
                    dataGrid.Effect = UIHelper.GetBlur();

                    BlockActions(false);
                });


            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            CanselDeleteUser_Click(sender, new RoutedEventArgs());

            EmployeeService.DeleteUser((dataGrid.SelectedItem as Employee).ID);
        }
        #endregion

        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = (sender as TextBox).Text;

            if (string.IsNullOrEmpty(searchText))
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = EmployeeService.UsersCollection;

                return;
            }

            result.Clear();

            foreach (var item in EmployeeService.UsersCollection.Where(x => x.FIO.ToUpper().Contains(searchText.ToUpper())))
                result.Add(item);

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = result;
        }

        /// <summary>
        /// Переходит на страницу заявок пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowUserRequests_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void RefreshUsersStatus_Click(object sender, RoutedEventArgs e)
        {

            await Task.Factory.StartNew(() => EmployeeService.RefreshUsersStatusAsync());

            dataGrid.ItemsSource = null;

            dataGrid.ItemsSource = EmployeeService.UsersCollection;
        }

        /// <summary>
        /// Отменить удаление пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanselDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            AnimationHelper.StartAnimation(this, "HideDeleteGrid", delegate
            {
                deleteUserGrid.Visibility = Visibility.Collapsed;

                dataGrid.Effect = null;

                BlockActions(true);
            });
        }

        /// <summary>
        /// Блокирует/разблокирует действия
        /// </summary>
        /// <param name="value">false - заблокировать, true - разблокировать</param>
        private void BlockActions(bool value)
        {
            dataGrid.IsEnabled = value;
            topMenu.IsEnabled = value;
        }
    }
}
