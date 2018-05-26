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
            EmployeeService.RefreshUsersStatusAsync();

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
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
