using MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MVC
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Server().Start("127.0.0.1", 88);

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
