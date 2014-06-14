using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CommonLib;
using System.Transactions;

namespace TransactionControl
{
    /// <summary>
    /// Transaction及TransactionScope控制範例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("start to update");

                //TraditionalTransaction();
                ControlByTransactionScope();

                Console.WriteLine("done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 傳統Transaction, 需與connection關聯
        /// </summary>
        private static void TraditionalTransaction()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {              
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd1 = new SqlCommand("insert into users values ('user1', 'pass')", conn, tran);
                        SqlCommand cmd2 = new SqlCommand("insert into users values ('user2', 'pass')", conn, tran);
                        SqlCommand cmd3 = new SqlCommand("insert into users_AAA values ('user3', 'pass')", conn);

                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        cmd3.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw;
                    }
                }                
            }
        }

        /// <summary>
        /// TransactionScope, 不須與connection關聯即可使用
        /// 注意TransactionScopeOption有三種選項, 請完全瞭解後再行使用
        /// </summary>
        private static void ControlByTransactionScope()
        {
            //需引用System.Transaction.dll
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Insert1();
                    Insert2();
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private static void Insert1()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into users values ('user4', 'pass')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        private static void Insert2()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into users_AAA values ('user5', 'pass')", conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
