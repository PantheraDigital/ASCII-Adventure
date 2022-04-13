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
        protected string m_tags;
        public DisplayObject m_displayObject { get; protected set; }


        public Entity(DisplayObject displayObject, Controller controller, string tags = "none")
        {
            m_displayObject = displayObject;
            m_controller = controller;
            m_tags = tags;
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

        public string GetTags()
        {
            return m_tags;
        }

        public bool HasTag(string tag)
        {
            return m_tags.ToLower().Contains(m_tags.ToLower());
        }
    }
}