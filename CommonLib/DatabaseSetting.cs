using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public static class DatabaseSetting
    {
        /// <summary>
        /// 資料庫連線字串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return "Data Source=localhost;Initial Catalog=Northwind;Persist Security Info=True;User ID=sa;Password=pass123;";
            }
        }

    }
}
