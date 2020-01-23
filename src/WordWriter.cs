using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Core;

namespace KDRS_Query
{
    class WordWriter
    {
        public void WriteToDoc(string fileName, Dictionary<string,string> queryResults)
        {
            Application wordApp = new Application();
            Documents documents = wordApp.Documents;
            Document document = documents.Open(fileName);

            Tables tables = document.Tables;


            //Table antArk = tables[5];

            Table antArk = getTable("AntArk", tables);

            Table antArkDel = getTable("AntArkDel", tables);

            
            antArk.Columns[2].Cells[3].Range.Text = "Antall arkiver skal settes inn her";
            antArkDel.Columns[2].Cells[3].Range.Text = "Antall arkivdeler skal settes inn her";

            document.SaveAs2(@"C:\developer\c#\kdrs_query\KDRS_Query\doc\testReport.docx");

            //document.Close();
            documents.Close();

            wordApp.Quit();
        }

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
