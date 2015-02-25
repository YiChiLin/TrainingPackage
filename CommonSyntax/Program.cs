using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonSyntax
{
    class Program
    {
        static void Main(string[] args)
        {
            //宣告變數p1為Person類別的一個新實體
            Person p1 = new Person();
            p1.Age = 27;
            p1.Name = "Brett";
            p1.SetHeight(167);
            string strP1 = p1.PrintMe();
            Console.WriteLine(strP1);

            //宣告變數p2為Person類別的另一個新實體
            Person p2 = new Person(DateTime.MaxValue);
            p2.Age = 16;
            p2.Name = "Yu";
            p2.SetHeight(160);
            Console.WriteLine(p2.PrintMe());            

            //static類別於程式啟動時就會建立實體, 故此處不須再建立新實體即可使用該類別的method
            string strStaticP = StaticPerson.PrintMe();
            Console.WriteLine(strStaticP);

            Console.WriteLine();
            
            //傳值呼叫, 原始值不會變更
            int numA = 5;
            Console.WriteLine(string.Format("before call by value, numA = {0}", numA));
            CallByValue(numA);
            Console.WriteLine(string.Format("after call by value, numA = {0}", numA));

            //傳參考呼叫, 原始值會被乘以100倍
            int numB = 7;
            Console.WriteLine(string.Format("before call by ref, numB = {0}", numB));
            CallByReference(ref numB);
            Console.WriteLine(string.Format("after call by ref, numB = {0}", numB));

            Console.WriteLine();
            
            decimal result = 0; 
            try
            {
                //傳入0或負數的時候會拋出exception
                result = DivideByNum(10);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                //發生錯誤時執行
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //不論try區塊中的程式碼是否執行完成, 最後都會執行finally區塊
                result = -1;
            }
            Console.WriteLine(result);


            Console.ReadKey();
        }

        /// <summary>
        /// 傳值呼叫, 在method中的變更[不會]改動傳入參數的原始值
        /// </summary>
        /// <param name="num"></param>
        private static void CallByValue(int num)
        {
            num = num * 100;
        }

        /// <summary>
        /// 傳參考呼叫, 在method中的變更[會]改動傳入參數的原始值
        /// </summary>
        /// <param name="num"></param>
        private static void CallByReference(ref int num)
        {
            num = num * 100;
        }

        private static decimal DivideByNum(int num)
        {
            if (num < 0)
            {
                throw new Exception("傳入值須大於0");
            }

            decimal result = Convert.ToDecimal( 1000 / num);
            return result;
        }
    }
}
