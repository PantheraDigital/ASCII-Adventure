using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{

    public abstract class GameObject
    {
        public DisplayObject m_displayObject { get; protected set; }
        public bool m_solid { get; protected set; }

        public GameObject(DisplayObject display, bool solid)
        {
            m_displayObject = display;
            m_solid = solid;
        }

        public virtual void Update() { }
        public virtual void OnCollide(MovingEntity other) { }
        public virtual void EndCollide() { }
        public virtual void OnDraw() { }

    }

    public class NoteObject : GameObject
    {
        GameWindow m_window;
        bool m_active;

        public NoteObject(DisplayObject display, GameWindow gameWindow)
            :base(display, false)
        {
            m_window = gameWindow;
        }

        public override void OnCollide(MovingEntity other)
        {
            m_active = true;

            m_window.Draw(4);
        }

        public override void EndCollide()
        {
            m_active = false;
        }

        public override void OnDraw()
        {
            if (m_active)
                m_window.Draw(4);
            else
                m_window.Erase();
        }
    }
}
