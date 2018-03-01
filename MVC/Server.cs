using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MVC
{
    public class Server
    {
        public void Start(string address, int port, int maxListen = 20)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
            Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketServer.Bind(endPoint);
            socketServer.Listen(20);

            Thread thread = new Thread(Listener);
            thread.IsBackground = true;
            thread.Start(socketServer);
            Console.WriteLine($"服务已经启动，正在监听{address}:{port}");
        }


        public void Listener(object obj)
        {
            Socket server = obj as Socket;
            //准备接收连接
            while (true)
            {
                Socket sender = server.Accept();
                Console.WriteLine($"{sender.RemoteEndPoint.ToString()}连上了");

                Thread thread = new Thread(Reciver);
                thread.IsBackground = true;
                thread.Start(sender);
            }
        }


        public void Reciver(object obj)
        {
            Socket sender = obj as Socket;
            //准备接收通讯数据
            while (true)
            {
                byte[] buffer = new byte[1 * 1024 * 1024];
                int lenth = sender.Receive(buffer);
                if (lenth == 0) continue;

                string text = System.Text.Encoding.UTF8.GetString(buffer, 0, lenth);
                Console.WriteLine($"收到来自{sender.RemoteEndPoint.ToString()}的数据：{text}");

                sender.Send(System.Text.Encoding.UTF8.GetBytes("ok"));
            }
        }
    }
}
