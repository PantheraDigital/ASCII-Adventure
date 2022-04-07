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
        public bool m_pickUp { get; protected set; }
        public string m_name { get; protected set; }

        public GameObject(DisplayObject display, bool solid)
        {
            m_displayObject = display;
            m_solid = solid;
            m_pickUp = false;
        }

        public virtual void Update() { }
        public virtual void OnCollide(MovingEntity other) { }
        public virtual void EndCollide() { }
        public virtual void OnDraw() { }

    }

    
}
