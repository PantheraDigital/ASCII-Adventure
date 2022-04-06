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


            public NoteWindowData(Vector2 screenPosition, Vector2 windowSize, string message,
                ConsoleColor messageForegroundColor = ConsoleColor.White, ConsoleColor messageBackgroundColor = ConsoleColor.Black,
                char backgroundChar = ' ', ConsoleColor windowForegroundColor = ConsoleColor.White, ConsoleColor windowBackgroundColor = ConsoleColor.Black,
                char borderChar = '\u0000', ConsoleColor borderForeground = ConsoleColor.White, ConsoleColor borderBackground = ConsoleColor.Black)
            {
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


        static int m_currentNote = 0;
        static NoteWindowData[] m_notes = {new NoteWindowData(new Vector2(1,1), new Vector2(9,5), "Hello there!", ConsoleColor.White, ConsoleColor.Black, '-', ConsoleColor.Gray, ConsoleColor.Black, '-', ConsoleColor.Cyan),
            new NoteWindowData(new Vector2(1,1), new Vector2(9, 5), "good bye", ConsoleColor.Red) };



        static int GetCurrentNote()
        {
            if (m_currentNote > m_notes.Length)
                m_currentNote = 0;

            ++m_currentNote;
            return m_currentNote - 1;
        }

        static public GameWindow CreateNoteWindow(Vector2 pos)
        {
            NoteWindowData note = m_notes[GetCurrentNote()];
            GameWindow gameWindow = new GameWindow(note.screenPosition, note.windowSize, note.backgroundChar, note.windowForeground, note.windowBackground);
            gameWindow.SetMessage(note.message, note.messageForeground, note.messageBackground);
            if (note.borderChar != '\u0000')
            {
                gameWindow.SetBorderChar(note.borderChar);
                gameWindow.SetBorderColor(note.borderForeground, note.borderBackground);
            }

            gameWindow.SetTextWrapping(true);
            return gameWindow;
        }
    }
}
