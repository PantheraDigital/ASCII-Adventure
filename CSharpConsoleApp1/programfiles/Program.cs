using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using FastConsole;

namespace AsciiProgram
{
    
    public class GameWindow
    {
        //can add formatting options like center set text for message
        //needs support for '\n'
        Vector2 m_screenPosition;
        Vector2 m_windowSize;
        
        char m_backgroundChar;
        ConsoleColor m_windowForegroundColor;
        ConsoleColor m_windowBackgroundColor;

        bool m_textWrapping;
        string m_message;
        ConsoleColor m_messageForegroundColor;
        ConsoleColor m_messageBackgroundColor;

        bool m_active;
        int m_layer;
        FastWrite m_fastWrite;



        public GameWindow(Vector2 screenPosition, Vector2 windowSize, char backgroundChar = ' ', ConsoleColor windowForegroundColor = ConsoleColor.White, ConsoleColor windowBackgroundColor = ConsoleColor.Black)
        {
            m_screenPosition = screenPosition;
            m_windowSize = windowSize;
            m_backgroundChar = backgroundChar;
            m_windowForegroundColor = windowForegroundColor;
            m_windowBackgroundColor = windowBackgroundColor;
            m_messageForegroundColor = windowForegroundColor;
            m_messageBackgroundColor = windowBackgroundColor;
            
            m_message = "";
            m_textWrapping = false;

            m_active = true;
            m_layer = 0;
            m_fastWrite = FastWrite.GetInstance();
            m_fastWrite.InitializeBuffer((short)Console.WindowWidth, (short)Console.WindowHeight);
        }


        public Char GetBackgroundCharacter()
        {
            return m_backgroundChar;
        }

        public Vector2 GetWindowSize()
        {
            return m_windowSize;
        }

        public void SetActive(bool active)
        {
            m_active = active;

            if (active == false)
                m_fastWrite.ClearLayer(m_layer);
        }

        public void SetWindowSize(Vector2 size)
        {
            if (size.x > 0 && size.y > 0)
                m_windowSize = size;
        }

        public Vector2 GetScreenPosition()
        {
            return m_screenPosition;
        }

        public void SetScreenPosition(Vector2 position)
        {
            if (position.x > 0 && position.y > 0)
                m_screenPosition = position;
        }

        public void SetMessage(string message)
        {
            m_message = message;
        }
        public void SetMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            m_message = message;
            SetMessageColors(foregroundColor, backgroundColor);
        }

        public void SetTextWrapping(bool textWrapping)
        {
            m_textWrapping = textWrapping;
        }

        public void SetMessageColors(ConsoleColor foreground, ConsoleColor background)
        {
            m_messageForegroundColor = foreground;
            m_messageBackgroundColor = background;
        }
        
        public void SetWindowColros(ConsoleColor foreground, ConsoleColor background)
        {
            m_windowForegroundColor = foreground;
            m_windowBackgroundColor = background;
        }

        public void Draw(int layer)
        {
            if (!m_active)
                return;

            m_layer = layer;
            
            //fill window with background
            for(int y = 0; y < m_windowSize.y; ++y)
            {
                for(int x = 0; x < m_windowSize.x; ++x)
                {
                    m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, layer, m_backgroundChar, m_windowForegroundColor, m_windowBackgroundColor);
                }
            }

            //print message
            string messageToPrint = m_message;
            
            if (m_textWrapping)
            {
                if (messageToPrint.Length > m_windowSize.x)
                {
                    string[] words = messageToPrint.Split(' ');
                    string[] message = new string[m_windowSize.y];
                    int fullLines = 0;

                    for (int i = 0; i < words.Length; ++i)
                    {
                        bool useSpace = true;
                        if ((message[fullLines] + words[i]).Length > m_windowSize.x)
                        {
                            fullLines += 1;
                            useSpace = false;
                        }
                        else if (((message[fullLines] + words[i]).Length == m_windowSize.x) || 
                            ((message[fullLines] + words[i] + words[i + 1]).Length > m_windowSize.x))
                        {
                            useSpace = false;
                        }

                        if (fullLines < m_windowSize.y)
                        {
                            if (useSpace)
                                message[fullLines] += string.Format("{0} ", words[i]);
                            else
                                message[fullLines] += words[i];
                        }
                        else
                            fullLines -= 1;
                    }

                    for (int i = 0; i <= fullLines; ++i)
                    {
                        m_fastWrite.AddToBuffer(m_screenPosition.x, m_screenPosition.y + i, layer, message[i], m_messageForegroundColor, m_messageBackgroundColor);
                    }
                }
                else
                    m_fastWrite.AddToBuffer(m_screenPosition.x, m_screenPosition.y, layer, messageToPrint, m_messageForegroundColor, m_messageBackgroundColor);

            }
            else
            {
                //if message too long then stop printing at end of window
                if (messageToPrint.Length > m_windowSize.x)
                    m_fastWrite.AddToBuffer(m_screenPosition.x, m_screenPosition.y, layer, string.Join("", messageToPrint.Take(m_windowSize.x).ToArray()), m_messageForegroundColor, m_messageBackgroundColor);//Console.Write(string.Join("", messageToPrint.Take(m_windowSize.x).ToArray()));
                else
                    m_fastWrite.AddToBuffer(m_screenPosition.x, m_screenPosition.y, layer, messageToPrint, m_messageForegroundColor, m_messageBackgroundColor);

            }

            m_fastWrite.DisplayBuffer();
        }
    }

    public abstract class GameObject
    {
        public virtual void Update() { }
        public virtual void OnCollide(ref Entity other) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.Write('\u2592');//unicode
            
            */

            Console.CursorVisible = false;


            List<List<Tile>> levelLayout;
            string layout = "...........\n" +
                            "......O.....\n" +
                            ".......O.......\n" +
                            ".......O...\n" +
                            "...........\n";

            string mazeLevel =  "============================================\n" +
                                "=                =       =       =         =\n" +
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
                        testWindow2.SetActive(true);
                        testWindow2.Draw(1);
                        Console.ReadKey(true);

                        testWindow2.SetActive(false);
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

                default:
                    tileToAdd = new Tile(new DisplayObject(' ', ConsoleColor.Gray, ConsoleColor.Black, position));
                    tileToAdd.m_solid = true;
                    break;
            }

            return tileToAdd;
        }
    }
}
