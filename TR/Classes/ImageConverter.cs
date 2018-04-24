using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TR.Classes
{
    public class ImageConverter
    {
        /// <summary>
        /// Конвертирует массив байтов в BitmapImage
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static BitmapImage ConvertImage(Object array)
        {
            byte[] imageData = array as byte[];

            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }


        /// <summary>
        /// Конвертирует изображение в массив байтов
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        private byte[] ConvertImage(BitmapEncoder encoder, ImageSource imageSource)
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
