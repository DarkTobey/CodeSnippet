using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOP
{
    public class Program
    {
        static void Main(string[] args)
        {
            DoProxy proxy = new DoProxy(new Do1());
            proxy.DoSomething();

            Console.ReadLine();
        }
    }

    public class Do1 : IDo
    {
        public void DoSomething()
        {
            Console.WriteLine("Do1 do");
            throw new Exception("异常测试");
        }
    }

}
