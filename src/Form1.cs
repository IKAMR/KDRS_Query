using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KDRS_Query
{
    public partial class Form1 : Form
    {

        List<QueryClass> queryList = new List<QueryClass>();
        List<SQL_Query> sqlQueryList = new List<SQL_Query>();

        List<string> queryInfo = new List<string>();

        XPathQueryRunner xPRunner = new XPathQueryRunner();
        MYSQL_Runner sqlRunner = new MYSQL_Runner();

        string targetFolder;

        public Form1()
        {
            InitializeComponent();
            Text = Globals.toolName + " " + Globals.toolVersion;
        }

        // Extracts all query information from query text file into query list
        private string GetQuery(string filename)
        {
            Console.WriteLine("Reading queries");

            using (StreamReader reader = new StreamReader(File.OpenRead(filename),Encoding.Default))
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
                                    queryText += "\r\n" + line;
                                }
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
        private void MakeSQLQuery(List<string> queryInfoList)
        {
            SQL_Query sqlQuery = new SQL_Query();

            sqlQueryList.Add(sqlQuery);
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

            queryList.Add(query);
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

        private void btnRunQ_Click(object sender, EventArgs e)
        {
            txtLogbox.Text = "Running queries";
            string inFile = txtInFile.Text;
            targetFolder = txtTrgtPath.Text;

             string queryFile = txtQFile.Text;

            if (String.IsNullOrEmpty(queryFile))
                queryFile = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\xml_queries.txt";

            Console.WriteLine("Reading queries from: " + inFile);

            GetQuery(queryFile);

            string outFile = Path.Combine(targetFolder, "kdrs_query_results.txt");

            xPRunner.RunXpath2(queryList, inFile);

            foreach (SQL_Query sql_Query in sqlQueryList)
            {
                sqlRunner.RunSQL(sql_Query);
            }
            using (File.Create(outFile)) { }

            // Creating text file containing all query info including query results
            using (StreamWriter w = File.AppendText(outFile))
            {

                foreach (XML_Query query in queryList)
                {
                    if (query.JobEnabled.Equals("1"))
                    {
                        txtLogbox.AppendText("\r\n" + query.JobId);

                        w.WriteLine(query.JobId);
                        w.WriteLine(query.JobEnabled);
                        w.WriteLine(query.JobName);
                        w.WriteLine(query.JobDescription);
                        w.WriteLine(query.System);
                        w.WriteLine(query.SubSystem);
                        w.WriteLine(query.Source);
                        w.WriteLine(query.Target);
                        w.WriteLine(query.Query);
                        w.WriteLine("");
                        w.WriteLine("Query result:");
                        w.WriteLine(query.Result);
                        w.WriteLine("=================================");
                    }
                }

                foreach (SQL_Query sqlQuery in sqlQueryList)
                {
                    if (sqlQuery.JobEnabled.Equals("1"))
                    {
                        txtLogbox.AppendText("\r\n" + sqlQuery.JobId);

                        w.WriteLine(sqlQuery.JobId);
                        w.WriteLine(sqlQuery.JobEnabled);
                        w.WriteLine(sqlQuery.JobName);
                        w.WriteLine(sqlQuery.JobDescription);
                        w.WriteLine(sqlQuery.System);
                        w.WriteLine(sqlQuery.SubSystem);
                        w.WriteLine(sqlQuery.Source);
                        w.WriteLine(sqlQuery.Target);
                        w.WriteLine(sqlQuery.Server);
                        w.WriteLine(sqlQuery.Database);
                        w.WriteLine(sqlQuery.User);
                        w.WriteLine(sqlQuery.Psw);
                        w.WriteLine(sqlQuery.Query);
                        w.WriteLine("");
                        w.WriteLine("Query result:");
                        w.WriteLine(sqlQuery.Result);
                        w.WriteLine("=================================");

                    }
                }
            }

            txtLogbox.AppendText("\r\nJob complete.");
            txtLogbox.AppendText("\r\nResults saved at: " + outFile);
        }

        private void btnInFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtInFile.Text = openFileDialog1.FileName;
        }

        private void btnTrgtFold_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtTrgtPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnQFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtQFile.Text = openFileDialog1.FileName;
        }

        private void btnWriteReport_Click(object sender, EventArgs e)
        {
            txtLogbox.AppendText("\r\nWriting report.");

            string reportFileName = "testReport.docx";

            if (!String.IsNullOrEmpty(txtReportFile.Text))
                reportFileName = txtReportFile.Text;

            string reportFilePath = Path.Combine(targetFolder, reportFileName);

            WordWriter writer = new WordWriter();

            string defaultFileName = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\IKAMR-Noark5-C-rapportmal_v1.2.0_2020-01-22.docx";

            if (File.Exists(defaultFileName))
            {
                writer.WriteToDoc(defaultFileName, queryList, reportFilePath);

                txtLogbox.AppendText("\r\nReport complete. File saved at: " + reportFilePath);
            }else
                txtLogbox.AppendText("\r\nReport file does not exist.");
        }
    }

    public static class Globals
    {
        public static readonly String toolName = "KDRS Query";
        public static readonly String toolVersion = "0.2";
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
