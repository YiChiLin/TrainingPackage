using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CommonLib;

namespace DataObject
{
    /// <summary>
    /// ADO.Net資料物件範例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DataSet ds;
            ds = GetDataByAdapter();
            //ds = GetDataByReader();

            //UpdateProductName(ds);
            PrintData.PrintDataSet(ds);

            //BulkCopy(ds);

            Console.ReadKey();
        }

        /// <summary>
        /// 使用DataReader取得資料
        /// </summary>
        /// <returns></returns>
        private static DataSet GetDataByReader()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "Table1";
            ds.Tables.Add(dt);

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                conn.Open();
                string sqlString = "select TitleOfCourtesy, LastName from Employees";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        dt.Rows.Add(new List<object>() { dr["TitleOfCourtesy"], dr["LastName"] });
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 使用DataAdapter取得資料
        /// </summary>
        /// <returns></returns>
        private static DataSet GetDataByAdapter()
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                string sqlString = "select top 5 productId, productName from products order by productid ";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dt.TableName = "Table1";
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                }

                sqlString = "select top 5 CustomerID, City from Customers ";
                cmd = new SqlCommand(sqlString, conn);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt2 = new DataTable();
                    dt2.TableName = "Table2";
                    da.Fill(dt2);
                    ds.Tables.Add(dt2);
                }
            }
            return ds;
        }

        /// <summary>
        /// 利用DataTable批次修改資料庫的值
        /// </summary>
        /// <param name="ds"></param>
        private static void UpdateProductName(DataSet ds)
        {
            Console.Write("please input new product name：");
            string productName = Console.ReadLine();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i]["productName"] = productName;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.UpdateCommand = new SqlCommand("update products set productname = @productName where productId = @productId", conn);
                    da.UpdateCommand.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar, 40, "productName"));
                    da.UpdateCommand.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int, 40, "productId"));

                    da.Update(ds.Tables[0]);
                }
            }

            Console.WriteLine("Update by DataAdapter done");
        }

        /// <summary>
        /// 使用SqlBulkCopy將DataTable的資料批次寫入資料庫中
        /// </summary>
        /// <param name="ds"></param>
        private static void BulkCopy(DataSet ds)
        {
            SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                conn.Open();

                bulkCopy.BatchSize = 1000;

                //設定目標table名稱
                bulkCopy.DestinationTableName = "products_copy";          
      
                //設定對應的欄位名稱, 注意大小寫需一致
                bulkCopy.ColumnMappings.Add("ProductId", "ProductId");
                bulkCopy.ColumnMappings.Add("ProductName", "ProductName");

                bulkCopy.WriteToServer(ds.Tables[0]);
            }

            Console.WriteLine("BulkCopy done");
        }
    }
}
