using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TR.Data;
using TR.MainData;
using TR.Notification;
using TR.Pages;

namespace TR.ForUser
{
    public class RequestsService
    {
        public static List<URequest> RequestsList = new List<URequest>();

        public static UsersRequestsPage UserRequests = new UsersRequestsPage(); 

        public static void GetRequests(long id)
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                
                    connection.Open();
                    using (var reader = new MySqlCommand($"SELECT * FROM Requests WHERE IDEmpl = {id}", connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Application.Current.Dispatcher.Invoke(() => RequestsList.Add(
                                new URequest(reader.GetInt64("ID"), reader.GetString("Text"), reader.GetDateTime("Date"), reader.GetInt64("State").ToString(), SetBackGround(reader.GetInt64("State")))));
                        }
                    }
                    connection.Close();
                
            }
        }

        public async static void SendRequest(string text)
        {
            using (var connection = new MySqlConnection(ConnectionDB.Connection))
            {
                var requestQuery = $"INSERT INTO Requests(IDEmpl,Text,Date,State) VALUES({CurrentUser.ID}, @Text, @Date, 0)";

                var notificatationQuery = $"INSERT INTO Notifications(IDEmployee, IDType, Text, Target) VALUES ({CurrentUser.ID}, 1, 'Пользователь отправил новую заявку', 0)";

                var cmd = new MySqlCommand(requestQuery, connection);

                cmd.Parameters.Add("@Text", MySqlDbType.Text);
                cmd.Parameters["@Text"].Value = text;

                cmd.Parameters.Add("@Date", MySqlDbType.DateTime);
                cmd.Parameters["@Date"].Value = DateTime.Now;

                var cmd1 = new MySqlCommand(notificatationQuery, connection);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await cmd1.ExecuteNonQueryAsync();
                await connection.CloseAsync();


                ContexTrayMenu.ShowMessage("Уведомление!", "Ваша заявка была успешно отрпавлена", System.Windows.Forms.ToolTipIcon.Info);

                UserRequests.requestsPanel.Children.Clear();
                RequestsList.Clear();

                GetRequests(CurrentUser.ID);
            }
        }

        #region Helpers
        private static Brush SetBackGround(long state)
        {
            var bc = new BrushConverter();

            switch ((Request.State)state)
            {
                case Request.State.Accepted:
                    return (Brush)bc.ConvertFrom("#FF2D9B69");
                case Request.State.Canseled:
                    return (Brush)bc.ConvertFrom("#FFB04B62");
                default:
                    return (Brush)bc.ConvertFrom("#FFB4AA48");
            }
        }
        #endregion
    }
}

