using System.Drawing;


namespace BlazorDungeon.Data
{
    public class Cell
    {
        public short x { get; set; }
        public short y { get; set; }
        public Color foreColor;
        public Color backColor;
        public string character { get; set; }

        public string foreColorHTML()
        {
            return System.Drawing.ColorTranslator.ToHtml(foreColor);
        }

        public string backColorHTML()
        {
            return System.Drawing.ColorTranslator.ToHtml(backColor);
        }
    }
}
