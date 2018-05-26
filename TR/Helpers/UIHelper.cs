using System.Windows;

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
    }
}
