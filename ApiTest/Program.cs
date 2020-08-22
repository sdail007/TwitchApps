using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI;

namespace ApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (ChatConnection twitchConnection = new ChatConnection(token, "truelove429"))
            //{
            //    twitchConnection.MessageReceived += TwitchConnection_MessageReceived;

            //    string message = Console.ReadLine();

            //    while (message != "q")
            //    {
            //        twitchConnection.SendMessage(message).ContinueWith(t => { });
            //        message = Console.ReadLine();
            //    }
            //}
        }

        private static void TwitchConnection_MessageReceived(object sender, TwitchMessage e)
        {
            Console.WriteLine($"{e.Sender}: {e.Message}");
        }
    }
}
