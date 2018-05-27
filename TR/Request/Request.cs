using System;

namespace TR.Request
{
    /// <summary>
    /// Заявка
    /// </summary>
    public class Request
    {
        /// <summary>
        ///Идентификатор 
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public long EmployeeID { get; set; }

        /// <summary>
        /// Текст заявки
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// ФИО сотрудника
        /// </summary>
        public string FIOUser { get; set; }

        /// <summary>
        /// Cостояния заявки
        /// </summary>
        public State State;
    }
}
