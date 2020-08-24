using System.Collections.Generic;

namespace BlazorDungeon.Code
{
    public class Row
    {
        public short y { get; set; }
        public IList<Cell> Cells { get; set; }
    }
}
