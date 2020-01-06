using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BattleBotTester
{
    public class BotProcess
    {
        public Action BotReady { get; set; }
        public Action<string> GotMessage { get; set; }        

        private Process _botProcess;
        private bool isReady = false;

        public BotProcess(FileInfo application, string arguments)
        {
            _botProcess = new Process
            {
                StartInfo =
                {
                    FileName = application.FullName,
				    Arguments = arguments,
				    WorkingDirectory = application.DirectoryName ?? throw new InvalidOperationException("Not possible to set Working Directory"),
                    //UseShellExecute = false,
                    //CreateNoWindow = false,
                    //WindowStyle = ProcessWindowStyle.Normal,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                },
                EnableRaisingEvents = true
            };

            _botProcess.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    if (isReady == false && args.Data.ToLower() == "ready!")
                    {
                        isReady = true;
                        BotReady?.Invoke();
                    }
                    else
                    {
                        GotMessage?.Invoke(args.Data);
                    }
                }
            };
        }

        internal void SendGameState(string gameState)
        {
            _botProcess.StandardInput.WriteLine(gameState);
        }

        public Task Start()
        {
            return Task.Run(() =>
            {
                _botProcess.Start();
                _botProcess.BeginOutputReadLine();
                _botProcess.BeginErrorReadLine();
                _botProcess.WaitForExit();
            });           
        }
    }
}
