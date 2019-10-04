using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDRS_Query
{
    class MYSQL_Runner
    {
        public void RunSQL(SQL_Query sqlQuery)
        {
            string connectionstring;
            MySqlConnection cnn = null;
            MySqlDataReader reader = null;

            connectionstring = "server=" + sqlQuery.Server + ";database=" + sqlQuery.Database + ";uid=" + sqlQuery.User + ";pwd=" + sqlQuery.Psw;
            Console.WriteLine("Connection string: " + connectionstring);
            cnn = new MySqlConnection(connectionstring);
            try
            {
                cnn.Open();
                Console.WriteLine("Connection Open");

                string query = "select * from arkiv";

                MySqlCommand cmd = new MySqlCommand(sqlQuery.Query, cnn);
                reader = cmd.ExecuteReader();

                int ColumnCount = reader.FieldCount;
                string ListOfColumns = string.Empty;

                while (reader.Read())
                {
                    for (int i = 0; i<ColumnCount; i++)
                    {
                        ListOfColumns = ListOfColumns + reader[i].ToString() + "|";
                    }
                    ListOfColumns = ListOfColumns + "\r\n";
                }

                sqlQuery.Result = ListOfColumns;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Unable to open connection!");

                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (cnn != null)
                {
                    cnn.Close();
                    Console.WriteLine("Connection Closed");
                }

            }
        }
    }
}
