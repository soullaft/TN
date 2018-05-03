using TR.Data;
using System.Windows.Controls;
using System.Windows;
using TR.Classes;
using System;

namespace TR.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();

            this.DataContext = new EmployeeService();

            filterBox.ItemsSource = Filter.FiltersList;
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
            new CustomMessageWindow("Вы действительно хотите удалить этого пользователя?").ShowDialog();
        }
        #endregion

        /// <summary>
        /// При нажатии на кнопку "Поиск"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (filterBox.SelectedValue != null)
            {
                MessageBox.Show(filterBox.SelectedValue.ToString());
            }
            else
                new CustomMessageWindow("Укажите фильтр для поиска!").ShowDialog();
        }
    }
}
