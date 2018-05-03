namespace TR.Notification
{
    /// <summary>
    /// Уведомление
    /// </summary>
    public class NotificationModel
    {
        /// <summary>
        /// Идентификатор заявки
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long IDEmployee { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Text { get; set; }
    }
}
