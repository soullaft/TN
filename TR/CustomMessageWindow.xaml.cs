using System.Windows;


namespace TR
{
    /// <summary>
    /// Interaction logic for CustomMessageWindow.xaml
    /// </summary>
    public partial class CustomMessageWindow : Window
    {
        public CustomMessageWindow(string text)
        {
            InitializeComponent();

            this.Icon = null;

            infoText.Text = text;
        }

        /// <summary>
        /// При нажатии на кнопку "Ок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
