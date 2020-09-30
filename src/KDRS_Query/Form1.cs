using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace KDRS_Query
{
    public partial class Form1 : Form
    {

        List<QueryClass> queryList = new List<QueryClass>();
        List<SQL_Query> sqlQueryList = new List<SQL_Query>();

        Query query = new Query();
        XPathQueryRunner xPRunner = new XPathQueryRunner();
        MYSQL_Runner sqlRunner = new MYSQL_Runner();

        string targetFolder;
        string queryFile;
        string inFile;
        bool cleanOut;

        //******************************************************************
        public Form1()
        {
            InitializeComponent();
            Text = Globals.toolName + " " + Globals.toolVersion;
        }

        //******************************************************************
        private void btnRunQ_Click(object sender, EventArgs e)
        {
            txtLogbox.Text = "Running queries";
            inFile = txtInFile.Text;
            targetFolder = txtTrgtPath.Text;

            cleanOut = chkBox_cleanOut.Checked;

            queryList.Clear();
            sqlQueryList.Clear();

            btnChooseReportTemplate.Enabled = false;
            btnInFile.Enabled = false;
            btnQFile.Enabled = false;
            btnReset.Enabled = false;
            btnRunQ.Enabled = false;
            btnTrgtFold.Enabled = false;
            btnWriteReport.Enabled = false;

            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();

        }

        //******************************************************************
        private void btnInFold_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtInFile.Text = folderBrowserDialog1.SelectedPath;
        }

        //******************************************************************
        private void btnTrgtFold_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtTrgtPath.Text = folderBrowserDialog1.SelectedPath;
        }

        //******************************************************************
        private void btnQFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtQFile.Text = openFileDialog1.FileName;
        }

        //******************************************************************
        private void btnChooseReportTemplate_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtReportTempFile.Text = openFileDialog1.FileName;
        }

        //******************************************************************
        private void btnWriteReport_Click(object sender, EventArgs e)
        {
            txtLogbox.AppendText("\r\nWriting report.");

            string reportFileName = "testReport.docx";

            if (!String.IsNullOrEmpty(txtReportFile.Text))
                reportFileName = txtReportFile.Text;

            string reportFilePath = Path.Combine(targetFolder, reportFileName);

            WordWriter writer = new WordWriter();

            string defaultFileName = txtReportTempFile.Text;

            //    @"C:\developer\c#\kdrs_query\KDRS_Query\doc\IKAMR-Noark5-C-rapportmal_v1.2.0_2020-01-22.docx";

            if (File.Exists(defaultFileName))
            {
                writer.WriteToDoc(defaultFileName, queryList, reportFilePath);

                txtLogbox.AppendText("\r\nReport complete. File saved at: " + reportFilePath);
            }else
                txtLogbox.AppendText("\r\nReport file does not exist.");
        }


        //******************************************************************
        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            queryFile = txtQFile.Text;

            // if (String.IsNullOrEmpty(queryFile))
            //   queryFile = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\xml_queries.txt";

            Console.WriteLine("Reading queries from: " + inFile);

            query.GetQuery(queryFile);
            backgroundWorker1.ReportProgress(0, "Queries read from file");
                       
            queryList = query.QueryList;

            xPRunner.OnProgressUpdate += query_OnProgressUpdate;
            xPRunner.RunXPath(queryList, inFile);

            sqlQueryList = query.SqlQueryList;

            xPRunner.OnProgressUpdate += query_OnProgressUpdate;
            foreach (SQL_Query sql_Query in sqlQueryList)
            {
                if (sql_Query.JobEnabled.Equals("1"))
                {
                    sqlRunner.RunSQL(sql_Query);
                }
            }
        }

        //******************************************************************
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtLogbox.AppendText("\r\n" + e.UserState.ToString());
        }

        //******************************************************************
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string outFile = Path.Combine(targetFolder, "kdrs_query_results_" + Path.GetFileNameWithoutExtension(inFile) + ".txt");

            writeResultsToFile(outFile, cleanOut);

            txtLogbox.AppendText("\r\nJob complete.");
            txtLogbox.AppendText("\r\nResults saved at: " + outFile);

            btnChooseReportTemplate.Enabled = true;
            btnInFile.Enabled = true;
            btnQFile.Enabled = true;
            btnReset.Enabled = true;
            btnRunQ.Enabled = true;
            btnTrgtFold.Enabled = true;
            btnWriteReport.Enabled = true;
        }

        private void writeResultsToFile(string outFile, bool cleanOut)
        {
            using (File.Create(outFile)) { }

            // Creating text file containing all query info including query results
            using (StreamWriter w = File.AppendText(outFile))
            {
                w.WriteLine("Query file: " + queryFile);
                w.WriteLine("");
                w.WriteLine("=================================");
                w.WriteLine("");

                foreach (XML_Query query in queryList)
                {
                    if (query.JobEnabled.Equals("1") || query.JobEnabled.Equals("2"))
                    {
                        txtLogbox.AppendText("\r\n" + query.JobId);

                        w.WriteLine(query.JobId);

                        if (!cleanOut)
                        {
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
                        }
                        else
                        { w.WriteLine("");

                        }

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
        }

        //******************************************************************
        private void query_OnProgressUpdate(string queryId)
        {
            base.Invoke((System.Action)delegate
            {
                backgroundWorker1.ReportProgress(0, queryId);
            });
        }

    }

    //====================================================================================================
    public static class Globals
    {
        public static readonly String toolName = "KDRS Query";
        public static readonly String toolVersion = "0.4";
    }
}
