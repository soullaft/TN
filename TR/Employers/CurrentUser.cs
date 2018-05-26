using TR.Data;

namespace TR.Data
{
    /// <summary>
    /// Предоставляет основную информацию и вошедшем пользователе
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public static Roles Role { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public static long ID { get; set; }
    }
}
