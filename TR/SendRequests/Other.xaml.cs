using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TR.Data;
using TR.MainData;
using TR.Notification;

namespace TR.Requests
{
    /// <summary>
    /// Interaction logic for Other.xaml
    /// </summary>
    public partial class Other : UserControl
    {
        public Other()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
            if (!String.IsNullOrEmpty(text))
            {
                ForUser.RequestsService.SendRequest(text);
                textBox.Document.Blocks.Clear();
            }
        }
    }
}
