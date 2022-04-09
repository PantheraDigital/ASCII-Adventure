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
        public string m_tags { get; protected set; }

        public GameObject(DisplayObject display, bool solid, string name = "none", string tags = "none")
        {
            m_displayObject = display;
            m_solid = solid;
            m_pickUp = false;

            if (name != "none")
                m_name = name;
            else
                m_name = "";

            if (tags != "none")
                m_tags = tags;
            else
                m_tags = "";
        }

        public virtual void Update() { }
        public virtual void OnCollide(MovingEntity other) { }
        public virtual void EndCollide() { }
        public virtual void OnDraw() { }

    }

    
}
