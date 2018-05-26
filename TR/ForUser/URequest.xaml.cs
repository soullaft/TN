using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace TR.ForUser
{
    /// <summary>
    /// Interaction logic for URequest.xaml
    /// </summary>
    public partial class URequest : UserControl
    {
        public URequest(long ReqID, string Text, DateTime Date, string State, Brush color)
        {
            InitializeComponent();

            border.Background = color;

            idText.Text = ReqID.ToString();
            textText.Text = Text;
            dateText.Text = Date.ToString();
        }
    }
}
