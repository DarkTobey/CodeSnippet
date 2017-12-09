using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.Impl
{
    public class Sleep : Interface.ISleep
    {
        public DateTime Time()
        {
            return DateTime.Now;
        }
    }
}
