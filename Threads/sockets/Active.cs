using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Threads.sockets
{
    internal class Active
    {
        public static void Run()
        {
            var ipHost = Dns.GetHostEntry("microsoft.com");


            IPAddress ip = ipHost.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint endPoint = new IPEndPoint(ip, 80);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                s.Connect(endPoint);

                if (s.Connected)
                {
                    String strSend = "GET\r\n\r\n";

                    s.Send(System.Text.Encoding.ASCII.GetBytes(strSend));

                    byte[] buffer = new byte[1024];
                    int l;

                    do
                    {
                        l = s.Receive(buffer);
                        Console.Write(System.Text.Encoding.ASCII.GetString(buffer, 0, l));
                    } while (l > 0);
                }
                else
                {
                    Console.WriteLine("Unable to connect to the server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }
        }
    }
}
