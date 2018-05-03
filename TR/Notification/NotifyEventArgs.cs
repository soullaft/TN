using System;

namespace TR.Notification
{
    /// <summary>
    /// Содержит данные о заявке и пользователе, который ее отправил
    /// </summary>
    class NotifyEventArgs : EventArgs
    {
        /// <summary>
        /// Идентификатор работника
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType Type { get; set; }
    }
}
