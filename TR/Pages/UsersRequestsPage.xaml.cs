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
using TR.Data;

namespace TR.Pages
{
    /// <summary>
    /// Interaction logic for UsersRequestsPage.xaml
    /// </summary>
    public partial class UsersRequestsPage : Page
    {
        public UsersRequestsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (requestsPanel.Children.Count > 0)
                return;
            foreach (var item in ForUser.RequestsService.RequestsList)
                requestsPanel.Children.Add(item);
        }
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            requestsPanel.Children.Clear();
            ForUser.RequestsService.RequestsList.Clear();
            await Task.Run(() => ForUser.RequestsService.GetRequests(CurrentUser.ID));
            foreach (var item in ForUser.RequestsService.RequestsList)
                requestsPanel.Children.Add(item);
        }
    }
}
