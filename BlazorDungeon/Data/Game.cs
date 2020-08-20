using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Timers;

namespace BlazorDungeon.Data
{
    public class Game
    {
        public IList<Row>[] rows;

        public string[] keyDown;

        public short[] playerX;
        public short[] playerY;
        public int[] playerScore;
        public Color[] playerColor;
        public short playerCount;
        public Guid[] playerSessionId;

        public short[] enemyX;
        public short[] enemyY;
        public short[] enemyDirection;
        public short enemyCount;

        public short[] itemX;
        public short[] itemY;
        public short[] itemType;
        public short[] itemValue;
        public short itemCount;

        public short cherrieX;
        public short cherrieY;
        public short redAppleX;
        public short redAppleY;
        public short greenAppleX;
        public short greenAppleY;
        public short strawberryX;
        public short strawberryY;

        public short width;
        public short height;
        public short widthDungeon;
        public short heightDungeon;

        public short gameSpeed;
        public Timer gameTimer;
        Random r = new Random();

        public string chPlayer;
        public string chEnemy;
        public string chWall;
        public string chCoin;
        public Color CoinColor;
        public string chCherrie;
        public Color CherrieColor;
        public string chRedApple;
        public Color RedAppleColor;
        public string chGreenApple;
        public Color GreenAppleColor;
        public string chStrawberry;
        public Color StrawberryColor;

        string[] maze;

        public Game(short width, short height)
        {
            this.width = width;
            this.height = height;
            widthDungeon = (short)(width - 12);
            heightDungeon = (short)(height - 2);

            playerCount = 5;

            rows = new List<Row>[playerCount+1];
            for (short i = 0; i < playerCount+1; i++)
            {
                rows[i] = new List<Row>();
                for (short y = 0; y < height; y++)
                {
                    Row r = new Row();
                    r.y = y;

                    IList<Cell> Cells = new List<Cell>();
                    for (short x = 0; x < width; x++)
                    {
                        Cell c = new Cell();
                        c.x = x;
                        c.y = y;
                        c.foreColor = Color.White;
                        c.backColor = Color.Black;
                        c.character = " ";
                        Cells.Add(c);
                    }

                    r.Cells = Cells;
                    rows[i].Add(r);
                }
            }

            chPlayer = Char.ConvertFromUtf32(1047636);
            chEnemy = Char.ConvertFromUtf32(1047634);
            chWall = Char.ConvertFromUtf32(1047550);
            chCoin = Char.ConvertFromUtf32(128308);
            chCherrie = Char.ConvertFromUtf32(127826);
            chRedApple = Char.ConvertFromUtf32(127822);
            chGreenApple = Char.ConvertFromUtf32(127823);
            chStrawberry = Char.ConvertFromUtf32(127827);

            maze = new string[23];
            maze[0] += "*********************************************************************";
            maze[1] += "*     *         *         *                 *         *         *   *";
            maze[2] += "* *** *** * *** ******* * ***** ***** ***** * * ***** * ******* * ***";
            maze[3] += "* * *   * * * *     *   *     * *     * *   * *     * * *       *   *";
            maze[4] += "* * *** * * * ***** * ******* * * ***** * ********* * * * ********* *";
            maze[5] += "* * *   * * * *   *   *   *   * *   * *   *         *   *   *       *";
            maze[6] += "* * * ***** * * * ******* * ******* * * ***** ************* *** *****";
            maze[7] += "* *         *   * *       *         *         *           *   *     *";
            maze[8] += "* * *********** * * * *************** *************** *** *** ***** *";
            maze[9] += "* *   *       * *   *       *       *   *           *   *   * *     *";
            maze[10] += "* ***** ***** * ********* *** * *** *** * ********* *** ***** * *****";
            maze[11] += "* *   *     * *   *     *     *   * *   * *       *   *         *   *";
            maze[12] += "* * * * ***** *** * ************* *** *** ***** * *** *********** ***";
            maze[13] += "*   *   *     * * *       *     *       *     * *   *       *   *   *";
            maze[14] += "* ******* ***** * ***** * * *** ******* ***** * *** ******* * * *** *";
            maze[15] += "*   *   * *       *   * *   *   *     *     * *   *   *   *   * *   *";
            maze[16] += "*** *** * * ******* * * * ******* *** ******* ***** * * ******* * ***";
            maze[17] += "*   *   * *         *   * *       * *       *   *   * *   *     * * *";
            maze[18] += "* *** *** ********* ******* ******* ******* *** * *** * * * ***** * *";
            maze[19] += "* *   *   *   *   * *     * *   *         * *   *   * * * *     *   *";
            maze[20] += "* * * * *** * * * *** *** * * * *** ***** * * ***** * *** ***** *** *";
            maze[21] += "*   * *     *   *       *     *     *     *         *         *     *";
            maze[22] += "*********************************************************************";

            keyDown = new string[playerCount];

            playerX = new short[playerCount];
            playerY = new short[playerCount];
            playerScore = new int[playerCount];
            playerColor = new Color[playerCount];
            playerSessionId = new Guid[playerCount];
            for (short i = 0; i < playerCount; i++)
            {
                randomPosition(out playerX[i], out playerY[i]);
                switch (i)
                {
                    case 0:
                        playerColor[i] = Color.DarkOrange;
                        break;
                    case 1:
                        playerColor[i] = Color.DarkGreen;
                        break;
                    case 2:
                        playerColor[i] = Color.DarkTurquoise;
                        break;
                    case 3:
                        playerColor[i] = Color.DarkSlateGray;
                        break;
                    case 4:
                        playerColor[i] = Color.DarkOrchid;
                        break;
                }
            }

            enemyCount = 10;
            enemyX = new short[enemyCount];
            enemyY = new short[enemyCount];
            enemyDirection = new short[enemyCount];
            for (short i = 0; i < enemyCount; i++)
            {
                randomPosition(out enemyX[i], out enemyY[i]);
                enemyDirection[i] = (short)(i % 4);
            }

            itemCount = 30;
            itemX = new short[itemCount];
            itemY = new short[itemCount];
            itemValue = new short[itemCount];
            itemType = new short[itemCount];
            for (short i = 0; i < itemCount; i++)
            {
                randomPosition(out itemX[i], out itemY[i]);

                if (i < 10) { itemType[i] = 0; itemValue[i] = 10; }
                else if (i < 15) { itemType[i] = 1; itemValue[i] = 20; }
                else if (i < 20) { itemType[i] = 2; itemValue[i] = 30; }
                else if (i < 25) { itemType[i] = 3; itemValue[i] = 40; }
                else { itemType[i] = 4; itemValue[i] = 50; }
            }
            CoinColor = Color.Gold;
            CherrieColor = Color.Red;
            GreenAppleColor = Color.Green;
            RedAppleColor = Color.DarkRed;
            StrawberryColor = Color.BlueViolet;

            gameSpeed = 200;

            gameTimer = new System.Timers.Timer();
            gameTimer.Interval = gameSpeed;
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
        }

