using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleBotTester
{
    public class ConsoleGui : IGuiDisplay
    {
        string enemyBot;

        public void BattleDone()
        {
            Console.WriteLine("Game Over");            
        }

        public void GetTokenFromServer()
        {
            Console.WriteLine("Contacting Server");
        }

        public void GotMessageFromUserBot(string message)
        {
            Console.WriteLine($"Got answer from Userbot");
        }

        public void GotNewGameState(string gameState)
        {
            if(string.IsNullOrEmpty(gameState) == false)
            {
                var state = JsonConvert.DeserializeObject<GameElements.GameState>(gameState);

                var userUnits = state.Planets.Where(x => x.OwnerId == 1).Sum(x => x.NumberOfShips) + state.Fleets.Where(x => x.OwnerId == 1).Sum(x => x.NumberOfShips);
                var enemyUnits = state.Planets.Where(x => x.OwnerId > 1).Sum(x => x.NumberOfShips) + state.Fleets.Where(x => x.OwnerId > 1).Sum(x => x.NumberOfShips);

                Console.WriteLine($"{userUnits} vs {enemyUnits} Units");
                Console.WriteLine();
            }
        }

        public void GotToken(TokenDto gameToken)
        {
            Console.WriteLine("Got game token");
        }

        public void Initialize(string enemyBot, string mapName)
        {
            this.enemyBot = enemyBot;
            Console.WriteLine($"Starting battle, map: {mapName}");
        }

        public void ListEnemyBots(List<string> enemies)
        {
            Console.WriteLine("Enemy Bots:");
            Console.WriteLine(string.Join(Environment.NewLine, enemies));
        }

        public void NewRound(int round)
        {
            Console.Clear();
            Console.WriteLine($"Round: {round++}");
            Console.WriteLine($"Userbot vs {enemyBot}");

        }

        public void SaveBattle(string outputFile)
        {            
            Console.WriteLine($"Battle saved to: {outputFile}");
        }

        public void SendFleet(string message)
        {
            
        }

        public void SendGameStateToUserBot()
        {
            Console.WriteLine($"Sending gamestate to Userbot");
        }

        public void UserBotReady()
        {
            Console.WriteLine("Userbot ready!");
        }

        public void WaitingForUserBot()
        {
            Console.WriteLine("Waiting for Userbot to get ready");
        }

        public void WinnerFound(string winner)
        {
            Console.WriteLine($"Winner: {winner}");
        }
    }
}
