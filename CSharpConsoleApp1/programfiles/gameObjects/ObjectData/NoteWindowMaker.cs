using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    static class NoteWindowMaker
    {
        struct NoteWindowData
        {
            public string noteName;
            public Vector2 screenPosition;
            public Vector2 windowSize;
            public string message;
            public ConsoleColor messageForeground;
            public ConsoleColor messageBackground;
            public char backgroundChar;
            public ConsoleColor windowForeground;
            public ConsoleColor windowBackground;
            public char borderChar;
            public ConsoleColor borderForeground;
            public ConsoleColor borderBackground;


            public NoteWindowData(string noteName, Vector2 screenPosition, Vector2 windowSize, string message,
                ConsoleColor messageForegroundColor = ConsoleColor.White, ConsoleColor messageBackgroundColor = ConsoleColor.Black,
                char backgroundChar = ' ', ConsoleColor windowForegroundColor = ConsoleColor.White, ConsoleColor windowBackgroundColor = ConsoleColor.Black,
                char borderChar = '\u0000', ConsoleColor borderForeground = ConsoleColor.White, ConsoleColor borderBackground = ConsoleColor.Black)
            {
                this.noteName = noteName;
                this.screenPosition = screenPosition;
                this.windowSize = windowSize;
                this.message = message;
                messageForeground = messageForegroundColor;
                messageBackground = messageBackgroundColor;
                this.backgroundChar = backgroundChar;
                windowForeground = windowForegroundColor;
                windowBackground = windowBackgroundColor;
                this.borderChar = borderChar;
                this.borderForeground = borderForeground;
                this.borderBackground = borderBackground;
            }
        }

        struct LevelNotes
        {
            public static NoteWindowData[] mazeLevel1Notes = { new NoteWindowData("helloNote", new Vector2(1,1), new Vector2(9,5), "Hello there!", ConsoleColor.White, ConsoleColor.Black, '-', ConsoleColor.Gray, ConsoleColor.Black, '-', ConsoleColor.Cyan),
                new NoteWindowData("byeMessage", new Vector2(1,1), new Vector2(9, 5), "good bye", ConsoleColor.Red) };


            public static NoteWindowData[] mazeLevel2Notes = { new NoteWindowData("hiNote", new Vector2(1, 1), new Vector2(9, 5), "Hello\nthere!", ConsoleColor.White, ConsoleColor.Black, '-', ConsoleColor.Gray, ConsoleColor.Black, '-', ConsoleColor.Red) };

        }

        //  /////////////////////////
        //  make non static by adding the level set up funcs in program to a class for level creation then add this as an object inside allong with levelChangeTracker
        //  /////////////////////////

        static int m_currentNote = 0;
        static string m_currentLevelName = "null";
        static NoteWindowData[] m_currentLevelNotes;    
        

        static void SetCurrentLevel(string levelName)
        {
            if(!levelName.Equals(m_currentLevelName))
            {
                m_currentLevelName = levelName;
                m_currentNote = 0;

                switch (levelName)
                {
                    case "mazeLevel1":
                        m_currentLevelNotes = LevelNotes.mazeLevel1Notes;
                        break;

                    case "mazeLevel2":
                        m_currentLevelNotes = LevelNotes.mazeLevel2Notes;
                        break;

                    default:
                        m_currentLevelNotes = null;
                        break;
                }
            }
        }

        static int GetCurrentNote(string levelName)
        {
            SetCurrentLevel(levelName);
            if (m_currentLevelNotes == null || m_currentNote >= m_currentLevelNotes.Length)
                m_currentNote = -1;

            ++m_currentNote;
            return m_currentNote - 1;
        }

        static public GameWindow CreateNoteWindow(string levelName)
        {
            int index = GetCurrentNote(levelName);

            if (index == -1)
                return null;

            NoteWindowData note = m_currentLevelNotes[index];
            
            GameWindow gameWindow = new GameWindow(note.noteName, note.screenPosition, note.windowSize, note.backgroundChar, note.windowForeground, note.windowBackground);
            gameWindow.SetMessage(note.message, note.messageForeground, note.messageBackground);
            if (note.borderChar != '\u0000')
            {
                gameWindow.SetBorderChar(note.borderChar);
                gameWindow.SetBorderColor(note.borderForeground, note.borderBackground);
            }
            return gameWindow;
        }
    }
}
