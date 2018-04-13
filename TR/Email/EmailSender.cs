using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using TR.Classes;

namespace TR.Email
{
    /// <summary>
    /// Класс для отправки сообщений на почтовые ящики
    /// </summary>
    public class EmailSender
    {
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
        /// Почтовые ящики
        /// </summary>
        protected static List<MailAddress> emails = new List<MailAddress>();

        /// <summary>
        /// Вложения
        /// </summary>
        protected static List<Attachment> attachments = new List<Attachment>();

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



        /// <summary>
        /// Добавить вложения(файлы) 
        /// </summary>
        public void ConvertAttachments()
        {
            //Пробегаемся по каждому выбранному файлу и добавляем его в коллекцию вложений
            foreach (var item in OpenDialog())
                attachments.Add(new Attachment(item));
        }

        private static IEnumerable<string> OpenDialog()
        {
            // Открываем диалоговое окно для выбора вложений(файлов)
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Multiselect = true
            };

            //Если пользователь нажал "Ок"
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileNames;
            else
                throw new Exception("");
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
        private static void ClearAttachments()
        {
            attachments.Clear();
        }

        /// <summary>
        /// Добавляет вложения(файлы) к письму, которое будет разослано
        /// </summary>
        private static void AddAttachments()
        {
            if (attachments != null)
                foreach (var item in attachments)
                    mailMessage.Attachments.Add(item);
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
        /// <param name="emails">Список почтовых ящиков</param>
        /// <param name="withAttach">отправить сообщение с файлами(вложениями)</param>
        public void Send(IEnumerable<String> emails, Boolean withAttach = true)
        {

            //Добавляем почтовые ящики, которые выбраны для рассылки
            AddEmails(emails);

            //Если сообщение с вложениями, то добавляем вложения к письму
            if (withAttach == true) AddAttachments();

            StandardMessage();

            //Задействуем сборку мусора
            GC.Collect();

            ClearAttachments();

        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="to">Кому отправить</param>
        public void Send(String to)
        {
            if (EmailChecker.IsValidEmail(to))
            {
                mailMessage.To.Add(to);

                StandardMessage();

                ClearEmails();
            }
        }
    }
}
