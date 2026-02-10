using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Threads.sockets
{
    internal class Passive
    {
        public static void Run()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, 1024);

            s.Bind(endPoint);
            s.Listen(10);

            try
            {
                while (true)
                {
                    Console.WriteLine("Server listing on PORT 1024");
                    Socket newSocket = s.Accept();
                    
                    Console.WriteLine(newSocket.RemoteEndPoint.ToString());
                    newSocket.Send(System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString()));
                    newSocket.Shutdown(SocketShutdown.Both);
                    newSocket.Close();
                }
            } catch(SocketException ex)
            {
                Console.WriteLine(ex.ToString());
            } finally
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }
        }
    }
}
