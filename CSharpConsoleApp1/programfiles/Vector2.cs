using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram//CSharpConsoleApp1.programfiles
{
    public struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }

        public bool IsEqual(Vector2 other)
        {
            if (other.x == x && other.y == y)
                return true;
            else
                return false;
        }
    }
}
