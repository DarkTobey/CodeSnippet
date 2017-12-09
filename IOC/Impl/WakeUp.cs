using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.Impl
{
    public class WakeUp : Interface.IWakeUp
    {
        public string Name()
        {
            return "Tobey";
        }

        public DateTime Time()
        {
            return DateTime.Now;
        }
    }
}
