using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TR.Data;
using TR.Email;
using TR.Notification;

namespace TR.Pages
{
    /// <summary>
    /// Страница отправки сообщения на почту
    /// </summary>
    public partial class SendEmailPage : Page
    {
        /// <summary>
        /// Письмо
        /// </summary>
        private EmailSender email;

        public SendEmailPage(int id)
        {
            InitializeComponent();

            DataContext = new EmailSender();

            email = new EmailSender
            {
                To = EmployeeService.UsersCollection.Where(x => x.ID == id).FirstOrDefault().Email
            };
        }

        /// <summary>
        /// Добавить вложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAttachments_Click(object sender, RoutedEventArgs e)
        {
            email.AddAttachments();
            attachmentsPanel.Children.Clear();
            foreach (var item in EmailSender.attachmentsView)
                attachmentsPanel.Children.Add(item);
        }

        /// <summary>
        /// Отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new UsersPage();
            attachmentsPanel.Children.Clear();
            EmailSender.attachmentsView.Clear();
        }

        /// <summary>
        /// Отправить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (InternetState.CheckForInternetConnection())
            {
                email.Subject = subjectText.Text;

                TextRange textRange = new TextRange(bodyText.Document.ContentStart, bodyText.Document.ContentEnd);

                email.Body = textRange.Text;

                Task.Run(() => { email.Send(); });

                ContexTrayMenu.ShowMessage("Уведомление!", "Сообщение было успешно отправлено!", System.Windows.Forms.ToolTipIcon.Info);

                (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new UsersPage();
            }
            else
                ContexTrayMenu.ShowMessage("Ошибка!", "Отсутствует подключение к интернету!", System.Windows.Forms.ToolTipIcon.Error);
        }
    }
}
