using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TR.Data;
using TR.MainData;

namespace TR.Pages
{
    /// <summary>
    /// Interaction logic for EditUserPage.xaml
    /// </summary>
    public partial class EditUserPage : Page
    {

        #region Private Fields
        /// <summary>
        /// Путь до выбранного изображения
        /// </summary>
        string path;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long id;

        #endregion



        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        /// <param name="employee">редактируемый пользователь</param>
        public EditUserPage(Employee employee)
        {
            InitializeComponent();

            profilePhoto.Source = employee.Image;

            var fio = employee.FIO.Split(' ');

            surnameText.Text = fio[0];

            nameText.Text = fio[1];

            midnameText.Text = fio[2];

            roomText.Text = employee.Room.ToString();

            emailText.Text = employee.Email;

            phoneText.Text = employee.Phone;

            loginText.Text = employee.Login;

            id = employee.ID;
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
                        path = dialog.FileName;
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

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (profilePhoto.Source != null && !String.IsNullOrEmpty(surnameText.Text) && !String.IsNullOrEmpty(nameText.Text) && !String.IsNullOrEmpty(midnameText.Text)
                && !String.IsNullOrEmpty(roomText.Text) && !String.IsNullOrEmpty(emailText.Text) && !String.IsNullOrEmpty(phoneText.Text) && !String.IsNullOrEmpty(loginText.Text))
            {


                Employee employee = new Employee()
                {
                    ID = id,
                    FIO = surnameText.Text.Trim() + " " + nameText.Text.Trim() + " " + midnameText.Text.Trim(),
                    Room = Convert.ToInt64(roomText.Text.Trim()),
                    Email = emailText.Text.Trim(),
                    Phone = phoneText.Text.Trim(),
                    Login = loginText.Text.Trim(),
                };


                EmployeeService.UpdateUserAsync(employee);

                (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new UsersPage();
            
            }
        }
    }
}
