using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TR.Data;
using TR.Notification;
using TR.Pages;

namespace TR
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Восстановить пароль 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecoverPassowordLabel_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = Application.Current.MainWindow as MainWindow;
            window.mainFrame.Content = new RecoverPasswordPage();
        }

        /// <summary>
        /// При нажатии на кнопку "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(password.Password))
            {

                var currentUser = EmployeeService.UsersCollection.Where(user => user.Login == login.Text && user.Password == HashCode.GenerateHash(password.Password)).FirstOrDefault();

                if (currentUser != null)
                {
                    //Если введенный пользователь - администратор
                    if (currentUser.Type == Roles.Admin)
                    {
                        MenuWindow window = new MenuWindow(0);

                        (Application.Current.MainWindow as MainWindow).Hide();
                        window.Closed += (s, ev) => Application.Current.Shutdown();
                        Application.Current.MainWindow = window;

                        CurrentUser.Role = currentUser.Type;

                        CurrentUser.ID = currentUser.ID;

                        window.Show();

                        Task.Run(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {

                                (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new UsersPage();


                                UIHelper.UnBlockTabs();
                            });
                        });
                    }
                    //Если введенный пользователь - обычный работник
                    else if (currentUser.Type == Roles.User)
                    {
                        MenuWindow window = new MenuWindow(2);

                        window.users.Visibility = Visibility.Collapsed;

                        window.requests.Visibility = Visibility.Collapsed;

                        window.sendrequest.Visibility = Visibility.Visible;

                        window.usersRequest.Visibility = Visibility.Visible;

                        (Application.Current.MainWindow as MainWindow).Hide();

                        window.Closed += (s, ev) => Application.Current.Shutdown();

                        Application.Current.MainWindow = window;

                        CurrentUser.Role = currentUser.Type;

                        CurrentUser.ID = currentUser.ID;

                        window.Show();

                        Task.Run(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new SendRequestPage();

                                UIHelper.UnBlockTabs();
                            });
                        });
                    }
                    //Если введенный пользователь - работник технического отдела
                    else
                    {

                    }
                }
                else
                    ContexTrayMenu.ShowMessage("Ошибка!", "Неправильный логин или пароль", System.Windows.Forms.ToolTipIcon.Error);

            }
            else
                ContexTrayMenu.ShowMessage("Ошибка!", "Поля должны быть заполнены!", System.Windows.Forms.ToolTipIcon.Error);
        }
        /// <summary>
        /// При нажатии на кнопку 'Enter' и фокусе на логин поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            var textBox = sender as TextBox;

            if (textBox.Text.Trim() == "root")
                (Application.Current.MainWindow as MainWindow).mainFrame.Content = new DBSettingsPage();
        }
        /// <summary>
        /// При загрузкe страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await EmployeeService.FillEmployeersAsync();

            enterButton.IsEnabled = true;
            label.IsEnabled = true;
            password.KeyDown += Password_KeyDown;
        }
        

        /// <summary>
        /// При нажатии на "Enter" имея фокус на Passwordbox
        /// </summary>
        /// <param name="sender">passowrdBox</param>
        /// <param name="e"></param>
        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EnterButton_Click(this, new RoutedEventArgs());
        }
    }
}
