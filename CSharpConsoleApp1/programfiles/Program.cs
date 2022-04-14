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
        static LevelManager game;// = new LevelManager("mazeLevel1", player);
        

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
            FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();

            PlayerController controller = new PlayerController();
            Stopwatch stopwatch = Stopwatch.StartNew();
            //bool quit = false;
            float timeStep = 3.0f;
            int timesUpdated = 0;

            DisplayObject playerDisplay = new DisplayObject('&', ConsoleColor.Green, ConsoleColor.Black, new Vector2(1, 1));
            ComplexEntity player = new ComplexEntity(playerDisplay, controller, "player");


            GameWindow pInv = new GameWindow("PlayerInv", new Vector2(Console.WindowWidth - 21, Console.WindowHeight - 17), new Vector2(11, 6), '-', ConsoleColor.Gray);
            pInv.SetBorderChar('\\');
            pInv.SetBorderColor(ConsoleColor.DarkRed, ConsoleColor.Black);
            pInv.SetShowName(true);
            player.AddComponent(new Inventory(pInv,27));
            
            //List<MovingEntity> players = new List<MovingEntity>();
            //players.Add(player);

            
            GameWindow quitWindow = new GameWindow("quitWindow", new Vector2(halfWindowWidth - (int)(25 / 2), halfWindowHeight - (int)(5 / 2)), new Vector2(25, 6), '-', ConsoleColor.Gray);
            quitWindow.SetMessage("Quitting game\n\n\nPress any key to leave");
            quitWindow.SetBorderChar('\\');
            quitWindow.SetBorderColor(ConsoleColor.DarkRed, ConsoleColor.Black);
            
            GameWindow startWindow = new GameWindow("startWindow", new Vector2(halfWindowWidth - (int)(25 / 2), halfWindowHeight - (int)(7 / 2)), new Vector2(25, 7), '-', ConsoleColor.Magenta, ConsoleColor.Black);
            startWindow.SetMessage("\n\n-Press any key to play-", ConsoleColor.Cyan, ConsoleColor.Black);
            //startWindow.SetTextWrapping(true);
            startWindow.SetBorderChar('*');

            startWindow.Draw();
            fastWrite.DisplayBuffer();
            Console.ReadKey(true);
            startWindow.Erase();

            fastWrite.DisplayBuffer();


            game = new LevelManager();
            game.Initialize("mazeLevel2", player);
            game.Run();


            quitWindow.Draw();
            fastWrite.DisplayBuffer();
            Console.ReadKey(true);
        }

        public static void EventTest(object sender, string args)
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
        }

        public static Level SetUpLevel(string levelName, MovingEntity player)
        {
            List<List<Tile>> layout = new List<List<Tile>>();
            Dictionary<Vector2, GameObject> gameObjects = new Dictionary<Vector2, GameObject>();
            List<MovingEntity> entities = new List<MovingEntity>();
            Vector2 spawn = new Vector2(0,0);

            string[] rows = GetLevelLayout(levelName).Split('\n');

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

                        GameObject gameObject = CreateGameObject(levelName, rows[y][x], new Vector2(x, y));
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

        public static string GetLevelLayout(string layoutName)
        {
            switch (layoutName)
            {
                case "mazeLevel1":
                    return LevelLayouts.mazeLevel1;

                case "mazeLevel2":
                    return LevelLayouts.mazeLevel2;

                default:
                    return LevelLayouts.mazeLevel1;
            }
        }

        public static GameObject CreateGameObject(string levelName, char objectType, Vector2 position)
        {
            GameObject objectToAdd = null;
            switch (objectType)
            {
                case '^':
                    GameWindow gameWindow = NoteWindowMaker.CreateNoteWindow(levelName);
                    if (gameWindow != null)
                        objectToAdd = new NoteObject(new DisplayObject(objectType, position), gameWindow);
                    break;

                case 'k':
                    objectToAdd = new KeyObject(new DisplayObject(objectType, position));
                    break;


                case '!':
                    objectToAdd = new DoorObject(new DisplayObject(objectType, position));
                    break;

                case '*':
                    string nextLevel = LevelChangeTracker.GetNextLevelChange(levelName);
                    if(nextLevel != null)
                    {
                        LevelChangeObject temp = new LevelChangeObject(new DisplayObject(objectType, ConsoleColor.Green, ConsoleColor.Black, position), nextLevel);
                        temp.LevelChange += game.ChangeLevel;
                        objectToAdd = temp;
                    }
                    break;
            }


            return objectToAdd;
        }
    }
}
