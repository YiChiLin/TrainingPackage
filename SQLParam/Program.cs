using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CommonLib;

namespace SQLParam
{
    /// <summary>
    /// SQL Injection範例及DbParameter(SqlParameter)範例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //輸入 1. 『' or 1=1;--』
            //     2. 『';delete from users;--』
            //     3. 『';drop database users;--』
            Console.Write("UserId:");
            string userId = Console.ReadLine();
            Console.Write("Password:");
            string password = Console.ReadLine();

            Console.WriteLine();            

            //bool pass = CheckUser(userId, password);
            bool pass = CheckUserParam(userId, password);
            Console.WriteLine(string.Format("passed:{0}", pass));
            Console.ReadKey();
        }

        /// <summary>
        /// 會遭受sql injection攻擊的範例
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool CheckUser(string userId, string password)
        {
            bool pass = false;
            string sqlString = "select * from users where userId = '{0}' and password = '{1}' ";
            sqlString = string.Format(sqlString, userId, password);

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {                
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    pass = true;
                }
            }

            return pass;
        }

        /// <summary>
        /// 使用SqlParamter防止sql injection攻擊的範例
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool CheckUserParam(string userId, string password)
        {
            bool pass = false;
            string sqlString = "select * from users where userId = @userId and password = @password ";
            sqlString = string.Format(sqlString, userId, password);

            using (SqlConnection conn = new SqlConnection(DatabaseSetting.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                cmd.Parameters.Add(new SqlParameter("@password", password));

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    pass = true;
                }
            }

            return pass;
        }
    }
}
