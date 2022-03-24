using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace AsciiProgram
{

    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.Write('\u2592');//unicode
            
            */

            Console.CursorVisible = false;


            List<List<Tile>> levelLayout;

            string mazeLevel =  "============================================\n" +
                                "=      *         =       =       =         =\n" +
                                "=== ============ =   === =     = ===== === =\n" +
                                "=              = === =   ===== =         = =\n" +
                                "= ========= == = =   = ===   = = ===== === =\n" +
                                "= =   =   = =  =   = =     =   = =     = ===\n" +
                                "= = =   = = =  ===== =========== = =====   =\n" +
                                "= = ===== = = ==     =       =   =     === =\n" +
                                "= =     = = =  = = = = ===== = ======= =   =\n" +
                                "= ===== = = == = = =   =     =   =   = = ===\n" +
                                "=       =   =  = = ===== ========= =   =   =\n" +
                                "=========== = == =     =         = === =====\n" +
                                "=           = =  ===== =     ===   ===     =\n" +
                                "= =========== === =    ===== ========  =====\n" +
                                "= =           =   = ====     =             =\n" +
                                "= =================    = ===================\n" +
                                "= =   =      =         =       =           =\n" +
                                "= = = === == = =========  ==== = ==== ==== =\n" +
                                "=   =      =   =                      =    =\n" +
                                "============================================\n";

            levelLayout = SetUpLevel(mazeLevel);
            

            PlayerController controller = new PlayerController();
            Stopwatch stopwatch = Stopwatch.StartNew();
            bool quit = false;
            float timeStep = 3.0f;
            int timesUpdated = 0;

            DisplayObject playerDisplay = new DisplayObject('&', ConsoleColor.Green, ConsoleColor.Black, new Vector2(1, 1));
            MovingEntity player = new MovingEntity(playerDisplay, controller);
            List<MovingEntity> players = new List<MovingEntity>();
            players.Add(player);
            Level level = new Level(levelLayout, players);

            int half = (int)(Console.WindowWidth / 2);
            half = half - (int)(level.GetMaxDimentions().x / 2);
            level.SetDrawOffset(new Vector2(half, 3));

            LevelCamera LevelCam = new LevelCamera(new Vector2(3,0), new Vector2(19,19), new Vector2(half, 3));

            GameWindow testWindow = new GameWindow(new Vector2(5, 5), new Vector2(10, 5), '/', ConsoleColor.White, ConsoleColor.DarkRed);
            testWindow.SetMessage("Quit", ConsoleColor.Blue, ConsoleColor.DarkGray);
            testWindow.SetTextWrapping(true);

            GameWindow testWindow2 = new GameWindow(new Vector2(half, 5), new Vector2(10, 5), '-', ConsoleColor.Magenta, ConsoleColor.Black);
            testWindow2.SetMessage("Hello There", ConsoleColor.Cyan, ConsoleColor.Black);
            testWindow2.SetTextWrapping(true);
            

            while (quit == false)
            {
                level.Update();
                
                LevelCam.CenterCameraOn(player.GetCurrentPosition());
                LevelCam.UpdateDisplayList(level);
                LevelCam.Draw(0);
                

                if (controller.HasInput())
                {
                    if (controller.GetInput().Key == ConsoleKey.Q)
                        quit = true;
                    else if (controller.GetInput().Key == ConsoleKey.E)
                    {
                        testWindow2.Draw(1);
                        Console.ReadKey(true);

                        testWindow2.Erase();
                    }
                }

                /*
                if (stopwatch.IsRunning && stopwatch.Elapsed.TotalSeconds >= timeStep)
                {
                    //Console.SetCursorPosition(6, 6);
                    //Console.Write("No Input");

                    ++timesUpdated;
                    Console.SetCursorPosition(6, 5);
                    Console.Write("Update num: " + timesUpdated);
                    stopwatch.Restart();
                }

                if (controller.HasInput())
                {
                    if (controller.m_input.Key == ConsoleKey.Q)
                        quit = true;

                    //stopwatch.Restart();
                    Console.SetCursorPosition(6, 6);
                    Console.Write("Input: " + controller.m_input.KeyChar);
                }*/
            }

            testWindow.Draw(1);
            Console.ReadKey(true);
        }

        public static List<List<Tile>> SetUpLevel(string levelLayout)
        {
            List<List<Tile>> layout = new List<List<Tile>>();
            string[] rows = levelLayout.Split('\n');
            
            for(int y = 0; y < rows.Length; ++y)
            {
                if(rows[y].Length > 0)
                {
                    layout.Add(new List<Tile>());

                    for (int x = 0; x < rows[y].Length; ++x)
                    {
                        layout[y].Add(CreateTile(rows[y][x], new Vector2(x, y)));
                    }
                }    
            }
            
            return layout;
        }

        public static Tile CreateTile(char tileType, Vector2 position)
        {
            Tile tileToAdd = null;
            switch (tileType)
            {
                case '.'://background 
                    tileToAdd = new Tile(new DisplayObject(tileType, ConsoleColor.Gray, ConsoleColor.Black, position));
                    break;

                case 'O'://wall
                    tileToAdd = new Tile(new DisplayObject(tileType, ConsoleColor.White, ConsoleColor.Black, position));
                    tileToAdd.m_solid = true;
                    break;

                case '='://wall
                    tileToAdd = new Tile(new DisplayObject(tileType, ConsoleColor.Gray, ConsoleColor.Black, position));
                    tileToAdd.m_solid = true;
                    break;

                case ' ':
                    tileToAdd = new Tile(new DisplayObject('.', ConsoleColor.DarkGray, ConsoleColor.Black, position));
                    break;

                case '*':
                    GameWindow testWindow2 = new GameWindow(new Vector2(1, 0), new Vector2(10, 5), '-', ConsoleColor.Magenta, ConsoleColor.Black);
                    testWindow2.SetMessage("Hello There", ConsoleColor.Cyan, ConsoleColor.Black);
                    testWindow2.SetTextWrapping(true);

                    tileToAdd = new TriggerTile(new DisplayObject('*', ConsoleColor.DarkGray, ConsoleColor.Black, position), new ShowWindowTrigger(testWindow2, 2));
                    break;

                default:
                    tileToAdd = new Tile(new DisplayObject(' ', ConsoleColor.Gray, ConsoleColor.Black, position));
                    tileToAdd.m_solid = true;
                    break;
            }

            return tileToAdd;
        }
    }
}
