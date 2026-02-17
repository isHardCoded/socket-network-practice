using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.Mail;

namespace Threads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string to = "z1tra@yandex.ru";
                string from = "z1tra@yandex.com";

                string password = "pduwuvdlbzbahdye";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(from, "ishardcoded");
                message.To.Add(to);
                message.Subject = "Тестовое письмо";
                message.Body = "Привет, как дела? \n\nЭто письмо отправлено из C# приложения";
                message.IsBodyHtml = false;

                SmtpClient smpt = new SmtpClient("smtp.yandex.ru", 587);
                smpt.Credentials = new NetworkCredential(from, password);
                smpt.EnableSsl = true;

                smpt.Send(message);

                Console.WriteLine("Письмо отправленно успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
