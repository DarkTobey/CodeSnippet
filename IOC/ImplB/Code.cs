using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.ImplB
{
    public class Code : Interface.ICode
    {
        public string DoSomething()
        {
            return "don't want to code";
        }

        public DateTime Time()
        {
            return DateTime.UtcNow;
        }
    }
}
