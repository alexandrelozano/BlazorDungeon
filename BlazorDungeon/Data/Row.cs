using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDungeon.Data
{
    public class Row
    {
        public short y { get; set; }
        public IList<Cell> Cells { get; set; }
    }
}
