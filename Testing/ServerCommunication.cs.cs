using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BattleBotTester
{
    public partial class ServerCommunication
    {
        private readonly HttpClient HttpClient = new HttpClient { BaseAddress = new Uri("http://www.spacebattlez.com/BattleTest/") };

        internal async Task<List<string>> GetEnemiesFromServer()
        {
            var response = await RequestData("EnemyBots", "Could not get enemies from server");
            return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
        }

        internal async Task<TokenDto> StartBattleOnServer(string serverBot, string map)
        {
            var response = await RequestData("StartBattle", "Could not get token from server", GetContent(new { serverBot, map }));
            return JsonConvert.DeserializeObject<TokenDto>(await response.Content.ReadAsStringAsync());
        }

        internal async Task SendFleetToServer(TokenDto token, string fleetCommands)
        {
            await RequestData("SubmitTurn", "Error sending fleetcommands", GetContent(GetFleetDto(token, fleetCommands)));
        }

        internal async Task<string> GetWinnerFromServer(TokenDto token)
        {
            var response = await RequestData("GetWinner", "Could not get winner from server", GetContent(token));
            return JsonConvert.DeserializeObject<WinnerDto>(await response.Content.ReadAsStringAsync()).Winner;
        }

        internal async Task<string> GetGameStateFromServer(TokenDto token)
        {
            var response = await RequestData("GetState", "Could not get gamestate from server", GetContent(token));
            if (response != null)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        private FleetDTO GetFleetDto(TokenDto token, string fleetCommand)
        {
            var fleet = JsonConvert.DeserializeObject<GameElements.FleetCommand[]>(fleetCommand);
            return new FleetDTO
            {
                Token = token.Token,
                Commands = fleet
            };
        }

        private StringContent GetContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        private async Task<HttpResponseMessage> RequestData(string request, string errorMessage, HttpContent content = null)
        {
            var response = content == null ?
                await HttpClient.GetAsync(request) :
                await HttpClient.PostAsync(request, content);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Gone)
                {
                    return null;
                }
            }
            throw new Exception(errorMessage);
        }
    }
}
