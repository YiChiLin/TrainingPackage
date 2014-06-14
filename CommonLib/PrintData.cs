using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CommonLib
{
    public static class PrintData
    {
        /// <summary>
        /// print出DataSet中所有DataTable的欄位值
        /// </summary>
        /// <param name="ds"></param>
        public static void PrintDataSet(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
            {
                PrintDataTable(dt);

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// print出DataTable的所有欄位值
        /// </summary>
        /// <param name="dt"></param>
        public static void PrintDataTable(DataTable dt)
        {
            Console.WriteLine(string.Format("Table Name: {0}", dt.TableName));
            foreach (DataColumn dc in dt.Columns)
            {
                Console.Write(dc.ColumnName);
                Console.Write(", ");
            }
            Console.WriteLine();

            foreach (DataRow dr in dt.Rows)
            {
                foreach (object o in dr.ItemArray)
                {
                    Console.Write(o);
                    Console.Write(", ");
                }
                Console.WriteLine();
            }
        }
    }
}
