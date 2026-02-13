using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Threads
{
    internal class Program
    {
        private const string MULTICAST_IP = "224.5.5.5";
        private const int PORT = 4567;
        private static int INTERVAL = 1000;
        private static string message = "";

        static void Listener()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
                socket.Bind(endPoint);

                IPAddress multicastAddr = IPAddress.Parse(MULTICAST_IP);

                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddr, IPAddress.Any));

                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = socket.Receive(buffer);
                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("Получено: " + received);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void SendMessage()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

                IPAddress dest = IPAddress.Parse(MULTICAST_IP);
                IPEndPoint endPoint = new IPEndPoint(dest, PORT);

                socket.Connect(endPoint);

                socket.Send(Encoding.UTF8.GetBytes(message));

                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void Main(string[] args)
        {
            Thread listenerThread = new Thread(Listener);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            Console.WriteLine("Multicast чат запущен.");
            Console.Write("Введите сообщение: ");

            while (true)
            {
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    message = input;
                    Thread senderThread = new Thread(SendMessage);
                    senderThread.IsBackground = true;
                    senderThread.Start();
                }
            }
        }
    }
}
