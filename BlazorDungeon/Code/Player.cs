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
        public Color color { get; set; }
        public Guid SessionId { get; set; }
        public IList<string> keyDown { get; set; }
        public IList<bool> sounds { get; set; }
        public IList<DateTime> soundsTime { get; set; }
    }
}
