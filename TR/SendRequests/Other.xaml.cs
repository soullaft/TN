using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
            try
            {
                var text = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
                if (!String.IsNullOrEmpty(text))
                {
                    ForUser.RequestsService.SendRequest(text);
                    textBox.Document.Blocks.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }
    }
}
