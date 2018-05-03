namespace TR.Notification
{
    /// <summary>
    /// Типы уведомлений
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Для теста
        /// </summary>
        dummy,
        /// <summary>
        /// Пользователь отравил заявку
        /// </summary>
        newRequest,
        /// <summary>
        /// Пользователь изменил пароль
        /// </summary>
        newPassword,
    }
}
