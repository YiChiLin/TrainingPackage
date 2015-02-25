using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonSyntax
{
    public static class StaticPerson
    {
        public static string Name = "Static Person";

        public static string PrintMe()
        {
            return string.Format("My name is {0}", Name);
        }

    }
}
