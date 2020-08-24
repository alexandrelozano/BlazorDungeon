using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace BlazorDungeon.Code
{
    public class Game
    {
        public IList<Row>[] rows;

        public IList<Player> players;
        public IList<Enemy> enemies;
        public IList<Item> items;

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

            players = new List<Player>();
            for (short i = 0; i < 5; i++)
            {
                Player player=new Player();
                randomPosition(out short x, out short y);
                player.x = x;
                player.y = y;
                player.keyDown = new List<string>();

                player.sounds = new List<bool>();
                player.soundsTime = new List<DateTime>();
                for (short s = 0; s < 6; s++)
                {
                    player.sounds.Add(false);
                    player.soundsTime.Add(DateTime.Now.AddDays(-1));
                }

                players.Add(player);
            }

            rows = new List<Row>[players.Count+1];
            for (short i = 0; i < players.Count+1; i++)
            {
                Player player = new Player();

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

            chPlayer = Char.ConvertFromUtf32(1047636);
            chEnemy = Char.ConvertFromUtf32(1047634);
            chWall = Char.ConvertFromUtf32(1047550);
            chCoin = Char.ConvertFromUtf32(128308);
            chCherrie = Char.ConvertFromUtf32(127826);
            chRedApple = Char.ConvertFromUtf32(127822);
            chGreenApple = Char.ConvertFromUtf32(127823);
            chStrawberry = Char.ConvertFromUtf32(127827);

            enemies = new List<Enemy>();
            for (short i = 0; i < 10; i++)
            {
                Enemy enemy = new Enemy();
                randomPosition(out short x, out short y);
                enemy.x = x;
                enemy.y = y;
                enemy.direction = (short)(i % 4);
                enemies.Add(enemy);
            }

            items = new List<Item>();
            for (short i=0;i<30;i++)
            {
                Item item = new Item();
                randomPosition(out short x, out short y);
                item.x = x;
                item.y = y;

                if (i < 10) { item.type = 0; item.value = 10; item.sound = 1; }
                else if (i < 15) { item.type = 1; item.value = 20; item.sound = 2; }
                else if (i < 20) { item.type = 2; item.value = 30; item.sound = 3; }
                else if (i < 25) { item.type = 3; item.value = 40; item.sound = 4; }
                else { item.type = 4; item.value = 50; item.sound = 5; }

                items.Add(item);
            }

            gameSpeed = 200;

            gameTimer = new System.Timers.Timer();
            gameTimer.Interval = gameSpeed;
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
        }

        public void step()
        {
            foreach (Player player in players)
            {
                for (short j = 0; j < player.keyDown.Count; j++)
                {
                    switch (player.keyDown[j])
                    {
                        case "ArrowUp":
                            if (player.y > 0 && maze[player.y - 1].ToCharArray()[player.x] != '*')
                                player.y--;
                            break;
                        case "ArrowDown":
                            if (player.y < heightDungeon && maze[player.y + 1].ToCharArray()[player.x] != '*')
                                player.y++;
                            break;
                        case "ArrowLeft":
                            if (player.x > 0 && maze[player.y].ToCharArray()[player.x - 1] != '*')
                                player.x--;
                            break;
                        case "ArrowRight":
                            if (player.x < widthDungeon && maze[player.y].ToCharArray()[player.x + 1] != '*')
                                player.x++;
                            break;
                    }
                }
                
                foreach (Item item in items)
                {
                    if (item.x== player.x && item.y == player.y)
                    {
                        player.score += item.value;
                        player.soundsTime[item.sound] = DateTime.Now.AddSeconds(1);
                        randomPosition(out short x, out short y);
                        item.x = x;
                        item.y = y;
                    }
                }

                foreach(Enemy enemy in enemies)
                {
                    if (enemy.x == player.x && enemy.y == player.y)
                    {
                        player.score = 0;
                        player.soundsTime[0] = DateTime.Now.AddSeconds(1);
                        randomPosition(out short x, out short y);
                        player.x = x;
                        player.y = y;
                    }
                }
            }

            foreach (Enemy enemy in enemies)
            {
                short ways = 0;

                if (maze[enemy.y].ToCharArray()[enemy.x + 1] == ' ') ways++;
                if (maze[enemy.y].ToCharArray()[enemy.x - 1] == ' ') ways++;
                if (maze[enemy.y + 1].ToCharArray()[enemy.x] == ' ') ways++;
                if (maze[enemy.y - 1].ToCharArray()[enemy.x] == ' ') ways++;
                if (ways > 2) enemy.direction = (short)r.Next(4);

                switch (enemy.direction)
                {
                    case 0:
                        if (maze[enemy.y].ToCharArray()[enemy.x + 1] == ' ')
                            enemy.x++;
                        else
                            enemy.direction = (short)r.Next(4);
                        break;
                    case 1:
                        if (maze[enemy.y].ToCharArray()[enemy.x - 1] == ' ')
                            enemy.x--;
                        else
                            enemy.direction = (short)r.Next(4);
                        break;
                    case 2:
                        if (maze[enemy.y + 1].ToCharArray()[enemy.x] == ' ')
                            enemy.y++;
                        else
                            enemy.direction = (short)r.Next(4);
                        break;
                    case 3:
                        if (maze[enemy.y - 1].ToCharArray()[enemy.x] == ' ')
                            enemy.y--;
                        else
                            enemy.direction = (short)r.Next(4);
                        break;
                }

                for (short j = 0; j < players.Count; j++)
                {
                    if (enemy.x == players[j].x && enemy.y == players[j].y)
                    {
                        players[j].score = 0;
                        players[j].soundsTime[0] = DateTime.Now.AddSeconds(1);
                        randomPosition(out short x, out short y);
                        players[j].x = x;
                        players[j].y = y;
                    }
                }
            }

            foreach (Player player in players)
            {
                for (short j = 0; j < player.soundsTime.Count(); j++)
                {
                    if (player.soundsTime[j] < DateTime.Now)
                    {
                        player.sounds[j] = false;
                    }
                    else
                    {
                        player.sounds[j] = true;
                    }
                }
            }
        }

        public void draw()
        {
            for (short i = 0; i < players.Count+1; i++)
            {
                drawLayout(rows[i],i);
                drawMaze(rows[i]);

                for (short j = 0; j < players.Count; j++)
                {
                    rows[i][players[j].y + 1].Cells[players[j].x + 1].character = chPlayer;
                    rows[i][players[j].y + 1].Cells[players[j].x + 1].cssClass = cssPlayer+(j+1).ToString();
                }

                foreach (Item item in items)
                {
                    switch (item.type)
                    {
                        case 0:
                            rows[i][item.y + 1].Cells[item.x + 1].character = chCoin;
                            rows[i][item.y + 1].Cells[item.x + 1].cssClass = cssCoin;
                            break;
                        case 1:
                            rows[i][item.y + 1].Cells[item.x + 1].character = chCherrie;
                            rows[i][item.y + 1].Cells[item.x + 1].cssClass = cssCherrie;
                            break;
                        case 2:
                            rows[i][item.y + 1].Cells[item.x + 1].character = chGreenApple;
                            rows[i][item.y + 1].Cells[item.x + 1].cssClass = cssGreenApple;
                            break;
                        case 3:
                            rows[i][item.y + 1].Cells[item.x + 1].character = chRedApple;
                            rows[i][item.y + 1].Cells[item.x + 1].cssClass = cssRedApple;
                            break;
                        case 4:
                            rows[i][item.y + 1].Cells[item.x + 1].character = chStrawberry;
                            rows[i][item.y + 1].Cells[item.x + 1].cssClass = cssStrawberry;
                            break;
                    }
                }

                foreach (Enemy enemy in enemies)
                {
                    rows[i][enemy.y + 1].Cells[enemy.x + 1].character = chEnemy;
                    rows[i][enemy.y + 1].Cells[enemy.x + 1].cssClass = cssEnemy;
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

            for (short i = 0; i < players.Count; i++)
            {
                drawCharacter(chPlayer, (short)(width - infopanelwidth + 2), (short)(3 + i), cssPlayer + (i+1).ToString(), rows);
                drawText(string.Format(" {0:000000}", players[i].score), (short)(width - infopanelwidth + 3), (short)(3 + i), cssPlayer + (i + 1).ToString(), rows);
            }

            if (playerNumber < players.Count)
            {
                drawText("YOU ", (short)(width - infopanelwidth + 3), 15, cssMarginText, rows);
                drawCharacter(chPlayer, (short)(width - infopanelwidth + 8), 15, cssPlayer + (playerNumber+1).ToString(), rows);
            }else
            {
                drawText("ROOM FULL", (short)(width - infopanelwidth + 2), 15, cssRoomFull, rows);
            }

            drawText("POINTS", (short)(width - infopanelwidth + 3), 17, cssMarginText, rows);
            drawCharacter(chCoin, (short)(width - infopanelwidth + 2), (short)(19), cssCoin, rows);
            drawText(" 10    ", (short)(width - infopanelwidth + 3), (short)(19), cssCoin, rows);
            drawCharacter(chCherrie, (short)(width - infopanelwidth + 2), (short)(20), cssCherrie, rows);
            drawText(" 20    ", (short)(width - infopanelwidth + 3), (short)(20), cssCherrie,  rows);
            drawCharacter(chGreenApple, (short)(width - infopanelwidth + 2), (short)(21), cssGreenApple,  rows);
            drawText(" 30    ", (short)(width - infopanelwidth + 3), (short)(21), cssGreenApple, rows);
            drawCharacter(chRedApple, (short)(width - infopanelwidth + 2), (short)(22), cssRedApple, rows);
            drawText(" 40    ", (short)(width - infopanelwidth + 3), (short)(22), cssRedApple, rows);
            drawCharacter(chStrawberry, (short)(width - infopanelwidth + 2), (short)(23), cssStrawberry, rows);
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

        private void drawCharacter(string character, short x, short y, string cssClass, IList<Row> rows)
        {
            rows[y].Cells[x].character = character;
            rows[y].Cells[x].cssClass = cssClass;
        }
    }
}