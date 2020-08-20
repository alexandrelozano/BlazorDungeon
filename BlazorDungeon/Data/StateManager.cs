using System;
using System.Diagnostics;

namespace BlazorDungeon.Data
{
    public class StateManager : IDisposable
    {
        public Guid Id { get; private set; }
        public Game game;

        public StateManager()
        {
            this.Id = Guid.NewGuid();
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
        }
    }
}
