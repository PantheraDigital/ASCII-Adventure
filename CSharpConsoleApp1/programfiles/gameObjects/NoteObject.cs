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

            ComplexEntity obj = other as ComplexEntity;
            if (obj != null)
            {
                if (obj.HasComponent("Inventory"))
                {
                    obj.GetComponent<Inventory>("Inventory").Add(this);
                }
            }
        }

        public override void EndCollide()
        {
            m_window.Erase();
        }
    }
}