        public void step()
        {
            for (short i = 0; i < playerCount; i++)
            {
                switch (keyDown[i])
                {
                    case "ArrowUp":
                        if (playerY[i] > 0 && maze[playerY[i] - 1].ToCharArray()[playerX[i]] != '*')
                            playerY[i]--;
                        break;
                    case "ArrowDown":
                        if (playerY[i] < heightDungeon && maze[playerY[i] + 1].ToCharArray()[playerX[i]] != '*')
                            playerY[i]++;
                        break;
                    case "ArrowLeft":
                        if (playerX[i] > 0 && maze[playerY[i]].ToCharArray()[playerX[i] - 1] != '*')
                            playerX[i]--;
                        break;
                    case "ArrowRight":
                        if (playerX[i] < widthDungeon && maze[playerY[i]].ToCharArray()[playerX[i] + 1] != '*')
                            playerX[i]++;
                        break;
                }
                keyDown[i] = "";

                for (short j = 0; j < itemCount; j++)
                {
                    if (itemX[j]==playerX[i] && itemY[j] == playerY[i])
                    {
                        playerScore[i] += itemValue[j];
                        randomPosition(out itemX[j], out itemY[j]);
                    }
                }

                for (short j=0; j < enemyCount; j++)
                {
                    if (enemyX[j] == playerX[i] && enemyY[j] == playerY[i])
                    {
                        playerScore[i] = 0;
                        randomPosition(out playerX[i], out playerY[i]);
                    }
                }
            }

            for (short i = 0; i < enemyCount; i++)
            {
                short ways = 0;

                if (maze[enemyY[i]].ToCharArray()[enemyX[i] + 1] == ' ') ways++;
                if (maze[enemyY[i]].ToCharArray()[enemyX[i] - 1] == ' ') ways++;
                if (maze[enemyY[i] + 1].ToCharArray()[enemyX[i]] == ' ') ways++;
                if (maze[enemyY[i] - 1].ToCharArray()[enemyX[i]] == ' ') ways++;
                if (ways > 2) enemyDirection[i] = (short)r.Next(4);

                switch (enemyDirection[i])
                {
                    case 0:
                        if (maze[enemyY[i]].ToCharArray()[enemyX[i] + 1] == ' ')
                            enemyX[i]++;
                        else
                            enemyDirection[i] = (short)r.Next(4);
                        break;
                    case 1:
                        if (maze[enemyY[i]].ToCharArray()[enemyX[i] - 1] == ' ')
                            enemyX[i]--;
                        else
                            enemyDirection[i] = (short)r.Next(4);
                        break;
                    case 2:
                        if (maze[enemyY[i] + 1].ToCharArray()[enemyX[i]] == ' ')
                            enemyY[i]++;
                        else
                            enemyDirection[i] = (short)r.Next(4);
                        break;
                    case 3:
                        if (maze[enemyY[i] - 1].ToCharArray()[enemyX[i]] == ' ')
                            enemyY[i]--;
                        else
                            enemyDirection[i] = (short)r.Next(4);
                        break;
                }

                for (short j = 0; j < playerCount; j++)
                {
                    if (enemyX[i] == playerX[j] && enemyY[i] == playerY[j])
                    {
                        playerScore[j] = 0;
                        randomPosition(out playerX[j], out playerY[j]);
                    }
                }
            }
        }

