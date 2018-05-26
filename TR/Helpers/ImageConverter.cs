using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TR
{
    public static class ImageConverter
    {
        /// <summary>
        /// Конвертирует изображение в массив байтов
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static byte[] ConvertImage(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;


            if (imageSource is BitmapSource bitmapSource)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }
            return bytes;
        }
    }
}
