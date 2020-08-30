using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace BlazorDungeon.Code
{
    public class StateManager : IDisposable
    {
        public Guid Id { get; private set; }
        public Game game;
        private readonly IConfiguration configuration;
        private string logFile;

        public StateManager(IConfiguration configuration)
        {
            this.Id = Guid.NewGuid();
            this.configuration = configuration;
            logFile = configuration.GetValue<string>("BlazorDungeon:LogFile");
        }

        public void Initialize(Game game)
        {
            this.game = game;

            foreach (Player player in game.players)
            {
                if (player.SessionId == Guid.Empty)
                {
                    player.SessionId = Id;
                    player.state = 0;
                    player.name = "";
                    player.cursorX = 31;
                    player.cursorY = 12;
                    break;
                }
            }

            try
            {
                if (Directory.Exists(Path.GetDirectoryName(logFile)))
                {
                    string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " New connection " + Id.ToString() + Environment.NewLine;
                    System.IO.File.AppendAllText(logFile, text);
                }
            }
            catch(Exception e)
            {
                Debug.Print(e.Message);
            }
        }

        public void Dispose()
        {
            if (game != null)
            {
                foreach (Player player in game.players)
                {
                    if (player.SessionId == Id)
                    {
                        try
                        {
                            if (Directory.Exists(Path.GetDirectoryName(logFile)))
                            {
                                string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " Ended connection " + player.SessionId.ToString() + " " + player.name + " max. score session:" + player.maxScoreSession.ToString() + Environment.NewLine;
                                System.IO.File.AppendAllText(logFile, text);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.Message);
                        }

                        player.SessionId = Guid.Empty;
                        player.name = "";
                        player.maxScoreSession = 0;
                        player.score = 0;
                        break;
                    }
                }
            }

        }
    }
}
