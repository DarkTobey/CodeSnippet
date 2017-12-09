using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.Impl
{
    public class Coding : Interface.ICode
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
