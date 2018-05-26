using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TR.Requests
{
    /// <summary>
    /// Interaction logic for Device.xaml
    /// </summary>
    public partial class Device : UserControl
    {
        public Device()
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
