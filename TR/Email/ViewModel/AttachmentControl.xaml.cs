using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TR.Email.ViewModel
{
    /// <summary>
    /// Interaction logic for AttachmentControl.xaml
    /// </summary>
    public partial class AttachmentControl : UserControl
    {
        /// <summary>
        /// Путь до файла
        /// </summary>
        public string Path { get; set; }

        public AttachmentControl(string path)
        {
            InitializeComponent();

            Path = path;
            AddImage(path);

            fileText.Text = System.IO.Path.GetFileName(path);
        }

        private void CloseButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnimationHelper.StartAnimation(this, "OnMouseLeftButtonDown1", (x, y) =>
            {
                ((Panel)this.Parent).Children.Remove(this);
                EmailSender.attachmentsView.Remove(this);
            });
        }

        private void AddImage(string path)
        {
            var type = GetFileExtension(path).ToUpper();

            if (type == "JPG" || type == "JPEG" || type == "PNG")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/image.png"));
            else if(type == "TXT" || type == "LOG")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/txt.png"));
            else if (type == "PDF")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/pdf.png"));

            else if (type == "DOC" || type == "PPT" || type == "DOCX")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/word.png"));

            else if (type == "ZIP")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/zip.png"));

            else if (type == "RAR")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/rar.png"));

            else if (type == "XLSX" || type == "XLSM" || type == "XLS")
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/excel.png"));

            else
                fileImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/types/question.png"));

        }

        /// <summary>
        /// Узнать расширение файла
        /// </summary>
        /// <param name="fileName">Путь до файла</param>
        /// <returns></returns>
        public string GetFileExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf(".") + 1);
        }
    }
}
