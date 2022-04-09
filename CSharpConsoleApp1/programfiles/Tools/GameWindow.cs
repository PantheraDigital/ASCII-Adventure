using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FastConsole;


namespace AsciiProgram
{
    public class GameWindow
    {
        Vector2 m_screenPosition;
        Vector2 m_windowSize;
        Vector2 m_textBounds;

        char m_backgroundChar;
        ConsoleColor m_windowForegroundColor;
        ConsoleColor m_windowBackgroundColor;

        TextBoxFormatter m_messageFormatter;
        string m_message;
        ConsoleColor m_messageForegroundColor;
        ConsoleColor m_messageBackgroundColor;

        char m_borderChar;
        bool m_useBorder;
        ConsoleColor m_borderForegroundColor;
        ConsoleColor m_borderBackgroundColor;


        bool m_active;
        bool m_updated;
        string m_windowName;
        FastWrite m_fastWrite;



        public GameWindow(string windowName, Vector2 screenPosition, Vector2 windowSize, char backgroundChar = ' ', ConsoleColor windowForegroundColor = ConsoleColor.White, ConsoleColor windowBackgroundColor = ConsoleColor.Black)
        {
            m_windowName = windowName;
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

            m_useBorder = false;
            m_active = true;

            m_fastWrite = FastWrite.GetInstance();
            m_messageFormatter = new TextBoxFormatter();

            m_textBounds = m_windowSize;

            if (m_useBorder)
            {
                m_textBounds.x -= 2;
                m_textBounds.y -= 2;
            }
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
            m_fastWrite.ClearLayer(m_windowName);
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
                UpdateTextBounds();
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

            UpdateTextBounds();
        }
        public void SetMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            m_message = message;
            m_updated = true;

            SetMessageColors(foregroundColor, backgroundColor);
            UpdateTextBounds();
        }

        public void AddToMessage(string text)
        {
            if (text != null)
            {
                m_message += text;
                m_updated = true;
                UpdateTextBounds();
            }
        }
        public void RemoveFromMessage(string text)
        {
            if (text != null && m_message.Contains(text))
            {
                m_message.Remove(m_message.IndexOf(text), text.Length);
                m_updated = true;
                UpdateTextBounds();
            }
        }

        public void SetBorderChar(char borderChar)
        {
            m_borderChar = borderChar;
            m_updated = true;
            m_useBorder = true;
            UpdateTextBounds();
        }

        public void SetUseBorder(bool useBorder)
        {
            m_useBorder = useBorder;
            m_updated = true;
            UpdateTextBounds();
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

        public void Draw()
        {
            if (!m_updated && m_active)
                return;


            m_active = true;

            DrawBackground();
            DrawText();
        }

        void DrawBackground()
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
                            m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, m_windowName, m_borderChar, m_borderForegroundColor, m_borderBackgroundColor);
                        }
                    }
                    else
                    {
                        //fill first and last positions with border
                        for (int x = 0; x < m_windowSize.x; ++x)
                        {
                            if (x == 0 || x == m_windowSize.x - 1)
                                m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, m_windowName, m_borderChar, m_borderForegroundColor, m_borderBackgroundColor);
                            else
                                m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, m_windowName, m_backgroundChar, m_windowForegroundColor, m_windowBackgroundColor);
                        }
                    }

                }
                else
                {
                    for (int x = 0; x < m_windowSize.x; ++x)
                    {
                        m_fastWrite.AddToBuffer(m_screenPosition.x + x, m_screenPosition.y + y, m_windowName, m_backgroundChar, m_windowForegroundColor, m_windowBackgroundColor);
                    }
                }
            }
        }

        void DrawText()
        {
            Vector2 pos = m_screenPosition;

            if (m_useBorder)
            {
                pos.x += 1;
                pos.y += 1;
            }

            int timesToLoop = 0;
            List<string> formattedMessage = m_messageFormatter.GetFormattedText();

            if (formattedMessage.Count > m_textBounds.y)
                timesToLoop = m_textBounds.y;
            else
                timesToLoop = formattedMessage.Count;


            for (int i = 0; i < timesToLoop; ++i)
            {
                m_fastWrite.SetCursorPosition(pos);

                if (formattedMessage[i].Length > 0)
                    m_fastWrite.AddToBuffer(m_windowName, formattedMessage[i], m_messageForegroundColor, m_messageBackgroundColor);

                pos.y += 1;
            }
        }

        void UpdateTextBounds()
        {
            if(m_useBorder)
            {
                Vector2 temp = m_windowSize;
                temp.x -= 2;
                temp.y -= 2;

                m_textBounds = temp;                                    
            }
            else
            {
                m_textBounds = m_windowSize;            
            }

            m_messageFormatter.FormatText(m_message, m_textBounds);
        }
    }
}