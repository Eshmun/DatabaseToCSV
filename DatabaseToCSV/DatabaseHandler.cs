using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DatabaseToCSV
{
    class DatabaseHandler
    {
        SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
        SqliteConnection connection;
        public DatabaseHandler(string filePath)
        {
            filePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + filePath;
            connectionStringBuilder.DataSource = filePath;
            connectionStringBuilder.Mode = SqliteOpenMode.ReadOnly;
            
            connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();
            
        }
        
        //Get the first date in the database
        public DateTime GetStartDate()
        {
            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT created FROM states WHERE entity_id = \"sensor.blitzwolf01_amperage\" ORDER BY state_id ASC LIMIT 1";
            var reader = cmd.ExecuteReader();
            return reader.GetDateTime(0);
        }

        //Get the last date in the database
        public DateTime GetEndDate()
        {
            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT created FROM states WHERE entity_id = \"sensor.blitzwolf01_amperage\" ORDER BY state_id DESC LIMIT 1";
            var reader = cmd.ExecuteReader();
            return reader.GetDateTime(0);
        }

        // Get the measurements from the database
        public Object[] GetRows(int amount, DateTime minDate, DateTime maxDate)
        {
            Object[] rows = new Object[amount];

            SqliteCommand ampCmd = connection.CreateCommand();
            ampCmd.CommandText = String.Format(
                "SELECT state_id, created, state FROM states WHERE entity_id = \"sensor.blitzwolf01_amperage\" AND created BETWEEN \"{1}\" AND \"{2}\" ORDER BY state_id ASC LIMIT {0}",
                amount,
                minDate.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                maxDate.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

            SqliteCommand voltCmd = connection.CreateCommand();
            voltCmd.CommandText = String.Format(
                "SELECT state_id, created, state FROM states WHERE entity_id = \"sensor.blitzwolf01_voltage\" AND created BETWEEN \"{1}\" AND \"{2}\" ORDER BY state_id ASC LIMIT {0}",
                amount,
                minDate.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                maxDate.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

            SqliteDataReader ampReader = ampCmd.ExecuteReader();
            SqliteDataReader voltReader = voltCmd.ExecuteReader();
            int i = 0;
            while(ampReader.Read() && voltReader.Read())
            {
                int ampId = ampReader.GetInt32(0);
                int voltId = voltReader.GetInt32(0);
                DateTime ampDateTime = ampReader.GetDateTime(1);
                DateTime voltDateTime = voltReader.GetDateTime(1);
                float ampState = ampReader.GetFloat(2);
                float voltState = voltReader.GetFloat(2);

                rows[i] = new Object[] { ampId, ampDateTime, ampState, voltState };

                i++;
            }
            return rows;
        }
    }
}
