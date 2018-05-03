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
using TR.Classes;
using TR.Data;
using TR.MainData;

namespace TR.Pages
{
    /// <summary>
    /// Interaction logic for EditUserPage.xaml
    /// </summary>
    public partial class EditUserPage : Page
    {

        #region Приватные поля
        /// <summary>
        /// Путь до выбранного изображения
        /// </summary>
        string path;

        /// <summary>
        /// пользователь
        /// </summary>
        Employee _employee;
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

            _employee = employee;

            buttonText.Content = "Сохранить";
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
                && !String.IsNullOrEmpty(roomText.Text) && !String.IsNullOrEmpty(emailText.Text) && !String.IsNullOrEmpty(phoneText.Text) && !String.IsNullOrEmpty(loginText.Text))
            {
                string query = $"UPDATE Employers SET RNumber = @Room,FIO = @FIO, Phone = @Phone, Email = @Email, Login = @Login Where ID = {_employee.ID}";

                using (var connection = new MySqlConnection(ConnectionDB.Connection))
                {

                    Employee employee = new Employee()
                    {
                        FIO = surnameText.Text.Trim() + " " + nameText.Text.Trim() + " " + midnameText.Text.Trim(),
                        Room = Convert.ToInt64(roomText.Text.Trim()),
                        Email = emailText.Text.Trim(),
                        Phone = phoneText.Text.Trim(),
                        Login = loginText.Text.Trim(),
                        Image = (BitmapImage)profilePhoto.Source,
                    };

                    var cmd = new MySqlCommand(query, connection);

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

                    //EmployeeService.UsersCollection.Insert(EmployeeService.UsersCollection.IndexOf(_employee), employee);

                    EmployeeService.UsersCollection[EmployeeService.UsersCollection.IndexOf(_employee)].Update(employee);

                    (Application.Current.MainWindow as MenuWindow).mainFrame.Content = new UsersPage();

                    Task.Run(() =>
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    });
                }
            }
        }
    }
}
