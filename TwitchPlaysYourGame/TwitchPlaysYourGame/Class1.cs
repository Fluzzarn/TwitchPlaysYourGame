using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace TwitchPlaysYourGame
{
    public static class TwitchPlays
    {
        private const int port = 6667;
        private static Thread messageThread;


        private static string _serverAddress;

        public static string ServerAddress
        {
            get { return _serverAddress; }
            private set { _serverAddress = value; }
        }


        private static string _password;

        public static string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private static string _nickname;

        public static string NickName
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        private static string _channelName;

        public static string ChannelName
        {
            get { return _channelName; }
            set { _channelName = value; }
        }


        private static string _buffer;

        /// <summary>
        /// Twitch Chat Stream Buffer
        /// </summary>
        public static string Buffer
        {
            get { return _buffer; }
           private set { _buffer = value; }
        }
        



        public static bool Connect()
        {
            System.Net.Sockets.TcpClient socket = new System.Net.Sockets.TcpClient();

            socket.Connect(_serverAddress, port);
            if(!socket.Connected)
            {
                Console.WriteLine("Connection Failed");
                return false;
            }

            var networkStream = socket.GetStream();

            var chatReader = new System.IO.StreamReader(networkStream);
            var chatWrite = new System.IO.StreamWriter(networkStream);

            chatWrite.WriteLine("PASS " + _password);
            chatWrite.WriteLine("NICK " + _nickname.ToLower());
            chatWrite.Flush();

            messageThread = new Thread(() => ChatMessageRecievedThread(chatReader, networkStream));
            messageThread.Start();

            return true;
        }

        private static void ChatMessageRecievedThread(StreamReader chatReader, NetworkStream networkStream)
        {
            
            while(true)
            {
                if (!networkStream.DataAvailable)
                    continue;

                _buffer = chatReader.ReadLine();

                //part of the successful join message
                if(_buffer.Split(' ')[1] == "001")
                {

                }

            }
        }
    }
}
