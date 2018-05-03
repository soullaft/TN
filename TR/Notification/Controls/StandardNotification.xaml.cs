using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TR.Pages;

namespace TR.Notification.Controls
{
    /// <summary>
    /// Interaction logic for StandardNotification.xaml
    /// </summary>
    public partial class StandardNotification : UserControl
    {
        public StandardNotification(NotificationModel model)
        {
            InitializeComponent();

            textBox.Text = model.Text = "Пользователь отправил заявку";
        }

        private void textBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AnimationHelper.StartAnimation(this, "OnMouseLeftButtonDown1", (x, y) =>
            {
                (Application.Current.MainWindow as MenuWindow).kek.Children.Remove(this);
            });
        }

        private void ShowRequest_Click(object sender, MouseButtonEventArgs e)
        {
            (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new RequestsPage();

            AnimationHelper.StartAnimation((Application.Current.MainWindow as MenuWindow), "hideNotifications", delegate 
            {
                (Application.Current.MainWindow as MenuWindow).mainFrame.Effect = null;
                (Application.Current.MainWindow as MenuWindow).kek.Children.Remove(this);
            });
        }
    }
}
