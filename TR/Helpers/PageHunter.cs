using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TR
{
    public class PageHunter
    {
        /// <summary>
        /// Предыдущая страница
        /// </summary>
        private static MenuItem _prevItem;

        /// <summary>
        /// Изменяет цвет картинки и текста для текущей страницы
        /// </summary>
        /// <param name="sender">Вызывающий MenuItem</param>
        public static void ChangePage(MenuItem sender)
        {

            var res = sender as MenuItem;

            if (_prevItem != null)
            {
                var text = ((Grid)res.Header).Children[1] as TextBlock;

                var image = ((Grid)res.Header).Children[0] as PackIcon;


                var prevText = ((Grid)_prevItem.Header).Children[1] as TextBlock;

                var prevImage = ((Grid)_prevItem.Header).Children[0] as PackIcon;

                prevImage.Foreground = Brushes.Black;
                prevText.Foreground = Brushes.Black;

                text.Foreground = Brushes.Red;
                image.Foreground = Brushes.Red;


                _prevItem = res;
            }

            else
            {
                var text = ((Grid)res.Header).Children[1] as TextBlock;

                var image = ((Grid)res.Header).Children[0] as PackIcon;

                text.Foreground = Brushes.Red;
                image.Foreground = Brushes.Red;

                _prevItem = sender;
            }
        }
    }
}
