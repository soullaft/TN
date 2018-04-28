using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TR.Classes;

namespace TR.Windows
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private MenuItem _prevItem;
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _prevItem = horrizontalMenu.Items[0] as MenuItem;

            PageHunter.ChangePage(_prevItem);

            DataService service = new DataService();
    }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PageHunter.ChangePage(sender as MenuItem);
        }
        private void Notify_Click(object sender, RoutedEventArgs e)
        {
            AnimationHelper.StartAnimation(this, "Ding", (x, y) => { });
        }
    }
}
