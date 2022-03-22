using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using FastConsole;

namespace AsciiProgram
{
    

    public class Tile
    {
        public DisplayObject m_displayObject { get; protected set; }
        public bool m_solid { get; set; }

        public Tile(DisplayObject displayObject)
        {
            m_displayObject = displayObject;
            m_solid = false;
        }

        public virtual void Update() { }
        public virtual void OnCollide() { }
    }

    public class DisplayObject
    {
        public char m_spriteChar { get; set; }
        public ConsoleColor m_foregroudColor { get; set; }
        public ConsoleColor m_backgroundColor { get; set; }
        public Vector2 m_displayPosition { get; set; }


        public DisplayObject(char spriteChar, ConsoleColor foregroundColor, ConsoleColor backgroundColor, Vector2 displayPosition)
        {
            m_spriteChar = spriteChar;
            m_foregroudColor = foregroundColor;
            m_backgroundColor = backgroundColor;
            m_displayPosition = displayPosition;
        }
        public DisplayObject(char spriteChar, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            : this(spriteChar, foregroundColor, backgroundColor, new Vector2())
        { }
        public DisplayObject(char spriteChar)
            : this(spriteChar, ConsoleColor.White, ConsoleColor.Black, new Vector2())
        { }
        public DisplayObject(char spriteChar, Vector2 displayPosition)
            : this(spriteChar, ConsoleColor.White, ConsoleColor.Black, displayPosition)
        { }
        

        public bool IsEqual(DisplayObject other, bool testPosition = false)
        {
            if (m_spriteChar == other.m_spriteChar && m_foregroudColor == other.m_foregroudColor && m_backgroundColor == other.m_backgroundColor)
            {
                if (testPosition && m_displayPosition.IsEqual(other.m_displayPosition))
                    return true;
                else if (testPosition && !m_displayPosition.IsEqual(other.m_displayPosition))
                    return false;
                else
                    return true;
            }

            return false;
        }
    }

    public abstract class Entity//concrete entities will define what controllers do when given input such as move or act some way
    {
        protected Controller m_controller;
        public DisplayObject m_displayObject { get; protected set; }


        public Entity(DisplayObject displayObject, Controller controller)
        {
            m_displayObject = displayObject;
            m_controller = controller;
        }

        public abstract void Update();

        public virtual Vector2 GetCurrentPosition()
        {
            return m_displayObject.m_displayPosition;
        }
    }

    public class MovingEntity : Entity//make interface? IMove? 
    {
        Vector2 m_movePosition;
        bool m_moved;


        public MovingEntity(DisplayObject displayObject, Controller controller)
            : base(displayObject, controller)
        {
            m_movePosition = m_displayObject.m_displayPosition;
            m_moved = false;
        }

        public override void Update()
        {
            if (m_controller == null || m_displayObject == null)
                return;


            m_controller.Update();

            if (m_moved)
                m_moved = false;
            else
                m_movePosition = m_displayObject.m_displayPosition;


            if (m_controller.HasInput())
            {
                switch (m_controller.GetInput().Key)
                {
                    case ConsoleKey.W:
                        m_movePosition.y = m_displayObject.m_displayPosition.y - 1;
                        break;
                    case ConsoleKey.S:
                        m_movePosition.y = m_displayObject.m_displayPosition.y + 1;
                        break;
                    case ConsoleKey.A:
                        m_movePosition.x = m_displayObject.m_displayPosition.x - 1;
                        break;
                    case ConsoleKey.D:
                        m_movePosition.x = m_displayObject.m_displayPosition.x + 1;
                        break;
                    default:
                        break;
                }
            }

        }

        public Vector2 GetMoveLocation()
        {
            return m_movePosition;
        }

        public void Move()
        {
            m_displayObject.m_displayPosition = m_movePosition;
            m_moved = true;
        }

    }

    public abstract class Controller
    {
        public abstract void Update();
        public abstract ConsoleKeyInfo GetInput();
        public abstract bool HasInput();
    }

    public class PlayerController : Controller
    {
        ConsoleKeyInfo m_input;
        bool m_hasInput;

        public override void Update()
        {
            if (Console.KeyAvailable == true)
            {
                m_input = Console.ReadKey(true);
                m_hasInput = true;
            }
            else
                m_hasInput = false;
        }

