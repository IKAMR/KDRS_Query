using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KDRS_Query
{
    public partial class Form1 : Form
    {

        List<QueryClass> queryList = new List<QueryClass>();

        XPathQueryRunner xPRunner = new XPathQueryRunner();

        public Form1()
        {
            InitializeComponent();
            Text = Globals.toolName + " " + Globals.toolVersion;
        }


        private string GetQuery(string filename)
        {
            Console.WriteLine("Reading queries");


            using (StreamReader reader = File.OpenText(filename))
            {
                String line;
                while (((line = reader.ReadLine()) != null))
                {
                    Console.WriteLine(line);


                    if (line.StartsWith("["))
                    {
                        string qType = line.Split('[', ']')[1];

                        switch (qType)
                        {
                            case "XML_QUERY":
                                //Call fill xml_query
                                //make xml
                                break;
                        }
                        XML_Query query = new XML_Query();

                        queryList.Add(query);

                        string qStart = @"#START#";
                        string qStop = @"#STOP#";
                        string queryText = "";
                        while ((line = reader.ReadLine().Trim()) != null)
                        {

                            string[] items = line.Split('=');
                            switch (items[0])
                            {
                                case "job_id":
                                    query.JobId = items[1];
                                    break;
                                case "job_enabled":
                                    query.JobEnabled = Int32.Parse(items[1]);
                                    break;
                                case "job_name":
                                    query.JobName = items[1];
                                    break;
                                case "job_description":
                                    query.JobDescription = items[1];
                                    break;
                                case "system":
                                    query.System = items[1];
                                    break;
                                case "subsystem":
                                    query.SubSystem = items[1];
                                    break;
                                case "source":
                                    query.Source = items[1];
                                    break;
                                case "target":
                                    query.Target = items[1];
                                    break;
                            }
                            if (line.Equals(qStart))
                            {
                                Console.WriteLine("START found");

                                while (!(line = reader.ReadLine()).Equals(qStop))
                                {
                                    queryText += "\r\n" + line;
                                }
                                query.Query = queryText;
                                break;
                            }
                        }
                    }
                }


            }
            Console.WriteLine("ALL queries read");
            return "";

        }

        private void btnRunQ_Click(object sender, EventArgs e)
        {
            txtLogbox.Text = "Running queries";
            string inFile = txtInFile.Text;
            string targetFolder = txtTrgtPath.Text;
             string queryFile = txtQFile.Text;
            //string queryFile = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\test.txt";
            Console.WriteLine("Reading queries from: " + inFile);

            GetQuery(queryFile);

            string outFile = Path.Combine(targetFolder, "results.txt");
            //string outFile = @"Y:\arkiv-test\sip\documaster\sample-extraction\2016-09-27_11-22-42-000333\uttrekk\results.txt";

            //File.Create(outFile);
            xPRunner.RunXPath(queryList, inFile);

            using (File.Create(outFile)) { }

            using (StreamWriter w = File.AppendText(outFile))
            {

                foreach (XML_Query query in queryList)
                {
                    txtLogbox.AppendText("\r\n" + query.JobId);

                    w.WriteLine(query.JobId);
                    w.WriteLine(query.JobEnabled.ToString());
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


                    Console.WriteLine(query.JobId);
                    Console.WriteLine(query.Query);
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


    }

    public static class Globals
    {
        public static readonly String toolName = "KDRS Query";
        public static readonly String toolVersion = "0.1";
    }

    public enum ClassType { XML_Query, SQL_Query }

    public class QueryClass
    {
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
        public string JobId { get; set; }
        public int JobEnabled { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Query { get; set; }
        public string Result { get; set; }
    }
    public class SQL_Query : QueryClass
    {
        public string JobId { get; set; }
        public int JobEnabled { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Psw { get; set; }
        public string Query { get; set; }
    }
}
