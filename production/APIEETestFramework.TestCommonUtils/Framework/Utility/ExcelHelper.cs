// **************************************************************************
// Copyright 2018 Honeywell International Sàrl
// **************************************************************************

using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Regression.TestCommonUtils.Framework.Utility
{
    public class ExcelHelper
    {
        public static void UpdateExcel(string WorkbookName, int SheetId, string[] UpdateData)
        {
            Application xlApp = new Application();
            //open the excel
            Workbook xlWorkbooK = xlApp.Workbooks.Open(@AppDomain.CurrentDomain.BaseDirectory + "\\TestData\\"+WorkbookName+".xlsx");   //"d:\range3.xlsx"
            //get the first sheet of the excel
            Worksheet xlWorkSheet = (Worksheet)xlWorkbooK.Worksheets.get_Item(SheetId);
            Range range = xlWorkSheet.UsedRange;
            int rowCount = range.Rows.Count;
            Console.WriteLine("Row count-->{0}", rowCount);
            int columnCount = range.Columns.Count;
            Console.WriteLine("Row count-->{0}", columnCount);
            // specify the rows
            for (int i = 1; i <= rowCount; i++)
            {
                //specify the columns
                for (int j = 1; j <= 2; j++)
                {

                    Range cell = range.Cells[i, j] as Range;

                    cell.Value = UpdateData[j-1];
                }
            }
            xlWorkbooK.Save();
            //release the resource
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkbooK);
            Marshal.ReleaseComObject(xlApp);
        }
     }
 }