        public override bool HasInput()
        {
            return m_hasInput;
        }

        public override ConsoleKeyInfo GetInput()
        {
            return m_input;
        }
    }

    public class Level
    {
        List<List<Tile>> m_tiles;
        List<MovingEntity> m_movingEntities;
        Vector2 m_drawOffset;
        Vector2 m_maxDimensions;
        

        public Level(List<List<Tile>> tiles, List<MovingEntity> movingEntities)
        {
            m_tiles = tiles;
            m_movingEntities = movingEntities;


            Vector2 temp = new Vector2(0, 0);
            temp.y = m_tiles.Count;
            for (int y = 0; y < m_tiles.Count; ++y)
            {
                if (m_tiles[y].Count - 1 > temp.x)
                    temp.x = m_tiles[y].Count;
            }

            m_maxDimensions = temp;
        }
        public Level(List<List<Tile>> tiles, List<MovingEntity> movingEntities, Vector2 drawOffset)
            : this(tiles, movingEntities)
        {
            m_drawOffset = drawOffset;
        }

        public void Update()
        {
            for (int y = 0; y < m_tiles.Count; ++y)
            {
                for (int x = 0; x < m_tiles[y].Count; ++x)
                {
                    m_tiles[y][x].Update();
                }
            }

            for (int i = 0; i < m_movingEntities.Count; ++i)
            {
                m_movingEntities[i].Update();

                if (!m_movingEntities[i].GetMoveLocation().IsEqual(m_movingEntities[i].GetCurrentPosition()))
                {
                    if (ValidateMove(m_movingEntities[i].GetMoveLocation()))
                    {
                        Vector2 coverdTilePos = m_movingEntities[i].GetCurrentPosition();

                        m_movingEntities[i].Move();
                    }
                }
            }
        }

        public void SetDrawOffset(Vector2 offset)
        {
            m_drawOffset = offset;
        }

        bool ValidateMove(Vector2 positionToValidate)
        {
            if (positionToValidate.y >= 0 && positionToValidate.y < m_tiles.Count)
            {
                if (positionToValidate.x >= 0 && positionToValidate.x < m_tiles[positionToValidate.y].Count)
                    if (!m_tiles[positionToValidate.y][positionToValidate.x].m_solid)
                        return true;
            }

            return false;
        }

        public Vector2 GetMaxDimentions()
        {
            return m_maxDimensions;
        }

        public Tile GetTile(int indexX, int indexY)
        {
            if (ValidTile(indexX, indexY))
                return m_tiles[indexY][indexX];
            
            return new Tile(new DisplayObject('*', new Vector2(indexX, indexY)));
        }

        public bool ValidTile(int indexX, int indexY)
        {
            if (indexY >= 0 && indexY < GetNumRows())
            {
                if (indexX >= 0 && indexX < GetRowLength(indexY))
                    return true;
            }
            
            return false;
        }

        public int GetRowLength(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < GetNumRows())
                return m_tiles[rowIndex].Count;

            return 0;
        }

        public int GetNumRows()
        {
            return m_tiles.Count;
        }

        public MovingEntity GetMovingEntity(int index)
        {
            if (index >= 0 && index < m_movingEntities.Count)
                return m_movingEntities[index];

            return null;
        }

        public int GetNumMovingEntities()
        {
            return m_movingEntities.Count;
        }
    }

    public class LevelCamera
    {
        struct DisplayData
        {
            public DisplayObject display;
            public bool fresh;

            public DisplayData(DisplayObject display, bool fresh)
            {
                this.display = display;
                this.fresh = fresh;
            }
        }

        Dictionary<Vector2, DisplayData> m_displayList;

        Vector2 m_displayOffset;//place on screen to display
        Vector2 m_displayBounds;//how much is displayed
        Vector2 m_cameraPosition;//cam pos in level

        FastWrite m_fastWrite;
        char m_emptyChar;
        

        public LevelCamera(Vector2 position, Vector2 displayBounds, Vector2 displayOffset)
        {
            m_displayOffset = displayOffset;
            m_cameraPosition = position;
            m_emptyChar = ' ';

            InitalizeDisplaySize(displayBounds);

            m_fastWrite = FastWrite.GetInstance();
            m_fastWrite.InitializeBuffer((short)Console.WindowWidth, (short)Console.WindowHeight);
        }


