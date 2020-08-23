using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Timers;

namespace BlazorDungeon.Data
{
    public class Game
    {
        public IList<Row>[] rows;
        public IList<bool>[] sounds;
        public IList<DateTime>[] soundsTime;

        public IList<string>[] keyDown;

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
        public short[] itemSound;
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
        public string chCherrie;
        public string chRedApple;
        public string chGreenApple;
        public string chStrawberry;

        private const string cssWall = "a";
        private const string cssCorridor = "b";
        private const string cssMarginTitle = "c";
        private const string cssMarginText = "d";
        private const string cssRoomFull = "e";
        private const string cssPlayer = "p";
        private const string cssEnemy = "f";
        private const string cssCoin = "g";
        private const string cssCherrie = "h";
        private const string cssGreenApple = "i";
        private const string cssRedApple = "j";
        private const string cssStrawberry = "k";

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
                        c.cssClass = cssCorridor;
                        c.character = " ";
                        Cells.Add(c);
                    }

                    r.Cells = Cells;
                    rows[i].Add(r);
                }
            }

            sounds = new List<bool>[playerCount + 1];
            soundsTime = new List<DateTime>[playerCount + 1];
            for (short i = 0; i < playerCount + 1; i++)
            {
                sounds[i] = new List<bool>();
                sounds[i].Add(false);
                sounds[i].Add(false);
                sounds[i].Add(false);
                sounds[i].Add(false);
                sounds[i].Add(false);
                sounds[i].Add(false);

                soundsTime[i] = new List<DateTime>();
                soundsTime[i].Add(DateTime.Now.AddDays(-1));
                soundsTime[i].Add(DateTime.Now.AddDays(-1));
                soundsTime[i].Add(DateTime.Now.AddDays(-1));
                soundsTime[i].Add(DateTime.Now.AddDays(-1));
                soundsTime[i].Add(DateTime.Now.AddDays(-1));
                soundsTime[i].Add(DateTime.Now.AddDays(-1));
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

            keyDown = new List<string>[playerCount];
            for (short i = 0; i < playerCount; i++)
            {
                keyDown[i] = new List<string>();
            }

            playerX = new short[playerCount];
            playerY = new short[playerCount];
            playerScore = new int[playerCount];
            playerSessionId = new Guid[playerCount];
            for (short i = 0; i < playerCount; i++)
            {
                randomPosition(out playerX[i], out playerY[i]);
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
            itemSound = new short[itemCount];
            itemType = new short[itemCount];
            for (short i = 0; i < itemCount; i++)
            {
                randomPosition(out itemX[i], out itemY[i]);

                if (i < 10) { itemType[i] = 0; itemValue[i] = 10; itemSound[i] = 1; }
                else if (i < 15) { itemType[i] = 1; itemValue[i] = 20; itemSound[i] = 2; }
                else if (i < 20) { itemType[i] = 2; itemValue[i] = 30; itemSound[i] = 3; }
                else if (i < 25) { itemType[i] = 3; itemValue[i] = 40; itemSound[i] = 4; }
                else { itemType[i] = 4; itemValue[i] = 50; itemSound[i] = 5; }
            }

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
                for (short j = 0; j < keyDown[i].Count; j++)
                {
                    switch (keyDown[i][j])
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
                }
                
                for (short j = 0; j < itemCount; j++)
                {
                    if (itemX[j]==playerX[i] && itemY[j] == playerY[i])
                    {
                        playerScore[i] += itemValue[j];
                        soundsTime[i][itemSound[j]] = DateTime.Now.AddSeconds(1);
                        randomPosition(out itemX[j], out itemY[j]);
                    }
                }

                for (short j=0; j < enemyCount; j++)
                {
                    if (enemyX[j] == playerX[i] && enemyY[j] == playerY[i])
                    {
                        playerScore[i] = 0;
                        soundsTime[i][0] = DateTime.Now.AddSeconds(1);
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
                        soundsTime[j][0] = DateTime.Now.AddSeconds(1);
                        randomPosition(out playerX[j], out playerY[j]);
                    }
                }
            }

            for (short i=0; i < playerCount; i++)
            {
                for (short j = 0; j < soundsTime.Count(); j++)
                {
                    if (soundsTime[i][j] < DateTime.Now)
                    {
                        sounds[i][j] = false;
                    }
                    else
                    {
                        sounds[i][j] = true;
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
                    rows[i][playerY[j] + 1].Cells[playerX[j] + 1].cssClass = cssPlayer+(j+1).ToString();
                }

                for (short j = 0; j < itemCount; j++)
                {
                    switch (itemType[j])
                    {
                        case 0:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chCoin;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].cssClass = cssCoin;
                            break;
                        case 1:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chCherrie;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].cssClass = cssCherrie;
                            break;
                        case 2:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chGreenApple;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].cssClass = cssGreenApple;
                            break;
                        case 3:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chRedApple;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].cssClass = cssRedApple;
                            break;
                        case 4:
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].character = chStrawberry;
                            rows[i][itemY[j] + 1].Cells[itemX[j] + 1].cssClass = cssStrawberry;
                            break;
                    }
                }

                for (short j = 0; j < enemyCount; j++)
                {
                    rows[i][enemyY[j] + 1].Cells[enemyX[j] + 1].character = chEnemy;
                    rows[i][enemyY[j] + 1].Cells[enemyX[j] + 1].cssClass = cssEnemy;
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
                            rows[y + 1].Cells[x + 1].cssClass = cssCorridor;
                            break;
                        case "*":
                            rows[y + 1].Cells[x + 1].character = chWall;
                            rows[y + 1].Cells[x + 1].cssClass = cssWall;
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
                rows[0].Cells[x].cssClass = cssMarginTitle;
                rows[height - 1].Cells[x].character = " ";
                rows[height - 1].Cells[x].cssClass = cssMarginTitle;
            }

            for (short y = 0; y < height; y++)
            {
                rows[y].Cells[0].character = " ";
                rows[y].Cells[0].cssClass = cssMarginTitle;
            }

            for (short x = (short)(width - infopanelwidth); x < width; x++)
                for (short y = 0; y < height; y++)
                {
                    rows[y].Cells[x].character = " ";
                    rows[y].Cells[x].cssClass = cssMarginTitle;
                }

            string title = "BLAZOR DUNGEON v1.0";
            drawText(title, (short)(((width - infopanelwidth) / 2) - (title.Length / 2)), 0, cssMarginTitle, rows);

            drawText("SCORES", (short)(width - infopanelwidth + 3), 1, cssMarginText, rows);

            for (short i = 0; i < playerCount; i++)
            {
                drawCh(chPlayer, (short)(width - infopanelwidth + 2), (short)(3 + i), cssPlayer + (i+1).ToString(), rows);
                drawText(string.Format(" {0:000000}", playerScore[i]), (short)(width - infopanelwidth + 3), (short)(3 + i), cssPlayer + (i + 1).ToString(), rows);
            }

            if (playerNumber < playerCount)
            {
                drawText("YOU ", (short)(width - infopanelwidth + 3), 15, cssMarginText, rows);
                drawCh(chPlayer, (short)(width - infopanelwidth + 8), 15, cssPlayer + (playerNumber+1).ToString(), rows);
            }else
            {
                drawText("ROOM FULL", (short)(width - infopanelwidth + 2), 15, cssRoomFull, rows);
            }

            drawText("POINTS", (short)(width - infopanelwidth + 3), 17, cssMarginText, rows);
            drawCh(chCoin, (short)(width - infopanelwidth + 2), (short)(19), cssCoin, rows);
            drawText(" 10    ", (short)(width - infopanelwidth + 3), (short)(19), cssCoin, rows);
            drawCh(chCherrie, (short)(width - infopanelwidth + 2), (short)(20), cssCherrie, rows);
            drawText(" 20    ", (short)(width - infopanelwidth + 3), (short)(20), cssCherrie,  rows);
            drawCh(chGreenApple, (short)(width - infopanelwidth + 2), (short)(21), cssGreenApple,  rows);
            drawText(" 30    ", (short)(width - infopanelwidth + 3), (short)(21), cssGreenApple, rows);
            drawCh(chRedApple, (short)(width - infopanelwidth + 2), (short)(22), cssRedApple, rows);
            drawText(" 40    ", (short)(width - infopanelwidth + 3), (short)(22), cssRedApple, rows);
            drawCh(chStrawberry, (short)(width - infopanelwidth + 2), (short)(23), cssStrawberry, rows);
            drawText(" 50    ", (short)(width - infopanelwidth + 3), (short)(23), cssStrawberry, rows);
        }

        private void drawText(string text, short x, short y, string cssClass, IList<Row> rows)
        {
            CharEnumerator ch = text.GetEnumerator();
            while (ch.MoveNext())
            {
                if (x < width && y < height)
                {
                    rows[y].Cells[x].character = ch.Current.ToString();
                    rows[y].Cells[x].cssClass = cssClass;
                }
                x++;
            }
        }

        private void drawCh(string ch, short x, short y, string cssClass, IList<Row> rows)
        {
            rows[y].Cells[x].character = ch;
            rows[y].Cells[x].cssClass = cssClass;
        }
    }
}
