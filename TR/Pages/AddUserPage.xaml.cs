using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TR.Classes;
using TR.Data;
using TR.MainData;

namespace TR.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        #region Приватные поля
        /// <summary>
        /// Путь до выбранного изображения
        /// </summary>
        string path;

        #endregion
        public AddUserPage()
        {
            InitializeComponent();
        }
        public AddUserPage(Employee employee)
        {
            InitializeComponent();
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
                    else
                    {
                        new CustomMessageWindow("Выберите изображение меньшего размера").ShowDialog();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (profilePhoto.Source != null && !String.IsNullOrEmpty(surnameText.Text) && !String.IsNullOrEmpty(nameText.Text) && !String.IsNullOrEmpty(midnameText.Text)
                && !String.IsNullOrEmpty(roomText.Text) && !String.IsNullOrEmpty(emailText.Text) 
                && !String.IsNullOrEmpty(phoneText.Text) && !String.IsNullOrEmpty(loginText.Text) && !String.IsNullOrEmpty(passText.Password)
                && !String.IsNullOrEmpty(repeatPassText.Password) && (passText.Password == repeatPassText.Password) 
                && ((bool)admin.IsChecked || (bool)user.IsChecked || (bool)tech.IsChecked))
            {
                using (var connection = new MySqlConnection(ConnectionDB.Connection))
                {

                    string query = "INSERT INTO Employers (RNumber,FIO,Phone,Email,Login,Password,Image, Role) values (@Room,@FIO,@Phone,@Email,@Login,@Password,@Image, @Role)";

                    var cmd = new MySqlCommand(query, connection);

                    Employee employee = new Employee()
                    {
                        //ID = EmployeeService.UsersCollection[EmployeeService.UsersCollection.Count - 1].ID + 1,
                        FIO = surnameText.Text.Trim() + " " + nameText.Text.Trim() + " " + midnameText.Text.Trim(),
                        Room = Convert.ToInt64(roomText.Text.Trim()),
                        Email = emailText.Text.Trim(),
                        Phone = phoneText.Text.Trim(),
                        Login = loginText.Text.Trim(),
                        Image = new BitmapImage(new Uri(path)),
                    };

                    Roles role;

                    if ((bool)admin.IsChecked)
                        role = Roles.Admin;
                    else if ((bool)tech.IsChecked)
                        role = Roles.Technical;
                    else
                        role = Roles.User;

                    cmd.Parameters.Add("@Role", MySqlDbType.Int64);
                    cmd.Parameters["@Role"].Value = Convert.ToInt64(role);

                    cmd.Parameters.Add("@FIO", MySqlDbType.VarChar);
                    cmd.Parameters["@FIO"].Value = employee.FIO;

                    cmd.Parameters.Add("@Room", MySqlDbType.Int64);
                    cmd.Parameters["@Room"].Value = employee.Room;

                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar);
                    cmd.Parameters["@Email"].Value = employee.Email;

                    cmd.Parameters.Add("@Phone", MySqlDbType.Int64);
                    cmd.Parameters["@Phone"].Value = employee.Phone;

                    cmd.Parameters.Add("@Login", MySqlDbType.VarChar);
                    cmd.Parameters["@Login"].Value = employee.Login;

                    cmd.Parameters.Add("@Password", MySqlDbType.VarChar);
                    cmd.Parameters["@Password"].Value = HashCode.GenerateHash(repeatPassText.Password.Trim());

                    cmd.Parameters.Add("@Image", MySqlDbType.LongBlob);

                    if (path.Substring(path.LastIndexOf(".") + 1) == "png")
                        cmd.Parameters["@Image"].Value = ImageConverter.ConvertImage(new PngBitmapEncoder(), profilePhoto.Source);
                    else if (path.Substring(path.LastIndexOf(".") + 1) == "jpg" || path.Substring(path.LastIndexOf(".") + 1) == "jpeg")
                        cmd.Parameters["@Image"].Value = ImageConverter.ConvertImage(new JpegBitmapEncoder(), profilePhoto.Source);
                    else
                    {
                        new CustomMessageWindow("Выберите другое изображение").ShowDialog();
                        return;
                    }

                    EmployeeService.UsersCollection.Add(employee);

                    (Application.Current.MainWindow as MenuWindow).Content = new UsersPage();
                    
                    Task.Run(()=> 
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    });
                }
            }
            else
                new CustomMessageWindow("Все поля должны быть заполнены!").ShowDialog();
        }

        private static void EncodeTo()
        {

        }
    }
}
