using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class NoteObject : GameObject
    {
        GameWindow m_window;

        public NoteObject(DisplayObject display, GameWindow gameWindow)
            : base(display, false, false)
        {
            m_window = gameWindow;
        }

        public override void OnCollide(MovingEntity other)
        {
            m_window.Draw();
        }

        public override void EndCollide()
        {
            m_window.Erase();
        }
    }
}
