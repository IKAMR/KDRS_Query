using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KDRS_Query
{
    class ExcelWriter
    {
        // Writes queries to the excel template log file.
        public void WriteToLog(string filename, List<QueryClass> queryList, string logFileName)
        {
            Console.WriteLine("writing log");
            Application xlApp1 = new Application
            {
                DecimalSeparator = ".",
                UseSystemSeparators = false
            };

            Workbooks xlWorkbooks = xlApp1.Workbooks;
            Workbook xlWorkBook = xlWorkbooks.Open(filename);
            Sheets xlWorksheets = xlWorkBook.Worksheets;

            object misValue = System.Reflection.Missing.Value;

            Worksheet xlWorksheet = xlWorksheets.get_Item(1);

            Range idRange = null;

            try
            {
                foreach (QueryClass q in queryList)
                {
                    Console.WriteLine("jobId " + q.JobId);
                    idRange = xlWorksheet.Range["A1:A121"];
                    int cellRow = getCell(q.JobId, idRange);
                    if (cellRow != 0 && q.JobEnabled.Equals("1"))
                    {
                        xlWorksheet.Range["E" + cellRow].Value = q.Result.Replace("\r\n", "\v");
                    }
                }

                if (String.IsNullOrEmpty(logFileName))
                    logFileName = @"C:\developer\c#\kdrs_query\KDRS_Query\doc\testReport.xlsx";
                
                xlWorkBook.SaveAs(logFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

                GC.Collect();
                GC.WaitForPendingFinalizers();


                Marshal.ReleaseComObject(idRange);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorksheets);

                xlWorkBook.Close();
                Marshal.ReleaseComObject(xlWorkBook);

                xlApp1.Quit();
                Marshal.ReleaseComObject(xlApp1);

            }
        }
        //******************************************************************

        // Get coordinates of cell with content.
        public int getCell(string cellContent, Range column)
        {

            foreach (Range r in column)
            {
                if (r.Value == cellContent)
                {
                    Marshal.ReleaseComObject(column);

                    return r.Row;
                }
            }
            Marshal.ReleaseComObject(column);

            return 0;
        }
    }
}
