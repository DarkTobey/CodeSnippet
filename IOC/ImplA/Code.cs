using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.ImplA
{
    public class Code : Interface.ICode
    {
        public DateTime Time()
        {
            return DateTime.Now;
        }

        public string DoSomething()
        {
            return "Coding";
        }
    }
}
