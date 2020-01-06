using CommandLine;
using System;
using System.IO;

namespace BattleBotTester
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            BattleMaster battleMaster = null;
            IGuiDisplay gui = new ConsoleGui();
            string outputFile = null;
            bool displayBattle = false;
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                var com = new ServerCommunication();
                if (o.ListBots)
                {
                    var enemies = com.GetEnemiesFromServer().Result;
                    gui.ListEnemyBots(enemies);                    
                    return;
                }
                
                var startProcess = new FileInfo(o.StartProcess);
                if(startProcess.Exists)
                {
                    outputFile = o.OutputFile;
                    gui.Initialize(o.EnemyBot, o.MapName);
                    var botProcess = new BotProcess(startProcess, o.Arguments);
                    gui.GetTokenFromServer();                    
                    var gameToken = com.StartBattleOnServer(o.EnemyBot, o.MapName).Result;
                    gui.GotToken(gameToken);                    
                    battleMaster = new BattleMaster(botProcess, com, gameToken, gui);
                    displayBattle = o.showresults;
                }
                else
                {
                    throw new ArgumentException($"Not possible to locate file: {startProcess.FullName}");
                }
            });

            if(battleMaster != null)
            {
                await battleMaster.Start();
                if(outputFile != null)
                {
                    File.WriteAllLines(outputFile, battleMaster.GetHistory());
                    gui.SaveBattle(outputFile);
                }
                gui.BattleDone();
            }

            if(displayBattle)
            {

            }
        }

        public class Options
        {
            [Option('l', "list bots", HelpText = "List all possible enemy bots to battle", SetName = "info")]
            public bool ListBots { get; set; }

            [Option('e', "enemy", HelpText = "Name of the enemy bot to battle\nUser -l to list all possible enemy bots", Default = "BasicBot", SetName = "Launch")]
            public string EnemyBot { get; set; }

            [Option('m', "map", HelpText = "Seed name of map to generate.\nExample:\nBattleBotTester.exe -m \"My Awesome Map\"", Default = "Test Map", SetName = "Launch")]
            public string MapName { get; set; }

            [Option('p', "start process", Required = true, HelpText = "Start process to start the local bot\nExample:\nBattleBotTester.exe -p \"MyAwesomeBot.exe\"\nBattleBotTester.exe -p \"Node.js\" -a \"c:\\SpaceBattlez\\MyAwesomeBot.js\"", SetName = "Launch")  ]
            public string StartProcess { get; set; }

            [Option('a', "start arguments", HelpText = "Arguments needed for the start process to start the local bot\nExample:\nBattleBotTester.exe -p \"Node.js\" -a \"c:\\SpaceBattlez\\MyAwesomeBot.js\"", SetName = "Launch")]
            public string Arguments { get; set; }

            [Option('o', "output file", HelpText = "File path to save all gamestates\nExample:\nBattleBotTester.exe -p \"MyAwesomeBot.exe -o \"c:\\temp\\GameStates.txt\"", Default = null, SetName = "Launch")]
            public string OutputFile { get; set; }

            [Option('d', "display battle", HelpText = "Start a viewer to show the battle\nBattleBotTester.exe -p \"MyAwesomeBot.exe -d", Default = true, SetName = "Launch")]
            public bool showresults { get; set; }
        }        
    }
}
