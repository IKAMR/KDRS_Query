using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDRS_Query
{
    class Query
    {

        List<string> queryInfo = new List<string>();

        public List<QueryClass> QueryList { get; set; } = new List<QueryClass>();
        public List<SQL_Query> SqlQueryList { get; set; } = new List<SQL_Query>();

        // Extracts all query information from query text file into query list
        public string GetQuery(string filename)
        {
            Console.WriteLine("Reading queries");

            using (StreamReader reader = new StreamReader(File.OpenRead(filename), Encoding.Default))
            {
                String line;
                while (((line = reader.ReadLine()) != null))
                {
                    Console.WriteLine(line);


                    if (line.StartsWith("["))
                    {
                        string qType = line.Split('[', ']')[1];
                        Console.WriteLine(qType);

                        string qStart = @"#START#";
                        string qStop = @"#STOP#";
                        string queryText = "";
                        while ((line = reader.ReadLine().Trim()) != null)
                        {

                            queryInfo.Add(line);

                            if (line.Equals(qStart))
                            {
                                Console.WriteLine("START found");

                                while (!(line = reader.ReadLine()).Equals(qStop))
                                {
                                    queryText += line + "\r\n";
                                }
                                queryText = queryText.TrimEnd('\r', '\n');
                                queryInfo.Add(queryText);
                                CreateQuery(qType, queryInfo);
                                queryInfo.Clear();
                                break;
                            }

                        }
                    }
                }
            }

            Console.WriteLine("All queries read");

            foreach (string s in queryInfo)
                Console.WriteLine(s);

            return "";
        }

        public void CreateQuery(string qType, List<string> queryInfoList)
        {
            switch (qType)
            {
                case "XML_QUERY":
                    MakeXMLQuery(queryInfoList);
                    break;
                case "SQL_QUERY":
                    MakeSQLQuery(queryInfoList);
                    break;
            }
        }


        // Reads SQL queries from queryInfoList into SQL_Query object.
        public void MakeSQLQuery(List<string> queryInfoList)
        {
            SQL_Query sqlQuery = new SQL_Query();

            SqlQueryList.Add(sqlQuery);
            sqlQuery.JobId = queryInfoList[1].Split('=')[1];
            sqlQuery.JobEnabled = queryInfoList[2].Split('=')[1];
            sqlQuery.JobName = queryInfoList[3].Split('=')[1].Trim();
            sqlQuery.JobDescription = queryInfoList[4].Split('=')[1].Trim();
            sqlQuery.System = queryInfoList[6].Split('=')[1];
            sqlQuery.SubSystem = queryInfoList[7].Split('=')[1];
            sqlQuery.Source = queryInfoList[8].Split('=')[1];
            sqlQuery.Target = queryInfoList[9].Split('=')[1];

            sqlQuery.Server = queryInfoList[11].Split('=')[1];
            sqlQuery.Database = queryInfoList[12].Split('=')[1];
            sqlQuery.User = queryInfoList[13].Split('=')[1];
            sqlQuery.Psw = queryInfoList[14].Split('=')[1];
            sqlQuery.Query = queryInfoList[17];
        }

        // Reads XPath queries from queryInfoList into XML_Query object.
        public void MakeXMLQuery(List<string> queryInfoList)
        {
            XML_Query query = new XML_Query();

            QueryList.Add(query);
            query.JobId = queryInfoList[1].Split('=')[1];
            query.JobEnabled = queryInfoList[2].Split('=')[1];
            query.JobName = queryInfoList[3].Split('=')[1].Trim();
            query.JobDescription = queryInfoList[4].Split('=')[1].Trim();
            query.System = queryInfoList[6].Split('=')[1];
            query.SubSystem = queryInfoList[7].Split('=')[1];
            query.Source = queryInfoList[8].Split('=')[1].Trim();
            query.Target = queryInfoList[9].Split('=')[1];
            query.Query = queryInfoList[12];
        }
    }

    public enum ClassType { XML_Query, SQL_Query }

    public class QueryClass
    {
        public string JobId { get; set; }
        public string JobEnabled { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Query { get; set; }
        public string Result { get; set; }

        public static QueryClass Create(ClassType classType)
        {
            switch (classType)
            {
                case ClassType.XML_Query: return new XML_Query();
                case ClassType.SQL_Query: return new SQL_Query();
                default: throw new ArgumentOutOfRangeException();
            }

        }
    }

    public class XML_Query : QueryClass
    {

    }
    public class SQL_Query : QueryClass
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Psw { get; set; }

    }
}
