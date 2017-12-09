using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.ImplB
{
    public class Sleep : Interface.ISleep
    {
        public DateTime Time()
        {
            return DateTime.UtcNow;
        }
    }
}
