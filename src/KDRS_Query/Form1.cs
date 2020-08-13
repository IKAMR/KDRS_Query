﻿using System;
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


        Query query = new Query();
        XPathQueryRunner xPRunner = new XPathQueryRunner();
        MYSQL_Runner sqlRunner = new MYSQL_Runner();

        string targetFolder;

        public Form1()
        {
            InitializeComponent();
            Text = Globals.toolName + " " + Globals.toolVersion;
        }

       


     

        private void btnRunQ_Click(object sender, EventArgs e)
        {
            txtLogbox.Text = "Running queries";
            string inFile = txtInFile.Text;
            targetFolder = txtTrgtPath.Text;

            queryList.Clear();
            sqlQueryList.Clear();

             string queryFile = txtQFile.Text;

           // if (String.IsNullOrEmpty(queryFile))
             //   queryFile = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\xml_queries.txt";

            Console.WriteLine("Reading queries from: " + inFile);

            query.GetQuery(queryFile);

            string outFile = Path.Combine(targetFolder, "kdrs_query_results.txt");

            queryList = query.QueryList;

            xPRunner.RunXPath(queryList, inFile);

            sqlQueryList = query.SqlQueryList;

            foreach (SQL_Query sql_Query in sqlQueryList)
            {
                if (sql_Query.JobEnabled.Equals("1"))
                {
                    sqlRunner.RunSQL(sql_Query);
                }
            }
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

        private void btnInFold_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtInFile.Text = folderBrowserDialog1.SelectedPath;
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

        private void btnChooseReportTemplate_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
                txtReportTempFile.Text = openFileDialog1.FileName;
        }

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

        private void btnReset_Click(object sender, EventArgs e)
        {

        }
    }

    public static class Globals
    {
        public static readonly String toolName = "KDRS Query";
        public static readonly String toolVersion = "0.2";
    }

    
}
