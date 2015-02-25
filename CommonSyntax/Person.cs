using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonSyntax
{
    public class Person
    {
        #region 建構子

        /// <summary>
        /// 無參數的建構子
        /// </summary>
        public Person()
        {
            _createTime = DateTime.Now;
        }

        /// <summary>
        /// 有參數的建構子
        /// </summary>
        /// <param name="time"></param>
        public Person(DateTime time)
        {
            _createTime = time;
        }

        private DateTime _createTime;

        #endregion


        #region 無屬性寫法

        /// <summary>
        /// field
        /// </summary>
        private int _height;

        /// <summary>
        /// Set Method
        /// </summary>
        /// <param name="height"></param>
        public void SetHeight(int height)
        {
            _height = height;
        }

        /// <summary>
        /// Get Method
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return _height;
        }

        #endregion


        #region 自訂寫法

        /// <summary>
        /// field
        /// </summary>
        private int _age;
        
        /// <summary>
        /// Property
        /// </summary>
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                //可加入驗證或轉換邏輯
                if (value < 0)
                {
                    _age = 0;
                }                    
                else
                {
                    _age = value;
                }
            }
        }        

        /// <summary>
        /// Readonly Property
        /// </summary>
        public bool IsAdult
        {
            get
            {
                return _age >= 18;
            }
        }

        #endregion


        #region 簡潔寫法 (c#專用)

        /// <summary>
        /// 簡寫的Property, .Net會自動生成private field
        /// </summary>
        public string Name { get; set; }

        #endregion


        public string PrintMe()
        {
            int height = GetHeight();
            string str = string.Format("my name is {0}, age is {1}, height is {2}, created at {3}",
                                        Name, Age, height, _createTime.ToString("yyyy-MM-dd HH:mm:ss"));

            return str;
        }

        protected void ProtectedMethod()
        { 
        
        }
    }

    public class Child : Person
    {
        public void ChildMethod()
        {
            base.ProtectedMethod();
        }
    }
}
