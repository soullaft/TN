using System.Xml;
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
            XmlDocument xDoc = new XmlDocument();

            xDoc.Load("../../Connection.xml");

            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "server")
                        Server = childnode.InnerText;

                    if (childnode.Name == "database")
                        DataBase = childnode.InnerText;

                    if (childnode.Name == "user")
                        User = childnode.InnerText;

                    if (childnode.Name == "password")
                        Password = childnode.InnerText;
                }
            }
        }

        static string Connect()
        {
            return "server=" + Server + ";user=" + User + ";database=" +
                DataBase + ";password=" + Password + ";charset=utf8;";
        }

        public static void UpdateSettings()
        {
            Server = Settings.Default.server1;
            DataBase = Settings.Default.database1;
            User = Settings.Default.user1;
            Password = Settings.Default.password1;
        }
    }
}
