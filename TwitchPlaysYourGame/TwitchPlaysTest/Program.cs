using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchPlaysYourGame;

namespace TwitchPlaysTest
{
    class Program
    {
        static void Main(string[] args)
        {

            TwitchPlays.ChannelName = "twitchplayspokemon";
            TwitchPlays.NickName = "Fluzzarn";
            TwitchPlays.Password = Password.OAuthPWord;
            TwitchPlays.ServerAddress = "irc.twitch.tv";

            TwitchPlays.Connect();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            while(true)
            {
                if(timer.ElapsedMilliseconds > 5000)
                {

                    string command = TwitchPlays.GetMostCommonCommand();
                    int amount = TwitchPlays.GetFrequencyOfCommand(command);
                    Console.WriteLine("Most common input was: " + command + " with " + amount + " instances");
                    TwitchPlays.ClearCommands();
                    timer.Reset();
                    timer.Start();
                }


            }
        }
    }
}
