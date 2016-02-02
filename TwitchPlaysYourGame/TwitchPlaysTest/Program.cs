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

        public delegate void kappa();

        static void Main(string[] args)
        {

            TwitchPlays.ChannelName = "fluzzarn";
            TwitchPlays.NickName = "Fluzzarn";
            TwitchPlays.Password = Password.OAuthPWord;
            TwitchPlays.ServerAddress = "irc.twitch.tv";

            TwitchPlays.Connect();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            kappa k = printKapps;
            TwitchPlays.AddCommandToFunction("Kappa",printKapps);
            TwitchPlays.AddCommandToFunction("exit", quit);
            TwitchPlays.AddCommandToFunction("quit", quit);
            while(true)
            {
                if(timer.ElapsedMilliseconds > 5000)
                {

                    string command = TwitchPlays.GetMostCommonCommand();
                    int amount = TwitchPlays.GetFrequencyOfCommand(command);
                    Console.WriteLine("Most common input was: " + command + " with " + amount + " instances");
                    TwitchPlays.ExecuteCommand(command);
                    TwitchPlays.ClearCommands();
                    timer.Reset();
                    timer.Start();
                }


            }
        }



        public static void printKapps()
        {
            Console.WriteLine(@"
                                                         
                                                         
                                                         
                                                         
                                                         
                                                         
                                                         
                     ```````                             
                   `.:;';,:;:;:;,.,                      
                 `.:,:;#+::;;'';++;,::`                  
               `..::;;'#''''''#;'''+;;;:                 
              .::';';;#;'+''++';'';+'+;:;.               
             `,:;:;':';'+;'+';':''@#';:;::`              
            `,':+:':::;;:',;::''''#+';;'+'',             
           .:::;,;;:::;';:+#@#++#'##:'++##@#`            
          .:;;:';;;:;;+';'+;+####+#'+'######;            
         .,;;;'::;:;'#':'#;+@####+#'+++'####+`           
         ,:;;:;;;';;;:'''++#'#+#+++'#+''#@#@@'           
        `,;:;::;;+';;++##++++'#++'+###++#@@@@+           
        .:;;::;''';++#+##+++##++'''+#+###@@@@#.          
        :;::;;;'++#@#+#####++++'';;;''+#######,          
        ;:;;''++##@######@#''';;::::;;''+#@@@@;          
       .';''''++#@+@#+++#+++';::,:,::::;'#@@@@;          
       .+'+''#+#@@@#'#+''+;;:,,,,,,,::::;+@@@@;          
       ,+++++###@@@###+;'';:,,,,,,,:,:::;'+@@#;          
       .+#++###@@@##++:;;':,,,,,,,:::::;;;'@##,          
       `+++#@###@##++':,:;:,,,,,,:,:::::;;;###,          
       `+###@###@++''+',,,:,,,,,:,::::::;;;'##.          
        +##@#####;+''::;:,,,,,,,,,:,::::;;;;#@,          
        ###@####'''::,,,,..,,,,,,,,,,,:::;;;#@,          
        +##@##+#+';,,,,..,..,.,,:,,,,,:::;;;+@.          
        ;##@####+;:,......,,,::::::,::;;;;;;'#`          
        ;##@###+;,,.....,,:;;';::::::;;;;;;';@           
        ,##@@##',,,,,,,,:'++++'';::;'++###++;#           
        ..#@##+:,..,::;';;''+''';:;++###+#++;'           
        ,:,###':...,:;''+##+++'':,:##@@@##++;,           
        ,';;+#+,...,:;+++''''';;:..++'+++++';            
        ,;,:;+',.....,::;;;'''::,..:''''''';:            
        .:,:''+,,.....,,::;:;:,,,..:''''';;;;            
        .:,;;,;,,,.......,,,,,,,,..,;;;;;::;:            
        .,,:,`,,,,.....`.....,,.,..,;;:::::;;            
        ..,,,..,,,,..........,,,....:;:::::;;            
        `.,,.,,,,,,,,,.....,,:,....,,;;::::;'            
         ..`...,,,,,,,,,,,,:::.....,:;;;;;;''            
          .....,,,,,,,,,,::;:::;::,::'';;;;''            
          `....,,,,,,,::::::,,;+'';;'+';;;'''            
           `:,,,,,,,,,:::::,,,,,:;'++'';;;;''            
             .:,,,,,,,,,::,,,,:::;;'+'';;;;';            
              .:,,,,,,,,:,,,:::::;'''';;;:;',            
               ,:,,,,,,,,,::::;;;;;'''+'::;'`            
                :::::,,,,,:::;:;++#++';;:;''             
                .::::::::,,,,,,::;;;;;;;;''`             
                 .;:::::;:,,,,::;;;';;;;'',              
                  .;;;;;;::,,,::;'';;;;'''               
                   ,;;;;;::,,::::;;';;;''                
                    `;;;::::,:::::;;;:;:                 
                      ;;;::::,,,,:::::',                 
                       ;;;;::::,:::;;'',                 
                       ,;;;;;;;;;;;''+,                  
                         ,;;'''''''+;                    
                                ``                       
                                                         ");
        }

        public static void quit()
        {
            //System.Environment.Exit();
        }
    }
}
