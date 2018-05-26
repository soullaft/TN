using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TR.Data;
using TR.ExcelHelper;
using TR.MainData;

namespace TR.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        #region Приватные поля
        

        ExcelReader excelReader;

        #endregion

        /// <summary>
        /// Путь до выбранного изображения
        /// </summary>
        public string Path { get; set; }


        public AddUserPage()
        {
            InitializeComponent();

            excelReader = new ExcelReader();

            usersGrid.DataContext = excelReader;
        }


        /// <summary>
        /// Происходит при загрузки изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image;
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
            };

            //Если пользователь в диалогом окне выбрал изображение и нажал "Ок"
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    image = new BitmapImage(new Uri(dialog.FileName));
                    //Если разрешение изображения меньше 1280 на 720, то 
                    if (image.Width <= 1280 && image.Height <= 720)
                    {
                        profilePhoto.Source = image;
                        Path = dialog.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Проверка чтобы в текстовом поле содержались тольк цифры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlyNumbers_Input(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(surnameText.Text) && !String.IsNullOrEmpty(nameText.Text) && !String.IsNullOrEmpty(midnameText.Text)
                && !String.IsNullOrEmpty(roomText.Text) && !String.IsNullOrEmpty(emailText.Text)
                && !String.IsNullOrEmpty(phoneText.Text) && !String.IsNullOrEmpty(loginText.Text) && !String.IsNullOrEmpty(passText.Password)
                && !String.IsNullOrEmpty(repeatPassText.Password) && (passText.Password == repeatPassText.Password)
                && ((bool)admin.IsChecked || (bool)user.IsChecked || (bool)tech.IsChecked))
            {

                Roles role;

                if ((bool)admin.IsChecked)
                    role = Roles.Admin;
                else if ((bool)tech.IsChecked)
                    role = Roles.Technical;
                else
                    role = Roles.User;

                Employee employee = new Employee()
                {
                    FIO = surnameText.Text.Trim() + " " + nameText.Text.Trim() + " " + midnameText.Text.Trim(),
                    Room = Convert.ToInt64(roomText.Text.Trim()),
                    Email = emailText.Text.Trim(),
                    Phone = phoneText.Text.Trim(),
                    Type = role,
                    Login = loginText.Text.Trim(),
                    Image = new BitmapImage(new Uri(Path)),
                    Password = HashCode.GenerateHash(repeatPassText.Password.Trim())
                };

                EmployeeService.AddUserAsync(employee, Path.Substring(Path.LastIndexOf(".") + 1), profilePhoto.Source);

                CanseAddUserGrid_Click(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Показать пользователей(Excel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowExcelUsers_Click(object sender, RoutedEventArgs e)
        {
            if (!excelReader.OpenFile())
                return;

            usersTable.Visibility = Visibility.Visible;
            usersGrid.Visibility = Visibility.Visible;

            AnimationHelper.StartAnimation(this, "ShowUsersTable", delegate { });
        }

        /// <summary>
        /// Добавить пользователей через excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUsersExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Отменить добавление пользователей через excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanselAddUsers_Click(object sender, RoutedEventArgs e)
        {
            excelReader.usersList.Clear();

            AnimationHelper.StartAnimation(this, "HideUsersTable", delegate 
            {
                usersTable.Visibility = Visibility.Collapsed;
                usersGrid.Visibility = Visibility.Collapsed;
            });
        }

        /// <summary>
        /// Показать "Добавление пользователя"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUserGrid_Click(object sender, RoutedEventArgs e)
        {
            addUserGrid.Visibility = Visibility.Visible;

            AnimationHelper.StartAnimation(this, "ShowUserTable", delegate { });
        }

        /// <summary>
        /// Отменить "Добавление пользователя"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanseAddUserGrid_Click(object sender, RoutedEventArgs e)
        {
            AnimationHelper.StartAnimation(this, "HideUserTable", delegate
            {
                surnameText.Text = null;
                nameText.Text = null;
            });
        }

        
    }
}
