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
        private static Thread outputThread;
        private static Queue<string> messagesToSend = new Queue<string>();


        private static string _serverAddress;

        public static string ServerAddress
        {
            get { return _serverAddress; }
            set { _serverAddress = value; }
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

        private static Dictionary<string, int> CommandDictionary = new Dictionary<string, int>();
        private static Dictionary<string, Delegate> CommandFuncDict = new Dictionary<string, Delegate>();

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


            outputThread = new Thread(() => ChatMessageSendThread(chatWrite));
            outputThread.Start();

            messageThread = new Thread(() => ChatMessageRecievedThread(chatReader, networkStream));
            messageThread.Start();


            Console.WriteLine("Connect Succesful!");
            return true;
        }

        private static void ChatMessageSendThread(StreamWriter chatWrite)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            while(true)
            {
                lock (messagesToSend)
                {
                    if(messagesToSend.Count > 0)
                    {
                        if(timer.ElapsedMilliseconds > 1500)
                        {
                            chatWrite.WriteLine(messagesToSend.Dequeue());
                            chatWrite.Flush();

                            timer.Stop();
                            timer.Restart();
                        }
                    }

                }
            }
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
                    SendCommand("JOIN #" + _channelName);
                }

                if(_buffer.Contains("PING :"))
                {
                    SendCommand("PONG :tmi.twitch.tv");
                }

                if(_buffer.Contains("PRIVMSG #"))
                {
                    string substringKey = "#" + _channelName + " :";
                    string userStrippedMsg = _buffer.Substring(_buffer.IndexOf(substringKey) + substringKey.Length);

                    //Console.WriteLine(userStrippedMsg);

                    lock (CommandDictionary)
                    {
                        if(CommandDictionary.ContainsKey(userStrippedMsg))
                        {
                            CommandDictionary[userStrippedMsg]++;
                        }
                        else
                        {
                            CommandDictionary.Add(userStrippedMsg,1);
                        }
                    }
                }

            }
        }

        private static void SendCommand(string command)
        {
            lock (messagesToSend)
            {
                messagesToSend.Enqueue(command);
            }
        }

        public static string GetMostCommonCommand()
        {

            string mostCommon = "";
            int highestAmount = 0;
            foreach (var item in CommandDictionary)
            {
                if(item.Value > highestAmount)
                {
                    mostCommon = item.Key;
                    highestAmount = item.Value;
                }
            }

            return mostCommon;
        }

        public static int GetFrequencyOfCommand(string command)
        {
            if(CommandDictionary.ContainsKey(command))
            {
                return CommandDictionary[command];
            }
            else
            {
                return 0;
            }
        }

        public static void ClearCommands()
        {
            CommandDictionary.Clear();
        }

        public static void AddCommandToFunction(string command, Action func )
        {
            if(!CommandFuncDict.ContainsKey(command))
            {
                CommandFuncDict.Add(command, func);
            }
        }

        public static void executeCommand(string command)
        {
            if(CommandFuncDict.ContainsKey(command))
            {
                CommandFuncDict[command].DynamicInvoke();
            }
        }
    }
}
