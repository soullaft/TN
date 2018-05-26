using System;
using System.Windows;
using System.Windows.Controls;
using TR.Data;
using TR.MainData;
using TR.Properties;

namespace TR.Pages
{
    /// <summary>
    /// Логика взаимодействия для DBSettingsPage.xaml
    /// </summary>
    public partial class DBSettingsPage : Page
    {
        public DBSettingsPage()
        {
            InitializeComponent();

            server.Text = Settings.Default.server;
            database.Text = Settings.Default.database;
            user.Text = Settings.Default.user;
            password.Password = Settings.Default.password;
        }

        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings.Default.server = server.Text;
                Settings.Default.database = database.Text;
                Settings.Default.user = user.Text;
                Settings.Default.password = password.Password;

                Settings.Default.Save();

                ConnectionDB.UpdateSettings();
            }
            catch(TypeInitializationException ex)
            {
                ex.ToString();
                Settings.Default.server = Settings.Default.server1;
                Settings.Default.database = Settings.Default.database1;
                Settings.Default.user = Settings.Default.user1;
                Settings.Default.password = Settings.Default.password1;

                Settings.Default.Save();

                ConnectionDB.UpdateSettings();
            }
        }

        /// <summary>
        /// Назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).mainFrame.Content = new LoginPage();
        }
    }
}
