using TR.Properties;

namespace TR.MainData
{
    public class ConnectionDB
    {
        private static string Server { get; set; }
        private static string DataBase { get; set; }
        private static string User { get; set; }
        private static string Password { get; set; }
        private static string Charset { get; set; }

        public static string Connection
        {
            get { return Connect(); }
        }

        static ConnectionDB()
        {
            Server = Settings.Default.server;
            DataBase = Settings.Default.database;
            User = Settings.Default.user;
            Password = Settings.Default.password;
            Charset = Settings.Default.charset;
        }

        static string Connect()
        {
            return "server=" + Server + ";user=" + User + ";database=" +
                DataBase + ";password=" + Password + ";";
        }

        public static void UpdateSettings()
        {
            Server = Settings.Default.server1;
            DataBase = Settings.Default.database1;
            User = Settings.Default.user1;
            Password = Settings.Default.password1;
            Charset = Settings.Default.charset;
        }
    }
}
