using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThrowException
{
    /// <summary>
    /// Throw及Throw ex差別範例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MethodThrow();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Throw:");
                Console.WriteLine(ex);
            }

            Console.WriteLine("");
            Console.WriteLine("");

            try
            {
                MethodThrowEx();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Throw Ex:");
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 使用單純的throw, 不會重置錯誤堆疊
        /// </summary>
        private static void MethodThrow()
        {
            try
            {
                MethodA();
            }
            catch (Exception ex)
            {
                //WriteLog(ex);  本行為模擬寫log, 切忌catch exception卻不做任何事
                throw;
            }
        }

        /// <summary>
        /// 使用Throw ex, 錯誤堆疊將從此Method重新開始
        /// </summary>
        private static void MethodThrowEx()
        {
            try
            {
                MethodA();
            }
            catch (Exception ex)
            {
                //WriteLog(ex);  本行為模擬寫log, 切忌catch exception卻不做任何事
                throw ex;
            }
        }

        /// <summary>
        /// 倒數第二層
        /// </summary>
        private static void MethodA()
        {
            MethodB();
        }

        /// <summary>
        /// 最底層
        /// </summary>
        private static void MethodB()
        {
            throw new Exception("i am an error");
        }
    }
}
