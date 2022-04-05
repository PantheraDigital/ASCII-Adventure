using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public abstract class Entity//concrete entities will define what controllers do when given input such as move or act some way
    {
        protected Controller m_controller;
        public DisplayObject m_displayObject { get; protected set; }


        public Entity(DisplayObject displayObject, Controller controller)
        {
            m_displayObject = displayObject;
            m_controller = controller;
        }

        public Controller GetController()
        {
            return m_controller;
        }

        public abstract void Update();

        public virtual Vector2 GetCurrentPosition()
        {
            return m_displayObject.m_displayPosition;
        }

        public virtual void SetPosition(Vector2 position)
        {
            m_displayObject.m_displayPosition = position;
        }
    }
}