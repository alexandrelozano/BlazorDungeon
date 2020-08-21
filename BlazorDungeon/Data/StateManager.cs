using System;
using System.Diagnostics;
using System.IO;

namespace BlazorDungeon.Data
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
            for (i = 0; i < game.playerCount; i++)
            {
                if (game.playerSessionId[i] == Guid.Empty)
                {
                    game.playerSessionId[i] = Id;
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
                for (i = 0; i < game.playerCount; i++)
                {
                    if (game.playerSessionId[i] == Id)
                    {
                        game.playerSessionId[i] = Guid.Empty;
                        game.playerScore[i] = 0;
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
