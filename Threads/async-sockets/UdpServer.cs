using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Threads.async_sockets
{
    internal class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public Socket Socket;
        public EndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    class UdpServer
    {
        private Socket _socket;
        private StateObject _state;
        private int _port;

        public UdpServer(int port)
        {
            _port = port;
            _state = new StateObject();
        }

        public void Start()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _state.Socket = _socket;

            Console.WriteLine($"UDP сервер запущен на порту {_port}");
            Console.WriteLine("Ожидание сообщений...");

            BeginReceive();
        }

        private void BeginReceive()
        {
            _socket.BeginReceiveFrom(_state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, ref _state.RemoteEndPoint, ReceiveCompleted, _state);
        }

        private void ReceiveCompleted(IAsyncResult asyncResult)
        {
            StateObject stateObject = (StateObject)asyncResult.AsyncState;
            int bytesRead = stateObject.Socket.EndReceiveFrom(asyncResult, ref stateObject.RemoteEndPoint);

            string message = Encoding.UTF8.GetString(stateObject.Buffer, 0, bytesRead);
            Console.WriteLine($"Получено от {stateObject.RemoteEndPoint}");
            Console.WriteLine(message);

            BeginReceive();
        }
    }

    class UdpClientSender
    {
        private UdpClient _udpClient;

        public UdpClientSender()
        {
            _udpClient = new UdpClient();
        }

        public void Send(string message, string ip, int port)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _udpClient.Send(data, data.Length, ip, port);
        }

    public void Run()
        {
            int port = 100;
            UdpServer server = new UdpServer(port);

            server.Start();

            UdpClientSender client = new UdpClientSender();
            Console.Write("Введите сообщение для отправки по UDP: ");

            while (true)
            {
                string message = Console.ReadLine();

                if (string.IsNullOrEmpty(message))
                {
                    break;
                }

                client.Send(message, "127.0.0.1", port);
                Console.WriteLine("Сообщение отправлено");
            }
        }
    }
}
