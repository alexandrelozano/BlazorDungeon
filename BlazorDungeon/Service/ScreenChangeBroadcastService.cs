using System;
using System.Collections.Generic;
using BlazorDungeon.Data;
using System.Timers;
using System.Drawing;

namespace BlazorDungeon.Service
{
    public class ScreenChangeBroadcastService : IScreenChangeBroadcastService
    {
        public Game game;

        public event ScreenChangeDelegate OnScreenChanged;

        public ScreenChangeBroadcastService()
        {
            game = new Game(80,25);
            game.gameTimer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            game.step();
            game.draw();

            if (this.OnScreenChanged != null)
                this.OnScreenChanged(this, new ScreenChangeEventArgs(null));
        }

        public void KeyDown(string key, Guid SessionId)
        {
            for (short i = 0; i < game.playerCount; i++)
            {
                if (game.playerSessionId[i] == SessionId)
                {
                    if (!game.keyDown[i].Contains(key))
                        game.keyDown[i].Add(key);
                }
            }
        }

        public void KeyUp(string key, Guid SessionId)
        {
            for (short i = 0; i < game.playerCount; i++)
            {
                if (game.playerSessionId[i] == SessionId)
                {
                    if (game.keyDown[i].Contains(key))
                        game.keyDown[i].Remove(key);
                }
            }
        }

        public IList<Row> GetCurrentValues(Guid SessionId)
        {
            IList<Row> rows = null;
            bool found=false;

            for (short i = 0; i < game.playerCount; i++)
            {
                if (game.playerSessionId[i] == SessionId)
                {
                    rows=game.rows[i];
                    found = true;
                }
            }

            if (!found) rows = game.rows[game.playerCount];

            return rows;
        }

        public IList<bool> GetCurrentValuesSounds(Guid SessionId)
        {
            IList<bool> sounds = null;

            for (short i = 0; i < game.playerCount; i++)
            {
                if (game.playerSessionId[i] == SessionId)
                {
                    sounds = game.sounds[i];
                }
            }

            return sounds;
        }

        public void Dispose()
        {
            
        }
    }
}
