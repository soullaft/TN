using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TR.Classes;
using TR.Data;
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

        private void label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = Application.Current.MainWindow as MainWindow;
            window.mainFrame.Content = new RecoverPasswordPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(password.Password))
            {
                var currentUser = EmployeeService.UsersCollection.Where(user => user.Login == login.Text && user.Password == HashCode.GenerateHash(password.Password)).FirstOrDefault();
                if (currentUser != null)
                {
                    if (currentUser.Type == Roles.Admin)
                    {
                        MenuWindow window = new MenuWindow(0);
                        (Application.Current.MainWindow as MainWindow).Hide();
                        window.Closed += (s, ev) => Application.Current.Shutdown();
                        Application.Current.MainWindow = window;

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
                    else if (currentUser.Type == Roles.User)
                    {
                        MenuWindow window = new MenuWindow(2);
                        window.users.Visibility = Visibility.Collapsed;
                        window.requests.Visibility = Visibility.Collapsed;
                        window.sendrequest.Visibility = Visibility.Visible;
                        (Application.Current.MainWindow as MainWindow).Hide();
                        window.Closed += (s, ev) => Application.Current.Shutdown();
                        Application.Current.MainWindow = window;

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
                    else
                    {

                    }
                }
                else
                    new CustomMessageWindow("Неправильный логин или пароль!").ShowDialog();

            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;


                if (textBox.Text.Trim() == "root")
                {
                    (Application.Current.MainWindow as MainWindow).mainFrame.Content = new DBSettingsPage();
                }
            }
        }
        /// <summary>
        /// При загрузки страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    EmployeeService service = new EmployeeService();
                }
                catch { }
            });
        }
        

        /// <summary>
        /// При нажатии на "Enter" имея фокус на passwordbox
        /// </summary>
        /// <param name="sender">passowrdBox</param>
        /// <param name="e"></param>
        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(this, new RoutedEventArgs());
        }
    }
}
