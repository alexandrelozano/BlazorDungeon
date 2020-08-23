using System.Drawing;


namespace BlazorDungeon.Data
{
    public class Cell
    {
        public short x { get; set; }
        public short y { get; set; }

        public string cssClass;

        public string character { get; set; }

    }
}
