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
            Console.SetWindowSize(80, 40);

            int halfWindowWidth = (int)(Console.WindowWidth / 2);
            int halfWindowHeight = (int)(Console.WindowHeight / 2);

            Console.CursorVisible = false;
            Console.Title = "Ascii Adventure";



            FastConsole.FastWrite.InitializeBuffer();

            PlayerController controller = new PlayerController();
            Stopwatch stopwatch = Stopwatch.StartNew();
            bool quit = false;
            float timeStep = 3.0f;
            int timesUpdated = 0;

            DisplayObject playerDisplay = new DisplayObject('&', ConsoleColor.Green, ConsoleColor.Black, new Vector2(1, 1));
            MovingEntity player = new MovingEntity(playerDisplay, controller);

            List<MovingEntity> players = new List<MovingEntity>();
            players.Add(player);

            Level level = SetUpLevel(LevelLayouts.mazeLevel1, player);

            int levelCenter = halfWindowWidth - (int)(level.GetMaxDimentions().x / 2);

            LevelCamera LevelCam = new LevelCamera(new Vector2(3,0), new Vector2(19,19), new Vector2(levelCenter, halfWindowHeight - (int)(level.GetMaxDimentions().y / 2)));

            GameWindow quitWindow = new GameWindow(new Vector2(halfWindowWidth - (int)(25 / 2), halfWindowHeight - (int)(5 / 2)), new Vector2(25, 6), '-', ConsoleColor.Gray);
            quitWindow.SetMessage("Quitting game\n\n\nPress any key to leave");
            quitWindow.SetBorderChar('\\');
            quitWindow.SetBorderColor(ConsoleColor.DarkRed, ConsoleColor.Black);

            GameWindow startWindow = new GameWindow(new Vector2(halfWindowWidth - (int)(25 / 2), halfWindowHeight - (int)(7 / 2)), new Vector2(25, 7), '-', ConsoleColor.Magenta, ConsoleColor.Black);
            startWindow.SetMessage("\n\n-Press any key to play-", ConsoleColor.Cyan, ConsoleColor.Black);
            startWindow.SetTextWrapping(true);
            startWindow.SetBorderChar('*');

            startWindow.Draw(1);
            Console.ReadKey(true);
            startWindow.Erase();

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

            quitWindow.Draw(1);
            Console.ReadKey(true);
        }

        public static Level SetUpLevel(string levelLayout, MovingEntity player)
        {
            List<List<Tile>> layout = new List<List<Tile>>();
            Dictionary<Vector2, GameObject> gameObjects = new Dictionary<Vector2, GameObject>();
            List<MovingEntity> entities = new List<MovingEntity>();
            Vector2 spawn = new Vector2(0,0);

            string[] rows = levelLayout.Split('\n');

            entities.Add(player);

            
            for(int y = 0; y < rows.Length; ++y)
            {
                if(rows[y].Length > 0)
                {
                    layout.Add(new List<Tile>());

                    for (int x = 0; x < rows[y].Length; ++x)
                    {
                        Tile tempTile = CreateTile(rows[y][x], new Vector2(x, y));
                        if (tempTile != null)
                            layout[y].Add(tempTile);
                        else
                            layout[y].Add(CreateTile('.', new Vector2(x, y)));

                        GameObject gameObject = CreateGameObject(rows[y][x], new Vector2(x, y));
                        if (gameObject != null)
                            gameObjects.Add(new Vector2(x, y), gameObject);

                        if (rows[y][x] == '0')
                            spawn = new Vector2(x, y);
                    }
                }    
            }
            
            return new Level(layout, entities, gameObjects, spawn, player);
        }

        public static Tile CreateTile(char tileType, Vector2 position)
        {
            Tile tileToAdd = null;
            switch (tileType)
            {
                case '.'://background 
                    tileToAdd = new Tile(new DisplayObject(tileType, ConsoleColor.DarkGray, ConsoleColor.Black, position));
                    break;

                case '='://wall
                    tileToAdd = new Tile(new DisplayObject(tileType, ConsoleColor.Gray, ConsoleColor.Black, position));
                    tileToAdd.m_solid = true;
                    break;

                case ' '://background
                    tileToAdd = new Tile(new DisplayObject('.', ConsoleColor.DarkGray, ConsoleColor.Black, position));
                    break;

            }

            return tileToAdd;
        }

        public static GameObject CreateGameObject(char objectType, Vector2 position)
        {
            GameObject objectToAdd = null;
            switch (objectType)
            {
                case '^':
                    GameWindow window = new GameWindow(new Vector2(1, 1), new Vector2(9, 5), '-', ConsoleColor.Magenta, ConsoleColor.Black);
                    window.SetMessage("Hello\nThere", ConsoleColor.Cyan, ConsoleColor.Black);
                    window.SetTextWrapping(true);

                    objectToAdd = new NoteObject(new DisplayObject(objectType, position), window);
                    break;
            }


            return objectToAdd;
        }
    }
}
