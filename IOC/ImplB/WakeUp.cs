using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.ImplB
{
    public class WakeUp : Interface.IWakeUp
    {
        public string Name()
        {
            return "XD";
        }

        public DateTime Time()
        {
            return DateTime.UtcNow;
        }
    }
}
