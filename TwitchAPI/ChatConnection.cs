using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchAPI
{
    public class TwitchMessage
    {
        public string Sender { get; }

        public string Message { get; }

        public TwitchMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }

    public class ChatConnection : IDisposable
    {
        const string twitchIRC = @"wss://irc-ws.chat.twitch.tv:443";

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ClientWebSocket webSocket = new ClientWebSocket();
        private readonly Thread readThread;

        public event EventHandler<TwitchMessage> MessageReceived;

        public string Channel { get; }

        public ChatConnection(Token token, string channelName)
        {
            Channel = channelName;

            Uri connectionUri = new Uri(twitchIRC);

            webSocket.ConnectAsync(connectionUri, cancellationTokenSource.Token).Wait();

            ThreadStart threadStart = new ThreadStart(ReadThread);
            readThread = new Thread(threadStart);
            readThread.Start();

            SendTextMessageAsync($"PASS oauth:{token.Value}").ContinueWith(_ =>
            SendTextMessageAsync($"NICK {token.User}")).ContinueWith(_ =>
            SendTextMessageAsync($"JOIN #{Channel}")).ContinueWith(_ => { });            
        }

        public async Task SendMessageAsync(string message)
        {
            await SendTextMessageAsync($"PRIVMSG #{Channel} :{message}");
        }

        private void ReadThread()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                byte[] buffer = new byte[1024];
                ArraySegment<byte> memory = new ArraySegment<byte>(buffer);

                try
                {
                    var result = webSocket.ReceiveAsync(memory, cancellationTokenSource.Token).Result;

                    Console.WriteLine(result.MessageType);
                    Console.WriteLine(result.EndOfMessage);
                    Console.WriteLine(result.CloseStatus);
                    Console.WriteLine(result.CloseStatusDescription);
                }
                catch (System.AggregateException)
                {
                    return;
                }

                string message = Encoding.ASCII.GetString(memory.Array).Trim('\0', '\r', '\n');
                Console.WriteLine(message);

                if (message == "PING :tmi.twitch.tv")
                    SendMessageAsync("PONG :tmi.twitch.tv").Wait();
            }
        }

        private static ArraySegment<byte> ToArraySegment(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            ArraySegment<byte> arraySegment = new ArraySegment<byte>(bytes);
            return arraySegment;
        }

        private async Task SendTextMessageAsync(string text)
        {
            await webSocket.SendAsync(ToArraySegment(text), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            readThread.Join();
            ((IDisposable)webSocket).Dispose();
        }
    }
}
