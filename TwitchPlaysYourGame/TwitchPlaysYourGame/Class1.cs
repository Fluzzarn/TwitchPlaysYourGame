using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private static int _TimeBetweenCommands;

        public static int TimeBetweenCommands
        {
            get { return _TimeBetweenCommands; }
            set { _TimeBetweenCommands = value; }
        }

        private static System.Net.Sockets.TcpClient m_Socket;

        /// <summary>
        /// Twitch Chat Stream Buffer
        /// </summary>
        public static string Buffer
        {
            get { return _buffer; }
           private set { _buffer = value; }
        }



        private static bool _isRunning;

        public static bool IsRunning
        {
            get { return _isRunning; }
           private set {  _isRunning = value; }
        }
        

        private static Dictionary<string, int> CommandDictionary = new Dictionary<string, int>();
        private static Dictionary<string, Delegate> CommandFuncDict = new Dictionary<string, Delegate>();

        public static bool Connect()
        {
             m_Socket = new System.Net.Sockets.TcpClient();

            m_Socket.Connect(_serverAddress, port);
            if(!m_Socket.Connected)
            {
                Console.WriteLine("Connection Failed");
                return false;
            }

            var networkStream = m_Socket.GetStream();

            var chatReader = new System.IO.StreamReader(networkStream);
            var chatWrite = new System.IO.StreamWriter(networkStream);

            chatWrite.WriteLine("PASS " + _password);
            chatWrite.WriteLine("NICK " + _nickname.ToLower());
            chatWrite.Flush();


            outputThread = new Thread(() => ChatMessageSendThread(chatWrite));
            outputThread.Start();

            messageThread = new Thread(() => ChatMessageRecievedThread(chatReader, networkStream));
            messageThread.Start();


            Console.WriteLine("Connect Successful!");
            _isRunning = true;
            return true;
        }

        private static void ChatMessageSendThread(StreamWriter chatWrite)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            while(_isRunning)
            {
                lock (messagesToSend)
                {
                    if(messagesToSend.Count > 0)
                    {
                        //1500 to make sure twitch doesn't time us out
                        if(timer.ElapsedMilliseconds > 1500)
                        {

                            string command = messagesToSend.Dequeue();
                            chatWrite.WriteLine(command);
                            chatWrite.Flush();

                            if(command == "PART #" + _channelName)
                            {
                                _isRunning = false;
                            }

                            timer.Stop();
                            timer.Reset();
                            timer.Start();
                        }
                    }

                }
            }
            Console.WriteLine("MessageThread Ended");
        }

        private static void ChatMessageRecievedThread(StreamReader chatReader, NetworkStream networkStream)
        {
            
            while(_isRunning)
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
            Console.WriteLine("Recieved Thread Ended");

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

        public static void ExecuteCommand(string command)
        {
            if(CommandFuncDict.ContainsKey(command))
            {
                CommandFuncDict[command].DynamicInvoke();
            }
        }


        public static void Disconnect()
        {
            SendCommand("PART #" + _channelName);
            _isRunning = false;
            m_Socket.Close();
        }

        public static void PauseInput()
        {
            _isRunning = false;
        }
    }
}
