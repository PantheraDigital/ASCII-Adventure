using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        bool m_updated;
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


        public char GetBackgroundCharacter()
        {
            return m_backgroundChar;
        }

        public Vector2 GetWindowSize()
        {
            return m_windowSize;
        }

        public void Erase()
        {
            m_active = false;
            m_fastWrite.ClearLayer(m_layer);
        }

        public bool IsActive()
        {
            return m_active;
        }

        public void SetWindowSize(Vector2 size)
        {
            if (size.x > 0 && size.y > 0)
            {
                m_windowSize = size;
                m_updated = true;
            }
        }

        public Vector2 GetScreenPosition()
        {
            return m_screenPosition;
        }

        public void SetScreenPosition(Vector2 position)
        {
            if (position.x > 0 && position.y > 0)
            {
                m_screenPosition = position;
                m_updated = true;
            }
        }

        public void SetMessage(string message)
        {
            m_message = message;
            m_updated = true;
        }
        public void SetMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            m_message = message;
            m_updated = true;
            SetMessageColors(foregroundColor, backgroundColor);
        }

        public void SetTextWrapping(bool textWrapping)
        {
            m_textWrapping = textWrapping;
            m_updated = true;
        }

        public void SetMessageColors(ConsoleColor foreground, ConsoleColor background)
        {
            m_messageForegroundColor = foreground;
            m_messageBackgroundColor = background;
            m_updated = true;
        }

        public void SetWindowColros(ConsoleColor foreground, ConsoleColor background)
        {
            m_windowForegroundColor = foreground;
            m_windowBackgroundColor = background;
            m_updated = true;
        }

        public void Draw(int layer)
        {
            if (!m_updated && m_active)
                return; 


            m_active = true;
            m_layer = layer;

            //fill window with background
            for (int y = 0; y < m_windowSize.y; ++y)
            {
                for (int x = 0; x < m_windowSize.x; ++x)
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
}
