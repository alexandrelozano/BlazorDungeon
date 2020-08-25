using System;
using System.Collections.Generic;
using System.Drawing;

namespace BlazorDungeon.Code
{
    public class Player
    {
        public short x { get; set; }
        public short y { get; set; }
        public int score { get; set; }
        public int maxScoreSession { get; set; }
        public Color color { get; set; }
        public Guid SessionId { get; set; }
        public IList<string> keyDown { get; set; }
        public IList<bool> sounds { get; set; }
        public IList<DateTime> soundsTime { get; set; }
        public short state { get; set; }
        public short cursorX { get; set; }
        public short cursorY { get; set; }
        public string name { get; set; }

        public Player()
        {
            name = "";
        }
    }
}