        public void SetEmptyChar(char emptyChar)
        {
            m_emptyChar = emptyChar;
        }

        public void SetDisplayOffset(Vector2 offset)
        {
            if (offset.x > 0 && offset.y > 0)
                m_displayOffset = offset;
        }

        public void SetCameraPosition(Vector2 position)
        {
            m_cameraPosition = position;
        }

        public void CenterCameraOn(Vector2 position)
        {
            position.x -= (int)(m_displayBounds.x / 2);
            position.y -= (int)(m_displayBounds.y / 2);
            SetCameraPosition(position);
        }

        public void InitalizeDisplaySize(Vector2 size)
        {
            m_displayList = new Dictionary<Vector2, DisplayData>();

            for(int y = 0; y < size.y; ++y)
            {
                for (int x = 0; x < size.x; ++x)
                    m_displayList.Add(new Vector2(x, y), new DisplayData(new DisplayObject(m_emptyChar, new Vector2(x, y)), true));
            }

            m_displayBounds = size;
        }
        
        public void UpdateDisplayList(Level currentLevel)
        {
            foreach (Vector2 key in m_displayList.Keys.ToList())
            {
                int levelSpaceY = m_cameraPosition.y + key.y;
                int levelSpaceX = m_cameraPosition.x + key.x;
                
                //if position has a tile (if position in camera space has a tile in it)
                if (currentLevel.ValidTile(levelSpaceX, levelSpaceY))
                {
                    //lookup display object at screenspace location
                    //if not equal to display at location in level
                    if (!m_displayList[key].display.IsEqual(currentLevel.GetTile(levelSpaceX, levelSpaceY).m_displayObject))
                        m_displayList[key] = new DisplayData(currentLevel.GetTile(levelSpaceX, levelSpaceY).m_displayObject, true);
                }
                else
                {
                    if (m_displayList[key].display.m_spriteChar != m_emptyChar)
                    {
                        m_displayList[key] = new DisplayData(new DisplayObject(m_emptyChar, new Vector2(levelSpaceX, levelSpaceY)), true);
                    }
                }
            }
            
            //loop through moveEntities in level
            //if position is in screen space (x and y are between cameraPos and cameraPos + displayBounds)
            //set display at key entity.pos to entity,displayObject
            for (int i = 0; i < currentLevel.GetNumMovingEntities(); ++i)
            {
                Vector2 tempPos = currentLevel.GetMovingEntity(i).GetCurrentPosition();
                if (tempPos.x >= m_cameraPosition.x && tempPos.x < m_cameraPosition.x + m_displayBounds.x)
                {
                    if (tempPos.y >= m_cameraPosition.y && tempPos.y < m_cameraPosition.y + m_displayBounds.y)
                    {
                        //get entitys screen space location
                        Vector2 screenPos = new Vector2(tempPos.x - m_cameraPosition.x, tempPos.y - m_cameraPosition.y);
                        if (m_displayList.ContainsKey(screenPos))
                            m_displayList[screenPos] = new DisplayData(currentLevel.GetMovingEntity(i).m_displayObject, true);

                    }
                }
            }
        }

        public void Draw(int layer)
        {
            List<Vector2> keys = m_displayList.Keys.ToList();
            
            foreach (Vector2 key in keys)
            {
                if (m_displayList[key].fresh)
                {
                    Vector2 temp = m_displayOffset;
                    temp.x -= m_cameraPosition.x;
                    temp.y -= m_cameraPosition.y;

                    temp.x = m_displayList[key].display.m_displayPosition.x + temp.x;
                    temp.y = m_displayList[key].display.m_displayPosition.y + temp.y;
                    
                    m_fastWrite.AddToBuffer(temp.x, temp.y, layer, m_displayList[key].display.m_spriteChar, m_displayList[key].display.m_foregroudColor, m_displayList[key].display.m_backgroundColor);
                    m_displayList[key] = new DisplayData(m_displayList[key].display, false);
                }
            }

            m_fastWrite.DisplayBuffer();
        }
    }
    
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
