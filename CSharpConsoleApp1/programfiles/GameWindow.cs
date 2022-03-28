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
        List<string> m_formattedMessage;
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
            m_formattedMessage = new List<string>();
            m_formattedMessage.Add("");
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
            
            List<string> formattedMessage = FormatMessage(m_message, bounds, m_textWrapping);
            /*
            FormatMessage(m_message, bounds, m_textWrapping);
            string debugtext = "lines" + m_formattedMessage.Count.ToString();

            for (int i = 0; i < m_formattedMessage.Count; ++i)
            {
                debugtext = debugtext + "-" + m_formattedMessage[i];

                m_fastWrite.SetCursorPosition(pos);
                if (m_formattedMessage[i].Length > 0)
                    m_fastWrite.AddToBuffer(layer, m_formattedMessage[i].Trim(' '), m_messageForegroundColor, m_messageBackgroundColor);

                pos.y += 1;
            }

            m_fastWrite.AddToBuffer(m_screenPosition.x, m_screenPosition.y - 1, layer, debugtext, m_messageForegroundColor, m_messageBackgroundColor);*/
            //m_formattedMessage[10].Trim();

            int timesToLoop = 0;
            if (formattedMessage.Count > bounds.y)
                timesToLoop = bounds.y;
            else
                timesToLoop = formattedMessage.Count;

            for (int i = 0; i < timesToLoop; ++i)
            {
                m_fastWrite.SetCursorPosition(pos);

                if (m_formattedMessage[i].Length > 0)
                    m_fastWrite.AddToBuffer(layer, m_formattedMessage[i].Trim(' '), m_messageForegroundColor, m_messageBackgroundColor);
            }
        }

        /*bool InWindowSpace(Vector2 relativePosition)
        {
            if (relativePosition.x >= 0 && relativePosition.x <= m_windowSize.x)
                if (relativePosition.y >= 0 && relativePosition.y <= m_windowSize.y)
                    return true;

            return false;
        }*/

        List<string> FormatMessage(string messageToPrint, Vector2 bounds, bool textWrapping)
        {
            
            List<string> formattedMessage = new List<string>();
            /*
            string[] lines = messageToPrint.Split('\n');
            foreach(string line in lines)
            {
                AddLineToFormattedMessage(line, bounds, textWrapping);
            }*/

            /*string[] words = messageToPrint.Split(' ');
            
            foreach (string word in words)
            {
                AddWordToFormattedMessage(word, bounds, textWrapping);
            }*/


            if (textWrapping && messageToPrint.Length > bounds.x)
            {
                string[] lines = messageToPrint.Split(' ');
                StringBuilder line = new StringBuilder();

                foreach (string word in lines)
                {
                    if(word.Contains('\n') || word.Length > bounds.x)
                    {
                        string cutWord = "";
                        foreach(char letter in word)
                        {
                            bool addWord = false;
                            if (letter == '\n')
                            {
                                addWord = true;
                            }
                            else
                                cutWord = cutWord + letter;

                            if ((cutWord + line).Length == bounds.x)
                                addWord = true;

                            if(addWord)
                            {
                                if ((cutWord + line).Length <= bounds.x)
                                    line.Append(cutWord + " ");
                                else
                                {
                                    formattedMessage.Add(line.ToString().Trim());
                                    line.Clear();
                                    line.Append(cutWord);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((line + word).Length <= bounds.x)//word fits
                        {
                            line.Append(word + " ");
                        }
                        else //word in line too large
                        {
                            if (line.Length > 0)
                            {
                                formattedMessage.Add(line.ToString().Trim());
                                line.Clear();
                            }

                            line.Append(word + " ");
                        }
                    }


                }

                if (line.Length > 0 && line.Length <= bounds.x)
                {
                    formattedMessage.Add(line.ToString().Trim());
                }
            }
            else
            {
                int numLineBreaks = 0;
                string line = "";
                foreach(char letter in messageToPrint)
                {
                    if (letter == '\n')
                    {
                        ++numLineBreaks;

                        formattedMessage.Add(line);
                        line = "";
                    }

                    if ((line + letter).Length > bounds.x || formattedMessage.Count >= bounds.y)
                        break;

                    if (letter != '\n')
                        line = line + letter;
                }

                if (line.Length > 0 && line.Length <= bounds.x)
                    formattedMessage.Add(line);
            }

            return formattedMessage;
        }

        void AddWordToFormattedMessage(string word, Vector2 bounds, bool textWrapping)
        {
            int maxIndex = m_formattedMessage.Count - 1;

            if (maxIndex < 0)
                maxIndex = 0;
         
            //word does not fit
            if(m_formattedMessage.Count >= bounds.y && (word + m_formattedMessage[maxIndex]).Length > bounds.x)
                return;

            
            //no line break and word fits in a line
            if (!word.Contains('\n') && word.Length <= bounds.x)
            {
                //word fits in line
                if ((word + m_formattedMessage[maxIndex]).Length <= bounds.x)
                {
                    if ((word + m_formattedMessage[maxIndex]).Length + 1 <= bounds.x)
                        m_formattedMessage[maxIndex] = m_formattedMessage[maxIndex] + word + " ";
                    else
                        m_formattedMessage[maxIndex] = m_formattedMessage[maxIndex] + word;

                }
                else
                {
                    if(textWrapping && m_formattedMessage.Count <= bounds.y)
                    {
                        //m_formattedMessage[maxIndex] = m_formattedMessage[maxIndex].Trim();
                        m_formattedMessage.Add(word);
                    }
                }
            }
            else//word has line break or longer than line
            {
                StringBuilder cutWord = new StringBuilder();

                foreach (char letter in word)
                {
                    if (letter == '\n')
                    {
                        if (cutWord.Length > 0)
                        {
                            AddWordToFormattedMessage(cutWord.ToString(), bounds, textWrapping);
                            cutWord.Clear();
                        }
                        if (m_formattedMessage.Count < bounds.y)
                            m_formattedMessage.Add("");
                    }


                    if (letter != '\n')
                    {
                        if (!textWrapping && (cutWord + m_formattedMessage[maxIndex]).Length == bounds.x)
                        {
                            break;
                        }

                        cutWord.Append(letter);
                    }
                        
                
                    

                
                }

                if(cutWord.Length > 0)
                {
                    AddWordToFormattedMessage(cutWord.ToString(), bounds, textWrapping);
                    
                }
            }
        }

        void AddLineToFormattedMessage(string line, Vector2 bounds, bool textWrapping, bool addToLastLine = false)
        {
            string[] words = line.Split(' ');
            StringBuilder formattedLine = new StringBuilder();


            if(textWrapping)
            {
                foreach (string word in words)
                {

                }
            }
            else
            {
                int numLineBreaks = 0;
                foreach (char letter in line)
                {
                    if (letter == '\n')
                    {
                        ++numLineBreaks;

                        m_formattedMessage.Add(formattedLine.ToString());
                        formattedLine.Clear();
                    }

                    if ((formattedLine.Length + 1) > bounds.x || m_formattedMessage.Count >= bounds.y)
                        break;

                    if (letter != '\n')
                        formattedLine.Append(letter);
                }

                if (formattedLine.Length > 0 && formattedLine.Length <= bounds.x)
                    m_formattedMessage.Add(formattedLine.ToString());
            }

        }
    }
}