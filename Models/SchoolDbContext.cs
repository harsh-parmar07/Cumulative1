using MySql.Data.MySqlClient;

namespace Cumulative_1.Models
{
    public class SchoolDbContext
    {
        // Input the details of the username, password, server, and port number to connect the server to the database
        private static string User { get { return "root"; } }
        private static string Password { get { return ""; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        // ConnectionString is a series of credentials which is used to connect to the database
        protected static string ConnectionString
        {
            get
            {
                // convert zero datetime is a db connection setting which returns NULL if the date is 0000-00-00
                // which allows interpretation of the date in Csharp

                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }



        /// We use this method to get Database
        /// <summary>
        /// It returns a connection to the Database
        /// </summary>
        /// <example>
        /// private SchoolDbContext Teachers = new SchoolDbContext();
        /// MySqlConnection Connection = Teachers.AccessDatabase();
        /// </example>
        /// <returns>A MySqlConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            // We are giving instance to SchoolDbContext class to create an object
            // The object is a specific connection to our school database on port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }
    }
}
