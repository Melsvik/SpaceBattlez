using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleBotTester
{
    class BattleMaster
    {
        private BotProcess botProcess;
        private List<string> history = new List<string>();
        private IGuiDisplay gui;
        private int round = 1;


        public BattleMaster(BotProcess botProcess, ServerCommunication com, TokenDto gameToken, IGuiDisplay guiDisplayer)
        {
            this.botProcess = botProcess;
            gui = guiDisplayer;                       
            
            this.botProcess.BotReady += () =>
            {
                gui.UserBotReady();
                GetNewGameState(com, gameToken).Wait();
            };

            this.botProcess.GotMessage += (message) =>
            {
                if(string.IsNullOrEmpty(message) == false)
                {
                    gui.GotMessageFromUserBot(message);
                    gui.SendFleet(message);
                    com.SendFleetToServer(gameToken, message).Wait();
                    GetNewGameState(com, gameToken).Wait();
                }
            };
        }

        private async Task GetNewGameState(ServerCommunication com, TokenDto gameToken)
        {
            gui.NewRound(round++);            
            var gameState = await com.GetGameStateFromServer(gameToken);
            gui.GotNewGameState(gameState);
            if(gameState == null)
            {
                var winner = await com.GetWinnerFromServer(gameToken);
                gui.WinnerFound(winner);
                Terminate();
                return;
            }            
            
            history.Add(gameState);
            gui.SendGameStateToUserBot();            
            botProcess.SendGameState(gameState);
        }

        internal Task Start()
        {
            gui.WaitingForUserBot();
            return botProcess.Start();
        }

        internal void Terminate()
        {
            botProcess.SendGameState("Terminate");
        }

        internal List<string> GetHistory()
        {
            return history;
        }
    }
}
