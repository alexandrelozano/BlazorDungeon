using System;
using System.IO;

namespace BlazorDungeon.Code
{
    public class StateManager : IDisposable
    {
        public Guid Id { get; private set; }
        public Game game;

        private string logPath;

        public StateManager()
        {
            this.Id = Guid.NewGuid();
            logPath = @"C:\Log\Tmp";
        }

        public void Initialize(Game game)
        {
            this.game = game;

            short i;
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
                if (Directory.Exists(logPath))
                {
                    string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " New connection " + Id.ToString() + Environment.NewLine;
                    System.IO.File.AppendAllText(logPath + @"\BlazorDungeon.txt", text);
                }
            }
            catch(Exception e)
            {
            }
        }

        public void Dispose()
        {
            if (game != null)
            {
                short i;
                foreach (Player player in game.players)
                {
                    if (player.SessionId == Id)
                    {
                        try
                        {
                            if (Directory.Exists(logPath))
                            {
                                string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " Ended connection " + player.SessionId.ToString() + " " + player.name + " max. score session:" + player.maxScoreSession.ToString() + Environment.NewLine;
                                System.IO.File.AppendAllText(logPath + @"\BlazorDungeon.txt", text);
                            }
                        }
                        catch (Exception e)
                        {
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
