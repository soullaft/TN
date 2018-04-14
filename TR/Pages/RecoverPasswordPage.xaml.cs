using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TR.Classes;
using TR.Email;

namespace TR
{
    /// <summary>
    /// Логика взаимодействия для RecoverPasswordPage.xaml
    /// </summary>
    public partial class RecoverPasswordPage : Page
    {
        private String secretCode;
        private EmailSender emailSender;
        AnimationHelper helper;
        public RecoverPasswordPage()
        {
            InitializeComponent();
            helper = new AnimationHelper();
            emailSender = new EmailSender();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(emailTextBox.Text) && EmailChecker.IsValidEmail(emailTextBox.Text))
            {
                secretCode = Randomizer.GetNumber(1, 10000);
                String email = emailTextBox.Text.Trim();
                Task.Run(() =>
                {
                    emailSender.Body = $"Код для восстановления пароля : {secretCode}";
                    emailSender.Subject = "Восстановление пароля";
                    emailSender.Send(email);
                });
                AnimationHelper.StartAnimation(this, "EmailToCode", (o, ee) =>
                {

                });
            }
            else
                MessageBox.Show("Введенный email не существует или поле не было заполнено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void emailTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Button_Click(this, new RoutedEventArgs());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (codeTextBox.Text.Trim() == secretCode)
            {
                AnimationHelper.StartAnimation(this, "applyToChange", (o, ee) =>
                {
                    
                });
            }
        }

        private void codeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click_1(this, new RoutedEventArgs());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(passwordbox1.Password == passwordbox2.Password)
            {

            }
        }

        private void passwordbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click_2(this, new RoutedEventArgs());
        }
    }
}