        public void draw()
        {
            for (short i = 0; i < playerCount+1; i++)
            {
                drawLayout(rows[i],i);
                drawMaze(rows[i]);

                for (short j = 0; j < playerCount; j++)
                {
                    rows[i][playerY[j] + 1].Cells[playerX[j] + 1].character = chPlayer;
                    rows[i][playerY[j] + 1].Cells[playerX[j] + 1].foreColor = playerColor[j];
                }

                for (short j = 0; j < itemCount; j++)
                {
                    switch (itemType[j])
                    {
                        case 0:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chCoin;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].foreColor = CoinColor;
                            break;
                        case 1:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chCherrie;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].foreColor = CherrieColor;
                            break;
                        case 2:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chGreenApple;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].foreColor = GreenAppleColor;
                            break;
                        case 3:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chRedApple;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].foreColor = RedAppleColor;
                            break;
                        case 4:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chStrawberry;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].foreColor = StrawberryColor;
                            break;
                    }
                }

                for (short j = 0; j < enemyCount; j++)
                {
                    rows[i][enemyY[j] + 1].Cells[enemyX[j] + 1].character = chEnemy;
                    rows[i][enemyY[j] + 1].Cells[enemyX[j] + 1].foreColor = Color.PaleVioletRed;
                }
            }
        }

        public void drawMaze(IList<Row> rows)
        {
            for (short y=0; y<heightDungeon; y++)
            {
                short x=0;
                CharEnumerator ch = maze[y].GetEnumerator();
                while (ch.MoveNext())
                {
                    switch (ch.Current.ToString())
                    {
                        case " ":
                            rows[y + 1].Cells[x + 1].character = ch.Current.ToString();
                            rows[y + 1].Cells[x + 1].foreColor = Color.Black;
                            rows[y + 1].Cells[x + 1].backColor = Color.Black;
                            break;
                        case "*":
                            rows[y + 1].Cells[x + 1].character = chWall;
                            rows[y + 1].Cells[x + 1].foreColor = Color.PaleGoldenrod;
                            rows[y + 1].Cells[x + 1].backColor = Color.Black;
                            break;
                    }
                    
                    x++;
                }
            }
        }

        public void randomPosition(out short x, out short y)
        {
            do
            {
                x = (short)r.Next(widthDungeon);
                y = (short)r.Next(heightDungeon);
            } while (maze[y].ToCharArray()[x] != ' ');
        }

        public void drawLayout(IList<Row> rows, short playerNumber)
        {
            short infopanelwidth = 11;

            for (short x = 0; x < width - infopanelwidth; x++)
            {
                rows[0].Cells[x].character = " ";
                rows[0].Cells[x].backColor = Color.CadetBlue;
                rows[height - 1].Cells[x].character = " ";
                rows[height - 1].Cells[x].backColor = Color.CadetBlue;
            }

            for (short y = 0; y < height; y++)
            {
                rows[y].Cells[0].character = " ";
                rows[y].Cells[0].backColor = Color.CadetBlue;
            }

            for (short x = (short)(width - infopanelwidth); x < width; x++)
                for (short y = 0; y < height; y++)
                {
                    rows[y].Cells[x].character = " ";
                    rows[y].Cells[x].backColor = Color.CadetBlue;
                }

            string title = "BLAZOR DUNGEON v1.0";
            drawText(title, (short)(((width - infopanelwidth) / 2) - (title.Length / 2)), 0, Color.Yellow, Color.CadetBlue, rows);

            drawText("SCORES", (short)(width - infopanelwidth + 3), 1, Color.Aquamarine, Color.CadetBlue, rows);

            for (short i = 0; i < playerCount; i++)
            {
                drawCh(chPlayer, (short)(width - infopanelwidth + 2), (short)(3 + i), playerColor[i], Color.Black, rows);
                drawText(string.Format(" {0:000000}", playerScore[i]), (short)(width - infopanelwidth + 3), (short)(3 + i), playerColor[i], Color.Black, rows);
            }

            if (playerNumber < playerCount)
            {
                drawText("YOU ", (short)(width - infopanelwidth + 3), 15, Color.Aquamarine, Color.CadetBlue, rows);
                drawCh(chPlayer, (short)(width - infopanelwidth + 8), 15, playerColor[playerNumber], Color.Black, rows);
            }else
            {
                drawText("ROOM FULL", (short)(width - infopanelwidth + 2), 15, Color.Red, Color.CadetBlue, rows);
            }

            drawText("POINTS", (short)(width - infopanelwidth + 3), 17, Color.Aquamarine, Color.CadetBlue, rows);
            drawCh(chCoin, (short)(width - infopanelwidth + 2), (short)(19), CoinColor, Color.Black, rows);
            drawText(" 10    ", (short)(width - infopanelwidth + 3), (short)(19), CoinColor, Color.Black, rows);
            drawCh(chCherrie, (short)(width - infopanelwidth + 2), (short)(20), CherrieColor, Color.Black, rows);
            drawText(" 20    ", (short)(width - infopanelwidth + 3), (short)(20), CherrieColor, Color.Black, rows);
            drawCh(chGreenApple, (short)(width - infopanelwidth + 2), (short)(21), GreenAppleColor, Color.Black, rows);
            drawText(" 30    ", (short)(width - infopanelwidth + 3), (short)(21), GreenAppleColor, Color.Black, rows);
            drawCh(chRedApple, (short)(width - infopanelwidth + 2), (short)(22), RedAppleColor, Color.Black, rows);
            drawText(" 40    ", (short)(width - infopanelwidth + 3), (short)(22), RedAppleColor, Color.Black, rows);
            drawCh(chStrawberry, (short)(width - infopanelwidth + 2), (short)(23), StrawberryColor, Color.Black, rows);
            drawText(" 50    ", (short)(width - infopanelwidth + 3), (short)(23), StrawberryColor, Color.Black, rows);
        }

        private void drawText(string text, short x, short y, Color foreColor, Color backColor, IList<Row> rows)
        {
            CharEnumerator ch = text.GetEnumerator();
            while (ch.MoveNext())
            {
                if (x < width && y < height)
                {
                    rows[y].Cells[x].character = ch.Current.ToString();
                    rows[y].Cells[x].foreColor = foreColor;
                    rows[y].Cells[x].backColor = backColor;
                }
                x++;
            }
        }

        private void drawCh(string ch, short x, short y, Color foreColor, Color backColor, IList<Row> rows)
        {
            rows[y].Cells[x].character = ch;
            rows[y].Cells[x].foreColor = foreColor;
            rows[y].Cells[x].backColor = backColor;
        }
    }
}
