using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;

namespace KDRS_Query
{
    class WordWriter
    {
        // Writes query results to report template. Results are written to tables with name matching query jobId.
        public void WriteToDoc(string fileName, List<QueryClass> queryList, string reportFileName)
        {
            Application wordApp = new Application();
            Documents documents = wordApp.Documents;
            Document document = documents.Open(fileName);

            Tables tables = document.Tables;

            foreach(QueryClass q in queryList)
            {
                string tableName = "tbl_" + q.JobId;
                Table table = getTable(tableName, tables);
                if (table != null)
                {
                    table.Columns[2].Cells[3].Range.Text = q.Result.Replace("\r\n", "\v");
                }
            }

            if (String.IsNullOrEmpty(reportFileName))
                reportFileName = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\testReport.docx";

            document.SaveAs2(reportFileName);

            documents.Close();

            wordApp.Quit();
        }

        // Returns table with spesific title.
        public Table getTable(string tableTitle, Tables tables)
        {
            foreach(Table t in tables)
            {
                if (t.Title != null && t.Title.Equals(tableTitle))
                    return t;
            }
            return null;
        }


    }
}
