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
                        player.SessionId = Guid.Empty;
                        player.score = 0;
                        break;
                    }
                }
            }

            try
            {
                if (Directory.Exists(logPath))
                {
                    string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " Ended connection " + Id.ToString() + Environment.NewLine;
                    System.IO.File.AppendAllText(logPath + @"\BlazorDungeon.txt", text);
                }
            }
            catch (Exception e)
            {
            }

        }
    }
}
