using MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string path = Console.ReadLine();
                if (path.ToLower().Equals("quit")) break;

                IResult result = Dispatcher.Instance().ResolveRoute(path);
                Console.WriteLine(result.GetResult());
            }

            Console.ReadLine();
        }
    }
}
