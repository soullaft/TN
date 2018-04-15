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
using TR.Windows;

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
            MenuWindow window = new MenuWindow();
            (Application.Current.MainWindow as MainWindow).Hide();
            window.Closed += (s, ev) => Application.Current.Shutdown();
            window.Show();
        }
    }
}
