using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Threads.async_sockets
{
    internal class Server
    {
        delegate void ConnectDelegate(Socket s);
        delegate void StartNetwork(Socket s);

        Socket socket;
        IPEndPoint endPoint;

        public Server(string address, int port)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }

        void ServerConnect(Socket s)
        {
            string httpResponse = "HTTP/1.1 200 OK\r\n" +
                         "Content-Type: text/plain; charset=utf-8\r\n" +
                         "Content-Length: 19\r\n" +
                         "\r\n" +
                         DateTime.Now.ToString();

            byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);
            s.Send(responseBytes);
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }

        void ServerBegin(Socket s)
        {
            while (true)
            {
                try
                {
                    Socket newSocket = s.Accept();
                    Console.WriteLine(newSocket.RemoteEndPoint.ToString());
                    ConnectDelegate cd = new ConnectDelegate(ServerConnect);
                    cd.BeginInvoke(newSocket, null, null);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(endPoint);
            socket.Listen(10);

            StartNetwork start = new StartNetwork(ServerBegin);
            start.BeginInvoke(socket, null, null);
        }

        public void Stop()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
