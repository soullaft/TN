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

        private void closeButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
