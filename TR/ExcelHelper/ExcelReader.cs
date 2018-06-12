using ExcelDataReader;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using TR.Data;

namespace TR.ExcelHelper
{
    /// <summary>
    /// Считывает данные из excel файла
    /// </summary>
    public sealed class ExcelReader
    {

        DataSet result;

        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        public static ObservableCollection<Employee> UsersList { get; set; }


        public ExcelReader()
        {
            UsersList = new ObservableCollection<Employee>();
        }

        /// <summary>
        /// Открывает диалоговое окно с выбором Excel файла
        /// </summary>
        public bool OpenFile()
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true})
            {
                //Если пользователь нажал "Ок"
                if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                result = reader.AsDataSet();
                            }
                        }
                        ReadFile(result);
                        return true;
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message); }
                }
                return false;
            }
        }
        /// <summary>
        /// Считывает данные из файла
        /// </summary>  
        /// <param name="data"></param>
        private void ReadFile(DataSet data)
        {
            //Очищаем коллекцию пользователей
            UsersList.Clear();

            //Для каждой итерации по строке создаем Employee и добавляем его в коллекцию пользователей
            for (int i = 1; i < data.Tables[0].Rows.Count; i++)
            {
                Employee employee = new Employee()
                {
                    FIO = data.Tables[0].Rows[i][0].ToString(),
                    Room = Convert.ToInt64(data.Tables[0].Rows[i][1]),
                    Email = data.Tables[0].Rows[i][2].ToString(),
                    Phone = data.Tables[0].Rows[i][3].ToString(),
                    Login = data.Tables[0].Rows[i][4].ToString(),
                    Password = data.Tables[0].Rows[i][5].ToString(),
                    Image = null,
                    Type = Roles.User
                };
                UsersList.Add(employee);
            }
        }
    }
}
