using System;
using System.Drawing;
using System.Windows.Forms;
using TR.Data;
using TR.Pages;

namespace TR.Notification
{
    /// <summary>
    /// Контекстное меню программы
    /// </summary>
    public class ContexTrayMenu
    {
        #region Private fields

        //Значок программы(трей)
        private static NotifyIcon notifyIcon;

        //Контекстное меню
        private static ContextMenuStrip contextMenu;
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public ContexTrayMenu() { }

        /// <summary>
        /// Конструктор типа
        /// </summary>
        static ContexTrayMenu()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = new Icon("withoutLayer.ico"),
                Visible = true
            };

            contextMenu = new ContextMenuStrip();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выйти", Image.FromFile("exit.png"), Exit);

            contextMenu.Items.Add(exitItem);

            notifyIcon.ContextMenuStrip = contextMenu;

        }

        #endregion

        #region События Контекстного меню 
        /// <summary>
        /// При клике на "Заявки"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Requests(object sender, EventArgs e)
        {
            //(System.Windows.Application.Current.MainWindow as MenuWindow).mainFrame.Content as Page)
            //PageHunter.ChangePage((System.Windows.Application.Current.MainWindow as MenuWindow).mainFrame.Content as Page));
            (System.Windows.Application.Current.MainWindow as MenuWindow).mainFrame.Content = new RequestsPage();
        }

        /// <summary>
        /// При клике на "Пользователи"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Users(object sender, EventArgs e)
        {
            (System.Windows.Application.Current.MainWindow as MenuWindow).mainFrame.Content = new UsersPage();
        }

        /// <summary>
        /// Выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Exit(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// <summary>
        /// Показывает уведомление
        /// </summary>
        /// <param name="title">Заголовок уведомления</param>
        /// <param name="text">Текст уведомления</param>
        /// <param name="tip">Тип иконки уведомления</param>
        public static void ShowMessage(string title, string text, ToolTipIcon tip)
        {
            notifyIcon.ShowBalloonTip(200, title, text, tip);
        }


        /// <summary>
        /// Показывает контекстное меню
        /// </summary>
        /// <param name="role">Тип пользователя</param>
        public static void Show(Roles role)
        {
            contextMenu.Items.Clear();

            switch (role)
            {
                case Roles.Admin:
                    AdminPanel();
                    break;
                case Roles.User:
                    UserPanel();
                    break;
                default:
                    UserPanel();
                    break;
            }
        }

        #region For each type of user
        /// <summary>
        /// Контекстное меню администратора
        /// </summary>
        private static void AdminPanel()
        {
            ToolStripMenuItem usersMenuItem = new ToolStripMenuItem("Пользователи", Image.FromFile("users.png"), Users);

            ToolStripMenuItem requestsMenuItem = new ToolStripMenuItem("Заявки", Image.FromFile("requests.png"), Requests);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выйти", Image.FromFile("exit.png"), Exit);

            contextMenu.Items.AddRange(new[] { usersMenuItem, requestsMenuItem, exitItem});

            notifyIcon.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Контекстное меню пользователя
        /// </summary>
        private static void UserPanel()
        {
            ToolStripMenuItem requestsMenuItem = new ToolStripMenuItem("Заявки", Image.FromFile("requests.png"), Requests);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выйти", Image.FromFile("exit.png"), Exit);

            contextMenu.Items.AddRange(new[] { requestsMenuItem, exitItem });

            notifyIcon.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Контекстное меню тех отдела
        /// </summary>
        private static void TechPanel()
        {
            ToolStripMenuItem usersMenuItem = new ToolStripMenuItem("Пользователи", Image.FromFile("users.png"), Users);

            ToolStripMenuItem requestsMenuItem = new ToolStripMenuItem("Заявки", Image.FromFile("requests.png"), Requests);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выйти", Image.FromFile("exit.png"), Exit);

            contextMenu.Items.AddRange(new[] { usersMenuItem, requestsMenuItem, exitItem });

            notifyIcon.ContextMenuStrip = contextMenu;
        }
        #endregion

        #endregion
    }
}
