using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;
using TR.Email.ViewModel;

namespace TR.Email
{
    /// <summary>
    /// Класс для отправки сообщений на почтовые ящики
    /// </summary>
    public class EmailSender
    {
        #region Публичные свойства
        /// <summary>
        /// Получатель сообщения
        /// </summary>
        public String To { get; set; }

        /// <summary>
        /// Тело сообщение
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        /// Тема сообщения
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        /// Коллекция вложений
        /// </summary>
        public static ObservableCollection<AttachmentControl> attachmentsView = new ObservableCollection<AttachmentControl>();

        #endregion

        #region Приватные переменные
        /// <summary>
        /// Почтовые ящики
        /// </summary>
        protected static List<MailAddress> emails = new List<MailAddress>();

        /// <summary>
        /// Сообщение электронной почты
        /// </summary>
        readonly private static MailMessage mailMessage = new MailMessage()
        {
            From = new MailAddress("transneft063@gmail.com")
        };



        /// <summary>
        /// СМТП клиент(через него будет производится отправка сообщений)
        /// </summary>
        readonly private static SmtpClient smtp = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("transneft063@gmail.com", "transneft"),
            EnableSsl = true,
        };

        #endregion


        /// <summary>
        /// Добавить вложения(файлы) 
        /// </summary>
        public void AddAttachments()
        {
            //Пробегаемся по каждому выбранному файлу и добавляем его в коллекцию вложений
            foreach (var item in OpenDialog())
            {
                attachmentsView.Add(new AttachmentControl(item));
            }
        }

        /// <summary>
        /// Открывает окно для выбора файлов
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> OpenDialog()
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog() { Filter = "All Files (*.*)|*.*", Multiselect = true })
            {
                //Если пользователь нажал "Ок"
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return openFileDialog.FileNames;
                else
                    return new List<String>();
            }       
        }



        /// <summary>
        /// Добавить почтовые ящики к сообщению для SMTP клиента
        /// </summary>
        /// <param name="emailsList">Список почтовых ящиков</param>
        private void AddEmails(IEnumerable<String> emailsList)
        {
            // Очищаем список почтовых ящиков подлежащих рассылке
            ClearEmails();

            //Добавляем почтовые ящики к рассылке
            foreach (var item in emailsList)
            {
                if(EmailChecker.IsValidEmail(item))
                    mailMessage.To.Add(item);
                else
                    throw new ArgumentException("Incorrect email");
            }
        }

        /// <summary>
        /// Очищает список получателей
        /// </summary>
        private static void ClearEmails()
        {
            // Очищаем список почтовых ящиков подлежащих рассылке
            mailMessage.To.Clear();
        }

        /// <summary>
        /// Очищает список файлов для рассылки
        /// </summary>
        private  void ClearAttachments()
        {
            attachmentsView.Clear();
        }

        /// <summary>
        /// Шаблон стандартного сообщения
        /// </summary>
        private void StandardMessage()
        {
            //Тело сообщения
            mailMessage.Body = Body;

            //Тема сообщения
            mailMessage.Subject = Subject;

            //Отправляем сообщение
            smtp.Send(mailMessage);
        }


        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="emails">Почтовый ящик получателя</param>
        /// <param name="withAttach">отправить сообщение с файлами(вложениями)</param>
        public void Send(String To = "")
        {
            //Кому будет отправлено письмо
            mailMessage.To.Add(this.To);

            foreach (var item in attachmentsView)
                mailMessage.Attachments.Add(new Attachment(item.Path));

            //Тело сообщения
            mailMessage.Body = Body;

            //Тема сообщения
            mailMessage.Subject = Subject;

            //Отправляем сообщение
            smtp.Send(mailMessage);

            ClearAttachments();

            //Задействуем сборку мусора
            GC.Collect();
        }
    }
}
