using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public abstract class Component
    {
        string m_type;

        public Component(string type)
        {
            m_type = type;
        }

        public string GetComponentType()
        {
            return m_type;
        }

        public virtual void Draw() { }
    }

    public interface IComponentManager
    {
        bool HasComponent(string type);
        T GetComponent<T>(string type);//return component in its type
        bool AddComponent(Component component);
        void DrawComponents();
    }
}
