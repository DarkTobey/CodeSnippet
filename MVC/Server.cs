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
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(endPoint);
            server.Listen(maxListen);

            Thread thread = new Thread(Listener);
            thread.IsBackground = true;
            thread.Start(server);
            Console.WriteLine($"服务已经启动，正在监听{address}:{port}");
        }


        public void Listener(object obj)
        {
            Socket server = obj as Socket;
            //准备接收连接
            while (true)
            {
                Socket client = server.Accept();
                Console.WriteLine($"客户端{client.RemoteEndPoint.ToString()}连上了");

                Thread thread = new Thread(Reciver);
                thread.IsBackground = true;
                thread.Start(client);
            }
        }


        public void Reciver(object obj)
        {
            Socket client = obj as Socket;
            //准备接收通讯数据
            while (true)
            {
                byte[] buffer = new byte[1 * 1024 * 1024];
                int lenth = client.Receive(buffer);
                if (lenth == 0) continue;

                string text = System.Text.Encoding.UTF8.GetString(buffer, 0, lenth);
                Console.WriteLine($"收到来自{client.RemoteEndPoint.ToString()}的数据：{text}");

                string result = "这是一段测试数据";
                byte[] body = System.Text.Encoding.UTF8.GetBytes(result);
                client.Send(HttpHeader(body.Length).Concat(body).ToArray());
            }
        }

        public byte[] HttpHeader(int bodyLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HTTP/1.1 200 OK\r\n");
            sb.Append("Content-Type:text/html; charset=utf-8\r\n");
            sb.Append($"Content-Length:{bodyLength}\r\n");
            sb.Append("\r\n");

            return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
