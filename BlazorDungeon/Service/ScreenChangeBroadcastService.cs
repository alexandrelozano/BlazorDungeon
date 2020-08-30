using System;
using System.Collections.Generic;
using BlazorDungeon.Code;
using System.Timers;
using System.Drawing;
using Microsoft.Extensions.Configuration;

namespace BlazorDungeon.Service
{
    public class ScreenChangeBroadcastService : IScreenChangeBroadcastService
    {
        public Game game;

        public event ScreenChangeDelegate OnScreenChanged;

        public ScreenChangeBroadcastService(IConfiguration configuration)
        {
            game = new Game(80,25, configuration.GetValue<string>("BlazorDungeon:HighScoresFile"));
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
            foreach(Player player in game.players)
            {
                if (player.SessionId == SessionId)
                {
                    if (!player.keyDown.Contains(key))
                        player.keyDown.Add(key);
                }
            }
        }

        public void KeyUp(string key, Guid SessionId)
        {
            foreach (Player player in game.players)
            {
                if (player.SessionId == SessionId)
                {
                    if (key== "ArrowUp" || key== "ArrowDown" || key== "ArrowLeft" || key== "ArrowRight")
                    {
                        if (player.keyDown.Contains(key))
                            player.keyDown.Remove(key);
                    }
                }
            }
        }

        public IList<Row> GetCurrentValues(Guid SessionId)
        {
            IList<Row> rows = null;
            bool found=false;

            for (short i = 0; i < game.players.Count; i++)
            {
                if (game.players[i].SessionId == SessionId)
                {
                    rows=game.rows[i];
                    found = true;
                }
            }

            if (!found) rows = game.rows[game.players.Count];

            return rows;
        }

        public IList<bool> GetCurrentValuesSounds(Guid SessionId)
        {
            IList<bool> sounds = null;

            foreach (Player player in game.players)
            {
                if (player.SessionId == SessionId)
                {
                    sounds = player.sounds;
                }
            }

            return sounds;
        }

        public void Dispose()
        {
            
        }
    }
}
