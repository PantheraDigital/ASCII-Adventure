using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AsciiProgram
{
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
            if (other != null && m_spriteChar == other.m_spriteChar && m_foregroudColor == other.m_foregroudColor && m_backgroundColor == other.m_backgroundColor)
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
}
