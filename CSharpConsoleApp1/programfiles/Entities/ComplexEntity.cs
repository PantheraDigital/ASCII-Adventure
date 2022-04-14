using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{

    public class ComplexEntity : MovingEntity, IComponentManager
    {
        Dictionary<string, Component> m_components;

        public ComplexEntity(DisplayObject displayObject, Controller controller, string tags = "none")
            : base(displayObject, controller, tags)
        {
            m_components = new Dictionary<string, Component>();
        }

        public bool HasComponent(string type)
        {
            if (m_components.ContainsKey(type))
                return true;
            else
                return false;
        }

        public T GetComponent<T>(string type)
        {
            if (m_components.ContainsKey(type))
                return (T)Convert.ChangeType(m_components[type], typeof(T));
            else
                return default(T);
        }

        public bool AddComponent(Component component)
        {
            if (m_components.ContainsKey(component.GetComponentType()))
                return false;
            else
            {
                m_components.Add(component.GetComponentType(), component);
                return true;
            }
        }
        public void DrawComponents()
        {
            foreach (string key in m_components.Keys)
            {
                m_components[key].Draw();
            }
        }
    }

}
