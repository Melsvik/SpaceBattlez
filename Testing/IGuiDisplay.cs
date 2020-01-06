using System.Collections.Generic;

namespace BattleBotTester
{
    public interface IGuiDisplay
    {
        void WaitingForUserBot();
        void UserBotReady();
        void GotMessageFromUserBot(string message);
        void NewRound(int round);
        void WinnerFound(string winner);
        void SendGameStateToUserBot();
        void ListEnemyBots(List<string> enemies);
        void GotToken(TokenDto gameToken);
        void GetTokenFromServer();
        void SendFleet(string message);
        void Initialize(string enemyBot, string mapName);
        void BattleDone();
        void SaveBattle(string outputFile);
        void GotNewGameState(string gameState);
    }
}