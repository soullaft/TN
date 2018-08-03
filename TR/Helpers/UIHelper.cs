using System.Windows;
using System.Windows.Media.Effects;

namespace TR
{
    /// <summary>
    /// Представляет методы для работы с UI частью 
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Разблокирует меню
        /// </summary>
        public static void UnBlockTabs()
        {
            (Application.Current.MainWindow as MenuWindow).horrizontalMenu.IsEnabled = true;
            (Application.Current.MainWindow as MenuWindow).dopMenu.IsEnabled = true;
        }

        /// <summary>
        /// Эффект замыливания
        /// </summary>
        /// <returns></returns>
        public static BlurEffect GetBlur() => new BlurEffect()
        {
            Radius = 5,
            RenderingBias = RenderingBias.Performance
        };
    }
}
