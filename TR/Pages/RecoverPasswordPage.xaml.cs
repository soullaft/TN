using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TR.Data;
using TR.Email;
using TR.MainData;
using TR.Notification;

namespace TR
{
    /// <summary>
    /// Логика взаимодействия для RecoverPasswordPage.xaml
    /// </summary>
    public partial class RecoverPasswordPage : Page
    {


        #region Private fields
        /// <summary>
        /// Секретный код
        /// </summary>
        private String secretCode;

        private EmailSender emailSender;

        private AnimationHelper helper;

        private Employee employee;

        #endregion


        #region Конструктор
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public RecoverPasswordPage()
        {
            InitializeComponent();
            helper = new AnimationHelper();
            emailSender = new EmailSender();
        }
        #endregion


        /// <summary>
        /// Отправить код на почту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCodeToEmail_Click(object sender, RoutedEventArgs e)
        {
            if(!InternetState.CheckForInternetConnection())
            {
                ContexTrayMenu.ShowMessage("Ошибка!", "Отсутствует подключение к интернету!", System.Windows.Forms.ToolTipIcon.Error);
                return;
            }
            //Если поле почтового ящика не пусто и почтовый ящик "валидный"
            if (!String.IsNullOrEmpty(emailTextBox.Text) && EmailChecker.IsValidEmail(emailTextBox.Text))
            {
                //Получаем ссылку на пользователя
                employee = EmployeeService.UsersCollection.Where(x => x.Email == emailTextBox.Text).FirstOrDefault();

                //если пользователь есть в базе
                if (employee != null)
                {

                    //Генерируем секретный код
                    secretCode = Randomizer.GetNumber(1, 10000);

                    //Получаем введенный пользователем почтовый ящик
                    String email = emailTextBox.Text.Trim();

                    //Отправляем сообщение на введенный пользователем почтовый ящик с секретный кодом
                    Task.Run(() =>
                    {

                        emailSender.Body = $"Код для восстановления пароля : {secretCode}";

                        emailSender.Subject = "Восстановление пароля";

                        emailSender.To = email;

                        emailSender.Send();

                    });

                    //Запускаем анимацию, которая переводит нас на "страницу" подтверждения секретного кода
                    AnimationHelper.StartAnimation(this, "EmailToCode", delegate { });
                }
                else
                    ContexTrayMenu.ShowMessage("Ошибка!", "Пользователя с таким почтовым ящиком не существует!", System.Windows.Forms.ToolTipIcon.Error);
            }
            else
                ContexTrayMenu.ShowMessage("Ошибка!", "Поле не может быть пустым или введенный текст не ялвяется почтовым ящиком!", System.Windows.Forms.ToolTipIcon.Error);
        }

        /// <summary>
        /// При нажатии на Enter(Отправить код на почту)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEmail_EnterPress(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                SendCodeToEmail_Click(this, new RoutedEventArgs());
        }


        /// <summary>
        /// Подтверждение кода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {
            if (codeTextBox.Text.Trim() == secretCode)
                AnimationHelper.StartAnimation(this, "applyToChange", delegate { });
            else
                ContexTrayMenu.ShowMessage("Ошибка!", "Неправильный код!", System.Windows.Forms.ToolTipIcon.Error);
        }


        /// <summary>
        /// При нажатии на Enter (Подтверждение кода)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckCode_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CheckCode_Click(this, new RoutedEventArgs());
        }


        /// <summary>
        /// Смена пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if(passwordbox1.Password == passwordbox2.Password)
            {
                using (var connection = new MySqlConnection(ConnectionDB.Connection))
                {

                    employee.Password = HashCode.GenerateHash(passwordbox2.Password);

                    var query = $"UPDATE Employers SET Password = '{employee.Password}' WHERE ID = {employee.ID}";

                    Task.Run(() =>
                    {
                        connection.Open();
                        var cmd = new MySqlCommand(query, connection);
                        cmd.ExecuteNonQuery();

                        EmployeeService.UsersCollection.Where(x => x.ID == employee.ID).FirstOrDefault().Update(employee);
                    });

                    var loginPage = new LoginPage();

                    loginPage.Loaded += delegate
                    {
                        AnimationHelper.StartAnimation(loginPage, "loadingPage", delegate { });
                    };

                    (Application.Current.MainWindow as MainWindow).mainFrame.Content = loginPage;

                    ContexTrayMenu.ShowMessage("Информация!", "Пароль был успешно изменен!", System.Windows.Forms.ToolTipIcon.Info);

                }
            }
            else
                ContexTrayMenu.ShowMessage("Ошибка!", "Пароли должны совпадать!", System.Windows.Forms.ToolTipIcon.Error);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePassword_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ChangePassword_Click(this, new RoutedEventArgs());
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToEnterPage_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).mainFrame.Content = new LoginPage();
        }
    }
}
