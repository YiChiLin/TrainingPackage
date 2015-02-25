using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CommonLib;

namespace RelatedDataTable
{
    /// <summary>
    /// DataTable與DataRelation範例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DataSet ds = GetData();
            ComputeSum(ds);            
            //ComputeSumByLinq(ds);

            Console.WriteLine();
            Console.WriteLine();
            PrintCustomerOrder();

            Console.ReadKey();
        }

        /// <summary>
        /// 以DataRelation實作DataTable中的Sum運算
        /// </summary>
        /// <param name="ds"></param>
        private static void ComputeSum(DataSet ds)
        {
            DataTable orderDt = ds.Tables["OrderDetail"];

            DataTable sumDt = new DataTable();
            sumDt.TableName = "GruopSum";
            sumDt.Columns.Add("ProductId", typeof(int));
            sumDt.Columns.Add("Discount", typeof(decimal));
            sumDt.Columns.Add("Sum", typeof(int));

            //寫入group by的兩個欄位
            foreach (DataRow dr in orderDt.Rows)
            {
                if (sumDt.Select(string.Format("productId = {0} and discount = {1}", dr["ProductId"], dr["Discount"])).Length == 0)
                {
                    sumDt.Rows.Add(new object[] { dr["ProductId"], dr["Discount"] });
                }
            }

            ds.Tables.Add(sumDt);

            //指定group by欄位對應, 加入relation
            ds.Relations.Add("GroupSumRelation",
                              new DataColumn[] { sumDt.Columns["ProductId"], sumDt.Columns["Discount"] },
                              new DataColumn[] { orderDt.Columns["ProductId"], orderDt.Columns["Discount"] });

            //指定彙總函數運算使用的relation及欄位
            sumDt.Columns["Sum"].Expression = "Sum(Child(GroupSumRelation).Quantity)";            

            PrintData.PrintDataSet(ds);
        }

        /// <summary>
        /// 使用linq運算, 版本限於.net 3.5以上
        /// </summary>
        /// <param name="ds"></param>
        private static void ComputeSumByLinq(DataSet ds)
        {
            var query = ds.Tables["OrderDetail"].AsEnumerable()
                          .GroupBy(data => new { ProductId = data["ProductId"], Discount = data["Discount"] })
                          .Select(p => new
                          {
                              ProductId = p.Key.ProductId,
                              Discount = p.Key.Discount,
                              GroupSum = p.Sum(q => Convert.ToInt32(q["Quantity"]))
                          });

            Console.WriteLine("ProductId, Discount, Sum");
            foreach (var group in query)
            {
                Console.WriteLine("{0}, {1}, {2}",
                    group.ProductId, group.Discount, group.GroupSum);
            }
        }

        /// <summary>
        /// 以relation實作兩DataTable Join及select
        /// </summary>
        private static void PrintCustomerOrder()
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                string sqlString = @"select CustomerID from Customers";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dt.TableName = "Customer";
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                }

                sqlString = @"select OrderID, CustomerID, OrderDate from Orders";
                cmd = new SqlCommand(sqlString, conn);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dt.TableName = "Orders";
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                }
            }

            DataTable customerDt = ds.Tables[0];
            DataTable orderDt = ds.Tables[1];

            ds.Relations.Add("CustomerOrderRelation",
                              customerDt.Columns["CustomerID"],
                              orderDt.Columns["CustomerID"]);

            if (customerDt.Rows.Count > 0)
            {
                //只取第一筆客戶資料
                DataRow customerDr = customerDt.Rows[0];                
                Console.WriteLine(string.Format("CustomerId:{0}", customerDr["CustomerId"].ToString()));

                //主table為parent, join的table為child
                foreach (DataRow orderDataRow in customerDr.GetChildRows("CustomerOrderRelation"))
                {
                    Console.WriteLine(string.Format("OrderID:{0}, OrderDate:{1}",
                                      orderDataRow["OrderID"].ToString(),
                                      Convert.ToDateTime(orderDataRow["OrderDate"]).ToString("yyyy-MM-dd")));
                }
            }
        }

        /// <summary>
        /// 取得彙總運算範例所需的訂單明細資料
        /// </summary>
        /// <returns></returns>
        private static DataSet GetData()
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                string sqlString = @"select ProductID, Discount, Quantity from [Order Details] 
                                     where Quantity >= 110 
                                     order by productid, Discount, Quantity ";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dt.TableName = "OrderDetail";
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }
    }
}
