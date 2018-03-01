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
    public class Client
    {
        public void Start(string listenAddress, int listenPort, string connectAddress, int connectPort)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(listenAddress), listenPort);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Bind(endPoint);

            client.Connect(new IPEndPoint(IPAddress.Parse(connectAddress), connectPort));

            Thread thread = new Thread(Reciver);
            thread.IsBackground = true;
            thread.Start(client);
            Console.WriteLine($"服务已经启动，正在监听{listenAddress}:{listenPort},并且试图连接{connectAddress}:{connectPort}");

            while (true)
            {
                string cmd = Console.ReadLine();
                client.Send(System.Text.Encoding.UTF8.GetBytes(cmd));
                if (cmd.Equals("close"))
                {
                    client.Disconnect(true);
                    break;
                }
            }
        }


        public void Reciver(object obj)
        {
            Socket server = obj as Socket;
            //准备接收通讯数据
            while (true)
            {
                byte[] buffer = new byte[1 * 1024 * 1024];
                int lenth = server.Receive(buffer);
                if (lenth == 0) continue;

                string text = System.Text.Encoding.UTF8.GetString(buffer, 0, lenth);
                Console.WriteLine($"收到来自{server.RemoteEndPoint.ToString()}的数据：{text}");
            }
        }
    }
}
