using System;
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

            //Если resource наследник класса Page
            if (resource is Page)
                storyBoard = (resource as Page).Resources[StoryBoardName] as Storyboard;

            //Если resource наследник класса Window
            else if (resource is Window)
                //получаем доступ к ресурсам и присваиваем ссылку на ресурус StoryBoardName storyBoard'y
                storyBoard = (resource as Window).Resources[StoryBoardName] as Storyboard;
            //Если resource наследник UserControl
            else if (resource is UserControl)
                storyBoard = (resource as UserControl).Resources[StoryBoardName] as Storyboard;

            else
                throw new ArgumentException("Параметр resource должен быть наследником Page,Window или UserControl");

            //Подписываемся на событие, которое произойдет при завершении анимации
            storyBoard.Completed += handler;
            storyBoard.Begin();
            
        }
    }
}
