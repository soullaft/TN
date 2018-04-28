using TR.Data;
using System.Windows.Controls;

namespace TR.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();

            dataGrid.ItemsSource = DataService.UsersCollection;
        }
    }
}
