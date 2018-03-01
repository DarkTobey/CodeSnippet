using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOP
{
    public class DoProxy : IDo
    {
        private IDo _Obj;

        public DoProxy(IDo obj)
        {
            _Obj = obj;
        }

        public void DoSomething()
        {
            try
            {
                OnBefore();
                _Obj.DoSomething();
                OnAfter();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }


        public void OnBefore()
        {
            Console.WriteLine("OnBefore do");
        }

        public void OnAfter()
        {
            Console.WriteLine("OnAfter do");
        }

        public void OnException(Exception ex)
        {
            Console.WriteLine("发生异常，" + ex.Message);
        }

    }
}
