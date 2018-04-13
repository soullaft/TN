using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TR
{
    /// <summary>
    /// Класс помогающий использовать анимации
    /// </summary>
    public sealed class AnimationHelper
    {
        /// <summary>
        /// Запускает анимацию
        /// </summary>
        /// <param name="resource">Page или Window</param>
        /// <param name="StoryBoardName">Имя тригера</param>
        /// <param name="handler"></param>
        public static void StartAnimation(UIElement resource, string StoryBoardName, EventHandler handler)
        {
            Storyboard storyBoard;

            //Если resouce наследник класса Page
            if (resource is Page)
                storyBoard = (resource as Page).Resources[StoryBoardName] as Storyboard;

            //Если resouce наследник класса Window
            else if (resource is Window)
                //получаем доступ к ресурсам и присваиваем ссылку на ресурус StoryBoardName storyBoard'y
                storyBoard = (resource as Window).Resources[StoryBoardName] as Storyboard;

            else
                throw new ArgumentException("Параметр resource должен быть наследником Page или Window");

            //Подписываемся на событие, которое произойдет при завершении анимации
            storyBoard.Completed += handler;
            storyBoard.Begin();
            
        }
    }
}
