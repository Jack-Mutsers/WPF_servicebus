using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class SqlData
    {
        private string _connectionString { get; } = "server={server};userid={user};password={password};database={database};";
        public string connectionString
        {
            get
            {
                string link = _connectionString;
                link = link.Replace("{server}", server);
                link = link.Replace("{user}", user);
                link = link.Replace("{password}", password);
                link = link.Replace("{database}", database);
                return link;
            }
        }

        private string server = @"localhost";
        private string user = "root";
        private string password = "";
        private string database = "proftaak";
        //public string connectionString = "Server=tcp:s2-battleships.database.windows.net,1433;Initial Catalog=ArcadeScores;Persist Security Info=False;User ID=StiptonShip;Password={y3gH1Za32Qdrc84};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
