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

        char m_borderChar;
        bool m_useBorder;
        ConsoleColor m_borderForegroundColor;
        ConsoleColor m_borderBackgroundColor;


        bool m_active;
        bool m_updated;
        int m_layer;
        FastWrite m_fastWrite;



        public GameWindow(Vector2 screenPosition, Vector2 windowSize, char backgroundChar = ' ', ConsoleColor windowForegroundColor = ConsoleColor.White, ConsoleColor windowBackgroundColor = ConsoleColor.Black)
        {
            m_screenPosition = screenPosition;
            m_windowSize = windowSize;
            m_backgroundChar = backgroundChar;
            m_borderChar = backgroundChar;

            m_windowForegroundColor = windowForegroundColor;
            m_windowBackgroundColor = windowBackgroundColor;
            m_messageForegroundColor = windowForegroundColor;
            m_messageBackgroundColor = windowBackgroundColor;
            m_borderForegroundColor = windowForegroundColor;
            m_borderBackgroundColor = windowBackgroundColor;

            m_message = "";

            m_textWrapping = false;
            m_useBorder = false;
            m_active = true;

            m_layer = 0;

            m_fastWrite = FastWrite.GetInstance();
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

        public void SetBorderChar(char borderChar)
        {
            m_borderChar = borderChar;
            m_updated = true;
            m_useBorder = true;
        }

        public void SetTextWrapping(bool textWrapping)
        {
            m_textWrapping = textWrapping;
            m_updated = true;
        }

        public void SetUseBorder(bool useBorder)
        {
            m_useBorder = useBorder;
            m_updated = true;
        }

        public void SetMessageColors(ConsoleColor foreground, ConsoleColor background)
        {
            m_messageForegroundColor = foreground;
            m_messageBackgroundColor = background;
            m_updated = true;
        }

        public void SetWindowColors(ConsoleColor foreground, ConsoleColor background)
        {
            m_windowForegroundColor = foreground;
            m_windowBackgroundColor = background;
            m_updated = true;
        }

        public void SetBorderColor(ConsoleColor foreground, ConsoleColor background)
        {
            m_borderForegroundColor = foreground;
            m_borderBackgroundColor = background;
            m_updated = true;
        }

        public void Draw(int layer)
        {
            if (!m_updated && m_active)
                return;


            m_active = true;
            m_layer = layer;

            DrawBackground(layer);
            DrawText(layer);
            /*
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
                        else if (i + 1 < words.Length && (((message[fullLines] + words[i]).Length == bounds.x) ||
                            ((message[fullLines] + words[i] + words[i + 1]).Length > bounds.x)))
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

            }*/

            m_fastWrite.DisplayBuffer();
        }

        void DrawBackground(int layer)
        {
            //fill window with background, include border
            for (int y = 0; y < m_windowSize.y; ++y)
            {
                if (m_useBorder)
                {
                    if (y == 0 || y == m_windowSize.y - 1)
                    {
                        //fill first and last row with border
                        for (int x = 0; x < m_windowSize.x; ++x)
                        {
                            m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, layer, m_borderChar, m_borderForegroundColor, m_borderBackgroundColor);
                        }
                    }
                    else
                    {
                        //fill first and last positions with border
                        for (int x = 0; x < m_windowSize.x; ++x)
                        {
                            if (x == 0 || x == m_windowSize.x - 1)
                                m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, layer, m_borderChar, m_borderForegroundColor, m_borderBackgroundColor);
                            else
                                m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, layer, m_backgroundChar, m_windowForegroundColor, m_windowBackgroundColor);
                        }
                    }

                }
                else
                {
                    for (int x = 0; x < m_windowSize.x; ++x)
                    {
                        m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, layer, m_backgroundChar, m_windowForegroundColor, m_windowBackgroundColor);
                    }
                }
            }
        }

        void DrawText(int layer)//test to see if local save survies without commit
        {
            string messageToPrint = m_message;
            Vector2 bounds = m_windowSize;
            Vector2 pos = m_screenPosition;


            if (m_useBorder)
            {
                pos.x += 1;
                pos.y += 1;
                bounds.x -= 2;
                bounds.y -= 2;
            }


            if (m_textWrapping && messageToPrint.Length > bounds.x)
            {
                if (m_fastWrite.SetCursorPosition(pos))
                {
                    string[] lines = messageToPrint.Split(' ');
                    List<string> formattedMessage = new List<string>();
                    
                    StringBuilder line = new StringBuilder();
                    
                    foreach(string word in lines)
                    {
                        if((line + word).Length <= bounds.x)
                        {
                            line.Append(word + " ");
                        }
                        else if(word.Length > bounds.x)
                        {
                            formattedMessage.Add(line.ToString());
                            line.Clear();
                            StringBuilder cutWord = new StringBuilder();

                            foreach(char letter in word)
                            {
                                if (cutWord.Length == bounds.x || letter == '\n')
                                {
                                    formattedMessage.Add(cutWord.ToString());
                                    cutWord.Clear();
                                }

                                cutWord.Append(letter);
                            }
                            if (cutWord.Length > 0)
                                formattedMessage.Add(cutWord.ToString());
                        }
                        else
                        {
                            formattedMessage.Add(line.ToString());
                            line.Clear();
                            line.Append(word + " ");
                        }
                    }

                    if(line.Length > 0)
                        formattedMessage.Add(line.ToString());

                    //
                    m_fastWrite.AddToBuffer(m_screenPosition.x, m_screenPosition.y - 1, layer, formattedMessage.Count.ToString(), m_messageForegroundColor, m_messageBackgroundColor);
                    m_fastWrite.SetCursorPosition(pos);
                    //
                    int timesToLoop = 0;
                    if (formattedMessage.Count > bounds.y)
                        timesToLoop = bounds.y;
                    else
                        timesToLoop = formattedMessage.Count;

                    for (int i = 0; i < timesToLoop; ++i)
                    {
                        int numLineBreaks = 0;
                        //++pos.y;

                        for(int x = 0; x < formattedMessage[i].Length; ++x)
                        {
                            if(formattedMessage[i][x] == '\n')
                            {
                                ++pos.y;
                                ++numLineBreaks;
                                if (!m_fastWrite.SetCursorPosition(pos) || numLineBreaks + 1 > bounds.y)
                                    break;
                            }

                            if (x - (numLineBreaks * bounds.x) >= bounds.x)
                                break;

                            if (formattedMessage[i][x] != '\n')
                                m_fastWrite.AddToBuffer(layer, formattedMessage[i][x], m_messageForegroundColor, m_messageBackgroundColor);
                        }
                        
                    }
                }
            }
            else
            {
                if (m_fastWrite.SetCursorPosition(pos))
                {
                    int numLineBreaks = 0;
                    for (int i = 0; i < messageToPrint.Length; ++i)
                    {
                        if (messageToPrint[i] == '\n')
                        {
                            ++pos.y;
                            ++numLineBreaks;
                            if (!m_fastWrite.SetCursorPosition(pos) || numLineBreaks + 1 > bounds.y)
                                break;
                        }

                        if (i - (numLineBreaks * bounds.x) >= bounds.x)
                            break;

                        if (messageToPrint[i] != '\n')
                            m_fastWrite.AddToBuffer(layer, messageToPrint[i], m_messageForegroundColor, m_messageBackgroundColor);
                    }
                }
            }

        }

        bool InWindowSpace(Vector2 relativePosition)
        {
            if (relativePosition.x >= 0 && relativePosition.x <= m_windowSize.x)
                if (relativePosition.y >= 0 && relativePosition.y <= m_windowSize.y)
                    return true;

            return false;
        }


    }
}