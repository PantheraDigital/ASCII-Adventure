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
        //bool m_active;

        public NoteObject(DisplayObject display, GameWindow gameWindow)
            : base(display, false)
        {
            m_window = gameWindow;
        }

        public override void OnCollide(MovingEntity other)
        {
            //m_active = true;

            m_window.Draw(4);
        }

        public override void EndCollide()
        {
            //m_active = false;

            m_window.Erase();
        }

        public override void OnDraw()
        {
            m_window.Draw(4);
            Console.ReadKey(true);
            m_window.Erase();
        }
    }
}
