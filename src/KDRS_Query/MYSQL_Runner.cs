using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KDRS_Query
{
    class MYSQL_Runner
    {

        public delegate void ProgressUpdate(string queryId);
        public event ProgressUpdate OnProgressUpdate;

        string openDatabase;

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
                OnProgressUpdate?.Invoke(sqlQuery.JobId);
                cnn.Open();
                Console.WriteLine("Connection Open");
                OnProgressUpdate?.Invoke("\r\nConnection Open. Server: " + sqlQuery.Server + ", Database: " + sqlQuery.Database );


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
                OnProgressUpdate?.Invoke("\r\nUnable to open connection");

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
                    OnProgressUpdate?.Invoke("\r\nConnection Closed");

                }

            }
        }

        public void RunSQL(List<SQL_Query> sqlQueries, string sourceFolder)
        {
            string connectionstring;
            MySqlConnection cnn = null;
            MySqlDataReader reader = null;

            foreach (SQL_Query sqlQuery in sqlQueries)
            {
                try
                {
                    if (openDatabase == null || !openDatabase.Equals(sqlQuery.Database))
                    {


                        if (cnn != null)
                        {
                            cnn.Close();
                            Console.WriteLine("Connection Closed");
                            OnProgressUpdate?.Invoke("\r\nConnection Closed");

                        }

                        connectionstring = "server=" + sqlQuery.Server + ";database=" + sqlQuery.Database + ";uid=" + sqlQuery.User + ";pwd=" + sqlQuery.Psw;
                        Console.WriteLine("Connection string: " + connectionstring);
                        cnn = new MySqlConnection(connectionstring);

                        cnn.Open();
                        OnProgressUpdate?.Invoke("\r\nConnection Open. Server: " + sqlQuery.Server + ", Database: " + sqlQuery.Database);
                        openDatabase = sqlQuery.Database;

                    }
                    string time = GetTimeStamp();
                    OnProgressUpdate?.Invoke("\r\n" + time + " " + sqlQuery.JobId);
                    Console.WriteLine("Connection Open");

                    MySqlCommand cmd = new MySqlCommand(sqlQuery.Query, cnn);
                    reader = cmd.ExecuteReader();

                    int ColumnCount = reader.FieldCount;
                    string ListOfColumns = string.Empty;

                    while (reader.Read())
                    {
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            ListOfColumns = ListOfColumns + reader[i].ToString() + "|";
                        }
                        ListOfColumns = ListOfColumns + "\r\n";
                    }

                    sqlQuery.Result = ListOfColumns;
                    OnProgressUpdate?.Invoke("\r\n" + sqlQuery.JobId + " - Success " + GetTimeStamp());

                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Unable to open connection!");
                    OnProgressUpdate?.Invoke("\r\nUnable to open connection");
                    openDatabase = null;

                    throw ex;
                }

            }

            if (cnn != null)
            {
                cnn.Close();
                Console.WriteLine("Connection Closed");
                OnProgressUpdate?.Invoke("\r\nConnection Closed");

            }
        }

        //******************************************************************

        public string GetTimeStamp()
        {
            return DateTime.Now.ToString("HH.mm.ss");
        }
    }
}
